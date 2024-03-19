using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtils;

public class ForestManager : MonoBehaviour
{
    [SerializeField] private Transform treeParent;
    private GameObject[] trees;

    private void Awake()
    {
        trees = treeParent.GetAllChildrenAsGameObject();
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
