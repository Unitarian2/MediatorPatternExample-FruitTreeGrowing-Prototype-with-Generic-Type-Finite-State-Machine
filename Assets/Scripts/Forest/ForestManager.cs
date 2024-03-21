using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtils;

public class ForestManager : MonoBehaviour
{
    [SerializeField] private Transform treeParent;
    private TreeStateMachine[] trees;

    [SerializeField] private float averageSpawnTime;
    [SerializeField] private float totalSpawnTime;

    private void Awake()
    {
        trees = treeParent.GetAllChildren<TreeStateMachine>();
    }

    void Start()
    {
        if(trees.Length > 0)
        {
            foreach(TreeStateMachine tree in trees)
            {
                tree.ActivateTree();
            }
        }
    }

    
    void Update()
    {
        if (trees.Length > 0)
        {
            float spawnTime = 0;
            foreach (TreeStateMachine tree in trees)
            {
                spawnTime += tree.Context.CurrentSpawnInfo.spawnTime;
            }

            averageSpawnTime = spawnTime / trees.Length;
            totalSpawnTime = spawnTime;
            spawnTime = 0;

        }
    }
}
