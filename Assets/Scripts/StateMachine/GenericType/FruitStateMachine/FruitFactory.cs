using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitFactory : MonoBehaviour
{
    public bool GetFruit(GameObject prefab, Transform spawnParent, GameObject spawnPoint)
    {
        if (spawnPoint == null) return false;

        try
        {
            spawnPoint.SetActive(true);

            GameObject spawnedFruit = Instantiate(prefab, spawnPoint.transform.position, spawnPoint.transform.rotation, spawnParent);
            if (spawnedFruit != null) return true;
            return false;
        }
        catch 
        {
            return false;
        }
        
    }
}
