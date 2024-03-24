using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour, IPickupable
{

    [SerializeField] private FruitSettings _settings;
    FruitStateMachine _stateMachine;

    private void Start()
    {
        _stateMachine = GetComponent<FruitStateMachine>();
    }
    public string GetDisplayName()
    {
        return _settings.FruitDisplayName;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void TriggerPickupAction()
    {
        _stateMachine.DeactivateFruit();
    }
}
