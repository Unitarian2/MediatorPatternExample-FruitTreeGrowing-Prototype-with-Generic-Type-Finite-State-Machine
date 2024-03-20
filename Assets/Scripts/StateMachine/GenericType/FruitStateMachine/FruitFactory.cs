using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitFactory : MonoBehaviour
{
    public GameObject GetFruit(GameObject prefab, Transform spawnParent, GameObject spawnPoint)
    {
        if (spawnPoint == null) return null;

        try
        {
            spawnPoint.SetActive(true);

            GameObject spawnedFruit = Instantiate(prefab, spawnPoint.transform.position, spawnPoint.transform.rotation, spawnParent);
            if (spawnedFruit != null) return spawnedFruit;
            return null;
        }
        catch 
        {
            return null;
        }
        
    }

    public void DestroyFruit(GameObject gameObject)
    {
        Destroy(gameObject);
    }
}
