using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitContext
{
    private FruitSettings settings;
    private Transform transform;
    private Material[] materials;
    private Rigidbody rb;
    private GameObject gameObject;
    private bool isInventoryItem;
    private float destroyTime = 20f;
    private FruitStateMachine stateMachine;

    //Tree Info
    private TreeContext assignedTree;
    private GameObject spawnPoint;
    public FruitContext(FruitSettings settings, Transform transform, Material[] materials, Rigidbody rb, GameObject gameObject, FruitStateMachine stateMachine)
    {
        this.settings = settings;
        this.transform = transform;
        this.materials = materials;
        this.rb = rb;
        this.gameObject = gameObject;
        this.stateMachine = stateMachine;
    }

    public FruitSettings Settings => settings;
    public Transform Transform => transform;
    public GameObject GameObject => gameObject;
    public Rigidbody Rigidbody { get { return rb; } set { rb = value; } }
    public bool IsInventoryItem { get { return isInventoryItem; } set { isInventoryItem = value; } }

    public TreeContext AssignedTree { get { return assignedTree; } }
    public GameObject SpawnPoint { get { return spawnPoint; } }
    public float DestroyTime => destroyTime;
    public FruitStateMachine StateMachine => stateMachine;  

    public void ChangeColor(Color color)
    {
        foreach (var mat in materials)
        {
            mat.color = color;
        }
    }

    public void SetTreeInfo(TreeContext assignedTree, GameObject spawnPoint)
    {
        this.assignedTree = assignedTree;
        this.spawnPoint = spawnPoint;
    }

}
