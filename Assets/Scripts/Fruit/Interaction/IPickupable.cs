using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickupable
{
    public string GetDisplayName();
    public GameObject GetGameObject();
    public void TriggerPickupAction();

}
