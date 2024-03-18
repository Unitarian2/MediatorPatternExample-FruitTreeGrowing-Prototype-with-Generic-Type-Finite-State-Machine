using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitContext
{
    private FruitSettings settings;
    private Transform transform;
    private Material[] materials;
    public FruitContext(FruitSettings settings, Transform transform, Material[] materials)
    {
        this.settings = settings;
        this.transform = transform;
        this.materials = materials;
    }

    public FruitSettings Settings => settings;
    public Transform Transform => transform;

    public void ChangeColor(Color color)
    {
        foreach (var mat in materials)
        {
            mat.color = color;
        }
    }
}
