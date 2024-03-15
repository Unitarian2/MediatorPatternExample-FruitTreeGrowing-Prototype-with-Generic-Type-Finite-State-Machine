using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
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


    //Player input values
    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;
    Vector3 appliedMovement;
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
    float groundedGravity = -.05f;

    //jump variables
    bool isJumpPressed = false;
    float initialJumpVelocity; 
    bool isJumping = false;
    bool isJumpAnimating = false;
    int jumpCount = 0;
    Dictionary<int, float> initialJumpVelocities = new();
    Dictionary<int, float> jumpGravities = new();
    Coroutine jumpResetCoroutine = null;

    private void Awake()
    {
        //Get References
        playerInputAction = new PlayerInputAction();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        //AnimatorHash for Performance
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");
        jumpCountHash = Animator.StringToHash("jumpCount");

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
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
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
        jumpGravities.Add(0, gravity);
        jumpGravities.Add(1, gravity);
        jumpGravities.Add(2, secondJumpGravity);
        jumpGravities.Add(3, thirdJumpGravity);

    }

    void JumpActionCallback(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }

    void RunActionCallback(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    void MovementActionCallback(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();

        //Player X ve Z axis'inde hareket ediyor.
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;//Unity'de Y vekt�r� vertical position oldu�u i�in Z axis'inde sakl�yoruz.
        currentRunMovement.x = currentMovementInput.x * runSpeed;
        currentRunMovement.z = currentMovementInput.y * runSpeed;//Unity'de Y vekt�r� vertical position oldu�u i�in Z axis'inde sakl�yoruz.
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;//S�f�r d���nda bir value geldiyse hareket etme tu�una bas�lm��t�r.
    }

    void HandleJump()
    {
        //�uan z�plama an�nda de�ilsek ve karakter yerdeyse ve Jump butonu bas�lm��sa, z�pl�yoruz.
        if(!isJumping && characterController.isGrounded && isJumpPressed)
        {
            //Jump combosu tamamlanmam��sa ResetTimer durduruyoruz ki jumpCount combonun ortas�nda s�f�rlanmas�n.
            if(jumpCount < 3 && jumpResetCoroutine != null)
            {
                StopCoroutine(jumpResetCoroutine);
            }
            //Dictionary d���na ��kmamak i�in ekledik
            else if (jumpCount >= 3)
            {
                jumpCount = 0;
                if(jumpResetCoroutine != null) StopCoroutine(jumpResetCoroutine);
            }
           
            

            //Animation ba�l�yor.
            animator.SetBool(isJumpingHash, true);
            isJumpAnimating = true;

            isJumping = true;
            jumpCount += 1;
            animator.SetInteger(jumpCountHash, jumpCount);

            //HandleGravity'de Velocity Verlet(yar�m ad�ml�k) hesaplama yapt���m�z i�in, initialJumpVelocity'yi yar� yar�ya d���rd�k.
            currentMovement.y = initialJumpVelocities[jumpCount];
            appliedMovement.y = initialJumpVelocities[jumpCount];

        }
        //�uan z�plama an�ndaysak ve karakter yerdeyse ve Jump butonu bas�lmam��sa, z�plama hen�z yeni bitmi�tir.
        else if(isJumping && characterController.isGrounded && !isJumpPressed)
        {
            isJumping = false;
        }
    }

    IEnumerator JumpResetTimer()
    {
        yield return new WaitForSeconds(0.5f);
        jumpCount = 0;
    }

    void HandleGravity()
    {
        //sadece havaya y�kselirken y 0'dan b�y�k olur.
        //Jump butonuna basmay� b�rak�rsak k���k jump(fallMultiplier 1f oldu�u i�in), bas�l� tutarsak ise full jump(fallMultiplier artt��� i�in)
        bool isFalling = currentMovement.y <= 0f || !isJumpPressed;

        //D���yorsak, public fallspeed de�eri kullan�yoruz. Yoksa 1.0f default de�er kullan�yoruz. D��me h�z�n� art�rmak i�in kullan�l�r.
        float fallMultiplier = fallSpeed;

        //CharacterController yere de�iyorsa, k���k bir gravity veriyoruz ki floating durumunda kalmas�n.
        if (characterController.isGrounded)
        {
            //Animation duruyor.
            if (isJumpAnimating) 
            {
                animator.SetBool(isJumpingHash, false);
                isJumpAnimating = false;

                //Jump durduktan bir s�re sonra jumpCount s�f�rlan�yor. B�ylece jump combosu resetlenmi� oluyor.
                jumpResetCoroutine = StartCoroutine(JumpResetTimer());

            } 

            currentMovement.y = appliedMovement.y = groundedGravity;
        } 
        //Player d����te. Else ile kod tekrar� olsa da ilerde buraya d���� an�na �zel kodlar eklenebilir o nedenle bu �ekilde b�rakt�k.
        else if (isFalling)
        {
            //Burada Velocity Verlet entegrasyonu kulland�k. Yar�m ad�ml�k h�z g�ncellemesi yapt�k b�ylece frame rate fark�ndan do�acak sapmalardan kurtulmak ama�lan�yor.
            float previousYVelocity = currentMovement.y;
            currentMovement.y = currentMovement.y + (jumpGravities[jumpCount] * fallMultiplier * Time.deltaTime);

            //NextYVelocity Clamp'liyoruz, y�ksek mesafeden a�a�� d���yorsak fallMultiplier'�m�z bizi a��r� h�zland�rmas�n diye. -20f d��erken ula�aca��m�z max d��me h�z�
            appliedMovement.y = Mathf.Max((previousYVelocity + currentMovement.y) * 0.5f, -20.0f);
        }
        //Di�er durumlarda Gravity uyguluyoruz.
        else
        {
            //Burada Velocity Verlet entegrasyonu kulland�k. Yar�m ad�ml�k h�z g�ncellemesi yapt�k b�ylece frame rate fark�ndan do�acak sapmalardan kurtulmak ama�lan�yor.
            float previousYVelocity = currentMovement.y;
            currentMovement.y = currentMovement.y + (jumpGravities[jumpCount] * Time.deltaTime);
            appliedMovement.y = (previousYVelocity + currentMovement.y) * 0.5f;
        }
    }

    void HandleRotation()
    {
        Vector3 positionToLookAt;
        //Player hareket etti�i y�ne do�ru bakmal�
        positionToLookAt = new Vector3(currentMovement.x, 0f, currentMovement.z);

        Quaternion currentRotation = transform.rotation;

        if (isMovementPressed)//Hareket etmiyorken d�nm�yoruz.
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            //Snappy d�n�� hareketleri olmamas� i�in Lerp kulland�k.
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        
    }

    void HandleAnimation()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);

        //E�er hareket tu�una bas�lm��sa ve karakter y�r�m�yorsa, y�r�me animasyonu �al���r.
        if (isMovementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);
        }
        //E�er hareket tu�una bas�lmam��sa ve karakter y�r�yorsa, y�r�me animasyonu durdurulur.
        else if (!isMovementPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }

        //E�er hem hareket tu�una hem de ko�ma tu�una bas�lm��sa, ve karakter buna ra�men ko�muyorsa, ko�ma animasyonu �al���r.
        if ((isMovementPressed && isRunPressed) && !isRunning)
        {
            animator.SetBool(isRunningHash, true);
        }
        //E�er hareket tu�una veya ko�ma tu�una bas�lmam��sa, ve karakter buna ra�men ko�uyorsa, ko�ma animasyonu durdurulur.
        else if ((!isMovementPressed || !isRunPressed) && isRunning)
        {
            animator.SetBool(isRunningHash, false);
        }
    }

    void HandleMovement()
    {
        if (isRunPressed)
        {
            appliedMovement.x = currentRunMovement.x;
            appliedMovement.z = currentRunMovement.z;
        }
        else
        {
            appliedMovement.x = currentMovement.x;
            appliedMovement.z = currentMovement.z;
        }

        characterController.Move(appliedMovement * Time.deltaTime);
    }

    void Update()
    {
        
        HandleRotation();
        HandleAnimation();
        HandleMovement();


        HandleGravity();//HandleMovement'tan sonra �al��mal�, ��nk� gravity de�eri isGrounded'a g�re de�i�ecek ve Move metodu HandleMovement'�n i�inde. Jump i�in yapt�k bunu.
        HandleJump();//Bunu HandleGravity'den sonra �al��t�r�yoruz ��nk� gravity de�erine ba�l� olarak i�lemlerini yap�yor.
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
