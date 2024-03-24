using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="FruitSettings" , menuName ="FruitSettings/Fruit")]
public class FruitSettings : ScriptableObject
{
    [Header("General")]
    public string FruitDisplayName;

    [Header("Lifecycle")]
    public float GrowTime = 10f;
    public float RipenTime = 5f;
    public float DecayTime = 20f;

    [Range(0.25f, 2f)] public float startSize = 0.5f;
    [Range(0.25f, 2f)] public float endSize = 1f;

    public Color StartColor;
    public Color GrewColor;
    public Color RipenedColor;
    public Color DecayColor;
}
