using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FruitSpawnInfo
{
    public GameObject[] fruitPrefabs;
    [Range(5f, 40f)] public float spawnTime;
}
