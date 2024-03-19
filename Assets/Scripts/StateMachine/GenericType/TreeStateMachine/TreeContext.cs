using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeContext : MonoBehaviour
{
    private TreeSettings settings;
    private GameObject[] spawns;
    public TreeContext(TreeSettings settings)
    {
        this.settings = settings;
    }

    public TreeSettings Settings => settings;
}
