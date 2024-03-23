using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickupable
{
    public bool IsOverlayActive { get; set; }
    public Material OverlayMaterial { get; set; }
    public void UpdateOverlay(bool value);


}
