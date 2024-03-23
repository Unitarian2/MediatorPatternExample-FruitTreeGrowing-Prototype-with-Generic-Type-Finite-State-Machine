using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActionController : MonoBehaviour
{
    PlayerStateMachine playerStateMachine;
    PlayerInputAction playerInputAction;

    bool isInteractPressed;
    bool isInteracting;

    public float PickupRadius = 3f;
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
        RaycastHit[] hits = Physics.SphereCastAll(ray, 0.5f);

        Dictionary<IPickupable, bool> tempAimedObjects = new Dictionary<IPickupable, bool>();

        foreach (var hit in hits)
        {
            //Bize SphereCast'in �arpt��� IPickupable tipindeki gameobject'ler laz�m sadece
            if (hit.collider.gameObject.TryGetComponent<IPickupable>(out var pickupableObject))
            {
                tempAimedObjects[pickupableObject] = true;
                pickupableObject.UpdateOverlay(true);

                // Dictionary'de zaten varm��. Sadece value g�ncelliyoruz.
                if (aimedObjects.ContainsKey(pickupableObject))
                {
                    aimedObjects[pickupableObject] = true;
                }
                else
                {
                    // Dictionary'de yokmu�. Ekliyoruz.
                    aimedObjects.Add(pickupableObject, true);
                }
            }
        }

        //// aimedObjects s�zl���nde olmayan nesnelerin Overlay kapat�yoruz.
        //foreach (var kvp in aimedObjects)
        //{
        //    if (!tempAimedObjects.ContainsKey(kvp.Key))
        //    {
        //        aimedObjects[kvp.Key] = false;
        //        kvp.Key.UpdateOverlay(false);
        //    }
        //}
        foreach (var kvp in aimedObjects.Keys.ToList())
        {
            // E�er tempAimedObjects s�zl���nde bu anahtar yoksa, de�eri false yap
            if (!tempAimedObjects.ContainsKey(kvp))
            {
                aimedObjects[kvp] = false;
                kvp.UpdateOverlay(false);
            }
        }

    }

    private void HandleInteraction()
    {
        if(!isInteracting && isInteractPressed)
        {
            //Interaction ba�lar
            isInteracting = true;



        }
    }

    List<IPickupable> GetPickupablesInCircle(Vector3 center, float radius)
    {
        // �ember i�indeki game object'leri tutmak i�in bir liste olu�tur
        List<IPickupable> pickups = new List<IPickupable>();

        // �ember i�indeki t�m collider'lar� al
        Collider[] colliders = Physics.OverlapSphere(center, radius);

        // Her collider'� d�ng�ye al
        foreach (var collider in colliders)
        {
            // Collider'a sahip game object'i al
            GameObject obj = collider.gameObject;

            // E�er IPickupable aray�z�ne sahipse, listeye ekle
            IPickupable pickupable = obj.GetComponent<IPickupable>();
            if (pickupable != null)
            {
                pickups.Add(pickupable);
            }
        }

        // T�m IPickupable game object'leri d�nd�r
        return pickups;
    }

    
}
