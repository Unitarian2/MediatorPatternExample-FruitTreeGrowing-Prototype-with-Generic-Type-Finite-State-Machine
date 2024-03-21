using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="TreeMediator" , menuName ="Tree/TreeMediator")]
public class TreeMediator : ScriptableObject
{
    public event Action<TreeContext> Spawned;
    public event Action<TreeContext> StartedRipening;
    public event Action<TreeContext> Decayed;

    public void OnSpawned(TreeContext treeContext) => Spawned?.Invoke(treeContext);
    public void OnStartedRipening(TreeContext treeContext) => StartedRipening?.Invoke(treeContext);
    public void OnDecayed(TreeContext treeContext) => Decayed?.Invoke(treeContext);
}
