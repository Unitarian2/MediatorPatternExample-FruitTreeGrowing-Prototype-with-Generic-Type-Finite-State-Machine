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
            //Bize SphereCast'in çarptýðý IPickupable tipindeki gameobject'ler lazým sadece
            if (hit.collider.gameObject.TryGetComponent<IPickupable>(out var pickupableObject))
            {
                tempAimedObjects[pickupableObject] = true;
                pickupableObject.UpdateOverlay(true);

                // Dictionary'de zaten varmýþ. Sadece value güncelliyoruz.
                if (aimedObjects.ContainsKey(pickupableObject))
                {
                    aimedObjects[pickupableObject] = true;
                }
                else
                {
                    // Dictionary'de yokmuþ. Ekliyoruz.
                    aimedObjects.Add(pickupableObject, true);
                }
            }
        }

        //// aimedObjects sözlüðünde olmayan nesnelerin Overlay kapatýyoruz.
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
            // Eðer tempAimedObjects sözlüðünde bu anahtar yoksa, deðeri false yap
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
            //Interaction baþlar
            isInteracting = true;



        }
    }

    List<IPickupable> GetPickupablesInCircle(Vector3 center, float radius)
    {
        // Çember içindeki game object'leri tutmak için bir liste oluþtur
        List<IPickupable> pickups = new List<IPickupable>();

        // Çember içindeki tüm collider'larý al
        Collider[] colliders = Physics.OverlapSphere(center, radius);

        // Her collider'ý döngüye al
        foreach (var collider in colliders)
        {
            // Collider'a sahip game object'i al
            GameObject obj = collider.gameObject;

            // Eðer IPickupable arayüzüne sahipse, listeye ekle
            IPickupable pickupable = obj.GetComponent<IPickupable>();
            if (pickupable != null)
            {
                pickups.Add(pickupable);
            }
        }

        // Tüm IPickupable game object'leri döndür
        return pickups;
    }

    
}
