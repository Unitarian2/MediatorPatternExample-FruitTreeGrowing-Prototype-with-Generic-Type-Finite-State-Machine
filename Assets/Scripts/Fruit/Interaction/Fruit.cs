using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour, IPickupable
{
    

    public bool IsOverlayActive { get; set; }
    public Material OverlayMaterial { get; set; }

    MeshRenderer meshRenderer;

    public void ActivateOverlay()
    {
        if (meshRenderer == null) return;

        //Material listesinin sonuna overlaymaterial ekledik
        Material[] materials = meshRenderer.materials;
        Array.Resize(ref materials, materials.Length + 1);
        materials[materials.Length - 1] = OverlayMaterial;
        meshRenderer.materials = materials;
    }

    public void DeactivateOverlay()
    {
        if (meshRenderer == null || meshRenderer.materials.Length == 0) return;

        //Overlay material son eleman olmalý, o nedenle son elemaný kaldýrýrsak overlay'de kalkmalý.
        Material[] materials = meshRenderer.materials;
        Array.Resize(ref materials, materials.Length - 1);
        meshRenderer.materials = materials;
    }

    public void UpdateOverlay(bool value)
    {
        if (IsOverlayActive == value) return;

        IsOverlayActive = value;
        if (value)
        {
            ActivateOverlay();
        }
        else
        {
            DeactivateOverlay();
        }


    }

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        IsOverlayActive = false;
        OverlayMaterial = Resources.Load<Material>("Material/Outline/OutlineMaterial");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
