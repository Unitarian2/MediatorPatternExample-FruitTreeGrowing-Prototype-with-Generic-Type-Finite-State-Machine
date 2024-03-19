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
        if(_assignedFruit == null)
        {
            //Her a�ac�n bir prefab'i olabilir. Ancak farkl� prefab t�rlerimiz olabilir. Elma a�ac�ndaki elma t�rleri gibi d���n�lebilir.
            int index = Random.Range(0, _settings.fruitPrefabs.Length);
            GameObject chosenFruit = _settings.fruitPrefabs[index];

            if (_factory.GetFruit(chosenFruit, _spawnParent, GetSpawnPoint()))
            {
                //Ba�ar�l� spawn.
            }
            
        }
        else
        {
            if (_factory.GetFruit(_assignedFruit, _spawnParent, GetSpawnPoint()))
            {
                //Ba�ar�l� spawn.
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
