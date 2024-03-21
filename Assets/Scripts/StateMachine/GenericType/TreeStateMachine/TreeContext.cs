using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class TreeContext
{
    private TreeSettings _settings;
    private FruitSpawnInfo _currentSpawnInfo;
    private TreeMediator _mediator;
    

    //Fruit Spawns
    private GameObject[] _spawns;
    private Transform _spawnParent;
    private FruitFactory _factory;
    private GameObject _assignedFruit;
    public TreeContext(TreeSettings settings, GameObject[] spawnPoints, Transform spawnParent, FruitFactory factory, TreeMediator mediator)
    {
        _settings = settings;
        _spawns = spawnPoints;
        _spawnParent = spawnParent;
        _factory = factory;
        _currentSpawnInfo = new FruitSpawnInfo{ fruitPrefabs = settings.fruitSpawnInfo.fruitPrefabs , spawnTime = settings.fruitSpawnInfo.spawnTime};
        _mediator = mediator;
    }

    public FruitSpawnInfo CurrentSpawnInfo => _currentSpawnInfo;
    public GameObject AssignedFruit => _assignedFruit;
    public TreeMediator Mediator => _mediator;

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
            int index = Random.Range(0, _currentSpawnInfo.fruitPrefabs.Length);
            GameObject chosenFruit = _currentSpawnInfo.fruitPrefabs[index];

            _assignedFruit = chosenFruit;
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
        else
        {
            _assignedFruit = null;
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

    public void OnEnable()
    {
        _mediator.Spawned += FruitSpawned;
        _mediator.StartedRipening += FruitStartedRipening;
        _mediator.Decayed += FruitDecayed;

    }

    public void OnDisable()
    {
        _mediator.Spawned -= FruitSpawned;
        _mediator.StartedRipening -= FruitStartedRipening;
        _mediator.Decayed -= FruitDecayed;
    }

    private void FruitSpawned(TreeContext treeContext)
    {
        //_assignedFruit null ise henüz bu aðaçta herhangi bir meyve spawn olmamýþtýr. Bu deðer ilk spawn edilen fruit sonrasý set ediliyor.
        if (_assignedFruit != null)
        {
            if (treeContext.AssignedFruit.GetInstanceID() == _assignedFruit.GetInstanceID())
            {
                //Bu aðacýn meyvesi türünde bir meyve spawn olmuþ.
                Debug.LogWarning("Kendi türünde meyve Spawn oldu " + _assignedFruit.name);
                //Spawn olan her meyve, kendi türünde meyve spawn eden aðaçlarýn spawn time'ýný 1sn uzatýr.
                _currentSpawnInfo.spawnTime = Mathf.Clamp(_currentSpawnInfo.spawnTime + 0.1f, 5f, 40f);
            }
        }
    }

    private void FruitStartedRipening(TreeContext treeContext)
    {
        if (_assignedFruit != null)
        {
            if (treeContext.AssignedFruit.GetInstanceID() == _assignedFruit.GetInstanceID())
            {
                //Bu aðacýn meyvesi türünde bir meyve olgunlaþmaya baþlamýþ.
                Debug.LogWarning("Kendi türünde meyve Ripening oldu " + _assignedFruit.name);
                //Spawn olan her meyve, kendi türünde meyve spawn eden aðaçlarýn spawn time'ýný 1sn uzatýr.
                _currentSpawnInfo.spawnTime = Mathf.Clamp(_currentSpawnInfo.spawnTime + 0.9f, 5f, 40f);
            }
        }      
    }

    private void FruitDecayed(TreeContext treeContext)
    {
        if (_assignedFruit != null)
        {
            if (treeContext.AssignedFruit.GetInstanceID() == _assignedFruit.GetInstanceID())
            {
                //Bu aðacýn meyvesi türünde bir meyve çürümüþ.
                Debug.LogWarning("Kendi türünde meyve Decayed oldu " + _assignedFruit.name);
                //Spawn olan her meyve, kendi türünde meyve spawn eden aðaçlarýn spawn time'ýný 2sn kýsaltýr.
                _currentSpawnInfo.spawnTime = Mathf.Clamp(_currentSpawnInfo.spawnTime - 1f, 5f, 40f);
            }
        } 
    }
}
