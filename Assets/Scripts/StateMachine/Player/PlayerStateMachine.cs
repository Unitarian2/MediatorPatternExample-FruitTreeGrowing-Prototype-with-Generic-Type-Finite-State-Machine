using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    //References
    PlayerInputAction playerInputAction;
    CharacterController characterController;
    Animator animator;

    //AnimatorHash for Performance
    int isWalkingHash;
    int isRunningHash;
    int isJumpingHash;
    int jumpCountHash;
    int isFallingHash;

    //Player input values
    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;
    Vector3 appliedMovement;
    Vector3 cameraRelativeMovement;
    bool isMovementPressed;
    bool isRunPressed;
    float rotationSpeed = 15f;

    //Changleable variables
    [Header("-Run Settings-")]
    [SerializeField] private float runSpeed = 3.0f;
    [Header("-Jump Settings-")]
    public float maxJumpTime = 0.5f;
    public float maxJumpHeight = 1.0f;
    public float fallSpeed = 2.0f;

    //gravity
    float gravity = -9.8f;
    

    //jump variables
    bool isJumpPressed = false;
    float initialJumpVelocity;
    bool isJumping = false;
    bool requireNewJumpPress = false;
    int jumpCount = 0;
    Dictionary<int, float> initialJumpVelocities = new();
    Dictionary<int, float> jumpGravities = new();
    Coroutine jumpResetCoroutine = null;

    //States
    PlayerBaseState currentState;
    PlayerStateFactory states;

    //properties
    public CharacterController CharacterController => characterController;
    public PlayerBaseState CurrentState { get { return currentState; } set { currentState = value; } }
    public Animator Animator => animator;
    public Coroutine JumpResetCoroutine { get { return jumpResetCoroutine; } set { jumpResetCoroutine = value; } }
    public Dictionary<int,float> InitialJumpVelocities => initialJumpVelocities;
    public Dictionary<int,float> JumpGravities => jumpGravities;
    public int JumpCount { get { return jumpCount; } set { jumpCount = value; } }
    public int IsWalkingHash => isWalkingHash;
    public int IsRunningHash => isRunningHash;
    public int IsJumpingHash => isJumpingHash;
    public int IsFallingHash => isFallingHash;
    public int JumpCountHash => jumpCountHash;
    public bool RequireNewJumpPress { get { return requireNewJumpPress; } set { requireNewJumpPress = value; } }
    public bool IsJumping { set => isJumping = value; }
    public bool IsMovementPressed => isMovementPressed;
    public bool IsRunPressed => isRunPressed;
    public bool IsJumpPressed => isJumpPressed;
    public float RunSpeed => runSpeed;
    public float FallSpeed => fallSpeed;
    public float Gravity => gravity;
    public Vector2 CurrentMovementInput => currentMovementInput;
    public float CurrentMovementY { get { return currentMovement.y; } set { currentMovement.y = value; } }
    public float AppliedMovementX { get { return appliedMovement.x; } set { appliedMovement.x = value; } }
    public float AppliedMovementY { get { return appliedMovement.y; } set { appliedMovement.y = value; } }
    public float AppliedMovementZ { get { return appliedMovement.z; } set { appliedMovement.z = value; } }
    public PlayerInputAction PlayerInputAction => playerInputAction;

    private void Awake()
    {
        //Get References
        playerInputAction = new PlayerInputAction();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        //setup state
        states = new PlayerStateFactory(this);
        currentState = states.Grounded();
        currentState.EnterState();

        //AnimatorHash for Performance
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");
        jumpCountHash = Animator.StringToHash("jumpCount");
        isFallingHash = Animator.StringToHash("isFalling");

        //Input System Callbacks
        playerInputAction.CharacterControls.Move.started += MovementActionCallback;
        playerInputAction.CharacterControls.Move.canceled += MovementActionCallback;
        playerInputAction.CharacterControls.Move.performed += MovementActionCallback;
        playerInputAction.CharacterControls.Run.started += RunActionCallback;
        playerInputAction.CharacterControls.Run.canceled += RunActionCallback;
        playerInputAction.CharacterControls.Jump.started += JumpActionCallback;
        playerInputAction.CharacterControls.Jump.canceled += JumpActionCallback;

        SetupJump();
    }

    private void SetupJump()
    {
        //Burada istenen jump de�erleri i�in hangi gravity ve hangi jump velocity ihtiyac�m�z var onu hesapl�yoruz.
        float timeToApex = maxJumpTime * 0.5f;
        float initialGravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;

        //2. ve 3. z�plama stillerinin Gravity ve Velocity de�erlerini ayarl�yoruz.
        float secondJumpGravity = (-2 * maxJumpHeight + 0.25f) / Mathf.Pow(timeToApex * 1.05f, 2);
        float secondInitialJumpVelocity = (2 * maxJumpHeight + 0.25f) / timeToApex * 1.05f;
        float thirdJumpGravity = (-2 * maxJumpHeight + 0.5f) / Mathf.Pow(timeToApex * 1.1f, 2);
        float thirdInitialJumpVelocity = (2 * maxJumpHeight + 0.5f) / timeToApex * 1.1f;

        //Dictionary'leri dolduruyoruz. jumpGravities'deki 0. eleman jump count resetlendi�inde kullanabilmek i�in
        initialJumpVelocities.Add(1, initialJumpVelocity);
        initialJumpVelocities.Add(2, secondInitialJumpVelocity);
        initialJumpVelocities.Add(3, thirdInitialJumpVelocity);
        jumpGravities.Add(0, initialGravity);
        jumpGravities.Add(1, initialGravity);
        jumpGravities.Add(2, secondJumpGravity);
        jumpGravities.Add(3, thirdJumpGravity);

    }
    private void Start()
    {
        characterController.Move(appliedMovement * Time.deltaTime);
    }

    void JumpActionCallback(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
        requireNewJumpPress = false;
    }

    void RunActionCallback(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    void MovementActionCallback(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();

        ////Player X ve Z axis'inde hareket ediyor.
        //currentMovement.x = currentMovementInput.x;
        //currentMovement.z = currentMovementInput.y;//Unity'de Y vekt�r� vertical position oldu�u i�in Z axis'inde sakl�yoruz.
        //currentRunMovement.x = currentMovementInput.x * runSpeed;
        //currentRunMovement.z = currentMovementInput.y * runSpeed;//Unity'de Y vekt�r� vertical position oldu�u i�in Z axis'inde sakl�yoruz.
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;//S�f�r d���nda bir value geldiyse hareket etme tu�una bas�lm��t�r.
    }

    void HandleRotation()
    {
        Vector3 positionToLookAt;
        //Player hareket etti�i y�ne do�ru bakmal�
        positionToLookAt.x = cameraRelativeMovement.x;
        positionToLookAt.y = 0;
        positionToLookAt.z = cameraRelativeMovement.z;
        

        Quaternion currentRotation = transform.rotation;

        if (isMovementPressed)//Hareket etmiyorken d�nm�yoruz.
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            //Snappy d�n�� hareketleri olmamas� i�in Lerp kulland�k.
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

    }

    void Update()
    {
        HandleRotation();
        currentState.UpdateStates();

        cameraRelativeMovement = ConvertToCameraSpace(appliedMovement);
        
        characterController.Move(cameraRelativeMovement * Time.deltaTime);
    }

    //input system'dan gelen world space temelli hareket direction'�n� kamera space temelli hale getirir. Bunu karakteri kameran�n bakt��� y�ne d�nd�rmek i�in kullan�yoruz.
    Vector3 ConvertToCameraSpace(Vector3 vectorToRotate)
    {
        //Y de�erini sakl�yoruz. ��lemler yap�l�rken Y axis'i g�z ard� edilecek ama movement i�in Y de�eri yine de laz�m. En son vekt�re eklicez bunu.
        float currentYValue = vectorToRotate.y;

        //Camera'n�n X ve Z vekt�rlerini ald�k.
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        //Y de�erini s�f�rl�yoruz b�ylece kamera X ve Z vekt�rlerine paralel bir hale geliyor. Yukar� a�a�� kamera a��s�n� ignore etmi� olduk.
        cameraForward.y = 0;
        cameraRight.y = 0;

        //Y de�eri s�f�rlay�nca vekt�r k�salm�� oldu. Uzunluk 1 olmal� vekt�rlerin, o nedenle normalize ediyoruz.
        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;

        //vectorToRotate X ve Z de�erlerini camera space temelli olarak d�nd�r�yoruz.
        Vector3 cameraForwardZProduct = vectorToRotate.z * cameraForward;
        Vector3 cameraRightXProduct = vectorToRotate.x * cameraRight;

        //iki vekt�r�n toplam�, kamera temelli vekt�r�m�z oluyor.
        Vector3 vectorRotatedToCameraSpace = cameraForwardZProduct + cameraRightXProduct;
        vectorRotatedToCameraSpace.y = currentYValue;
        return vectorRotatedToCameraSpace;
    }

    private void OnEnable()
    {
        playerInputAction.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        playerInputAction.CharacterControls.Disable();
    }
}
