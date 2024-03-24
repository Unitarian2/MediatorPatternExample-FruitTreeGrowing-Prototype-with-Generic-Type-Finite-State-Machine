using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActionController : MonoBehaviour
{
    PlayerStateMachine playerStateMachine;
    PlayerInputAction playerInputAction;

    bool isInteractPressed;
    bool isInteracting;
    public GameObject hoveredObject;//Bunu debug için ekledik.HandlePickupSearch içinde local field'a taþýnmasý gerek.
    private IPickupable hoveredItem;
    [SerializeField] private TextMeshProUGUI hoveredItemName;

    public float PickupSphereRadius = 0.5f;
    public float PickupDistance = 1f;
    private Dictionary<IPickupable, bool> aimedObjects = new Dictionary<IPickupable, bool>();

    // Start is called before the first frame update
    void Start()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();
        playerInputAction = playerStateMachine.PlayerInputAction;

        playerInputAction.CharacterControls.Interaction.started += Interact;
        playerInputAction.CharacterControls.Interaction.canceled += Interact;
    }

    private void Interact(InputAction.CallbackContext context)
    {
        isInteractPressed = context.ReadValueAsButton();
    }

    // Update is called once per frame
    void Update()
    {
        HandlePickupSearch();
        HandleInteraction();
    }

    private void HandlePickupSearch()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit[] hits = Physics.SphereCastAll(ray, PickupSphereRadius);
        if(hits.Length == 0)
        {
            return;
        }

        RaycastHit closestHit = hits[0];

        if(hits.Length > 1)
        {
            float closestHitDistance = Mathf.Infinity;
            foreach (RaycastHit hit in hits)
            {
                //Ray'e en yakýn cismi hoverlýyoruz.
                if (closestHitDistance > Vector3.Distance(hit.point, ray.origin))
                {
                    //Cismin IPickupable implement etmiþ olmasý lazým.
                    if(hit.collider.gameObject.TryGetComponent<IPickupable>(out var po))
                    {
                        
                        {
                            closestHit = hit;
                            closestHitDistance = Vector3.Distance(hit.point, ray.origin);
                        }
                    }
                }
                    
            }
        }

        //Pickup edilebilir bir mesafede olmasý lazým
        if (Vector3.Distance(closestHit.point, transform.position) < PickupDistance)
        {
            hoveredObject = closestHit.collider.transform.gameObject;
        }
        else
        {
            hoveredObject = null;
            hoveredItemName.text = "";
            return;
        }
            
        if (hoveredObject.TryGetComponent<IPickupable>(out var pickupableObject))
        {
            hoveredItemName.text = "(E) Pickup " + pickupableObject.GetDisplayName();
            hoveredItem = pickupableObject;
        }
        else
        {
            //IPickupable'a sahip bir cisime niþan almamýþýz. Interaction olmaz.
            hoveredObject = null;
            hoveredItemName.text = "";
        }
    }

    private void HandleInteraction()
    {
        //Hali hazýrda interaction sürecinde deðilsek, interaction butonuna basýlmýþsa ve üzerine hovered olmuþ bir object varsa, interaction baþlar
        if(!isInteracting && isInteractPressed && hoveredObject != null)
        {
            //Interaction baþlar
            isInteracting = true;
            hoveredItem.TriggerPickupAction();
            isInteracting = false;
        }
    }

}
