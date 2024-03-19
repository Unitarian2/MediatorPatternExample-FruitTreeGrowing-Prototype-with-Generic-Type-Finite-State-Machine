using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeContext
{
    private TreeSettings _settings;
    

    //Fruit Spawns
    private GameObject[] _spawns;
    private Transform _spawnParent;
    private FruitFactory _factory;
    private GameObject _assignedFruit;
    public TreeContext(TreeSettings settings, GameObject[] spawnPoints, Transform spawnParent, FruitFactory factory)
    {
        this._settings = settings;
        _spawns = spawnPoints;
        this._spawnParent = spawnParent;
        _factory = factory;
    }

    public TreeSettings Settings => _settings;

    public void Init()
    {
        foreach (var spawn in _spawns)
        {
            spawn.SetActive(false);
        }
    }

    public void SpawnFruit()
    {
        var spawnPoint = GetSpawnPoint();
        GameObject spawnedFruit = null;

        if (_assignedFruit == null)
        {
            //Her aðacýn bir prefab'i olabilir. Ancak farklý prefab türlerimiz olabilir. Elma aðacýndaki elma türleri gibi düþünülebilir.
            int index = Random.Range(0, _settings.fruitPrefabs.Length);
            GameObject chosenFruit = _settings.fruitPrefabs[index];

            spawnedFruit = _factory.GetFruit(chosenFruit, _spawnParent, spawnPoint);         
        }
        else
        {
            spawnedFruit = _factory.GetFruit(_assignedFruit, _spawnParent, spawnPoint);
        }


        if (spawnedFruit != null)
        {
            //Baþarýlý spawn.
            spawnedFruit.GetComponent<FruitStateMachine>().ActivateFruit(spawnPoint, this);
        }
    }

    public void DeactivateSpawnPoint(GameObject spawnPoint)
    {
        foreach(var spawn in _spawns)
        {
            if(spawn == spawnPoint)
            {
                spawn.SetActive(false);
            }
        }
    }

    public GameObject GetSpawnPoint()
    {
        foreach (GameObject s in _spawns)
        {
            if (!s.activeSelf)
            {
                return s;
            }
        }
        return null;
    }
}
