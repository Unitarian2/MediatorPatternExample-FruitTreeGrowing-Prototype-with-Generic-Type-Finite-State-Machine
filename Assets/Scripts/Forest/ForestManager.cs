using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtils;

public class ForestManager : MonoBehaviour
{
    [SerializeField] private Transform treeParent;
    private TreeStateMachine[] trees;

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
        
    }
}
