using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitContext
{
    private FruitSettings settings;
    private Transform transform;
    private Material[] materials;
    private Rigidbody rb;
    public FruitContext(FruitSettings settings, Transform transform, Material[] materials, Rigidbody rb)
    {
        this.settings = settings;
        this.transform = transform;
        this.materials = materials;
        this.rb = rb;
    }

    public FruitSettings Settings => settings;
    public Transform Transform => transform;

    public Rigidbody Rigidbody { get { return rb; } set { rb = value; } }

    public void ChangeColor(Color color)
    {
        foreach (var mat in materials)
        {
            mat.color = color;
        }
    }
}
