using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TreeSettings", menuName = "TreeSettings/Tree")]
public class TreeSettings : ScriptableObject
{
    public GameObject[] fruitPrefabs;
    [Range(5f, 40f)] public float spawnRate;
    
}
