using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public abstract class BaseSpawner : MonoBehaviour
{
    [SerializeField] protected GameObject[] prefabToSpawn;
    [SerializeField] protected float spawnInterval = 2.0f;
    [SerializeField] protected int maxSpawnCount = 10;
    [SerializeField] protected Transform spawnContainer;
    [SerializeField] protected bool spawnAtStart = true;
    [SerializeField] protected Vector2 spawnAreaOffset=Vector2.zero;
    protected int currentSpawnCount = 0;
    protected float spawnTimer = 0.0f;
    protected bool isActive = true;

    private List<GameObject> spawnedObjects = new List<GameObject>();
    private float cleanupTimer = 0.0f;
    private float cleanupInterval = 2.0f;
    protected void Start()
    {
        InitializeSpawner();
    }

    protected virtual void InitializeSpawner()
    {
        if (prefabToSpawn == null || prefabToSpawn.Length == 0)
        {
            Debug.LogWarning("Prefab to spawn is null or empty.");
            enabled = false;
            return;
        }
        spawnTimer= spawnAtStart ? spawnInterval : 0.0f;
        Debug.Log("Spawner initialized.");
    }

    protected virtual void Update()
    {
        if (!isActive) return;
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval && CanSpawn())
        {
            SpawnObject();
            spawnTimer = 0.0f;
        }

        CleanupDestroyedObjects();
    }

    protected virtual bool CanSpawn()
    {
        return currentSpawnCount < maxSpawnCount;
    }

    protected virtual void SpawnObject()
    {
        if(prefabToSpawn.Length == 0) return;
        GameObject prefab = SelectedPrefabToSpawn();
        if (prefab == null) return;

        Vector2 spawnPosition = GetSpawnPosition();
        GameObject spawnedObject = Instantiate(prefab, spawnPosition, GetSpawnRotation());
        
        if(spawnContainer !=null)
        {
            spawnedObject.transform.SetParent(spawnContainer);
        }
        
        RegisterSpawnedObject(spawnedObject);
        //Debug.Log("Spawned object: " + spawnedObject.name + " at " + spawnPosition);
    }

    protected virtual GameObject SelectedPrefabToSpawn()
    {
        return prefabToSpawn[Random.Range(0,prefabToSpawn.Length)];
    }
    protected Vector2 GetSpawnPosition()
    {
        float randomX = Random.Range(
            ScreenUtils.ScreenLeft + spawnAreaOffset.x,
            ScreenUtils.ScreenRight - spawnAreaOffset.x);
        float randomY = Random.Range(
            ScreenUtils.ScreenBottom + spawnAreaOffset.y,
            ScreenUtils.ScreenTop - spawnAreaOffset.y);

        return new Vector2(randomX, randomY);
    }

    protected virtual Quaternion GetSpawnRotation()
    {
        return Quaternion.Euler(0, 0, 0);
    }

    protected virtual void RegisterSpawnedObject(GameObject spawnedObject)
    {
        spawnedObjects.Add(spawnedObject);
        currentSpawnCount++;
    }
    protected virtual void CleanupDestroyedObjects()
    {
        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            if (spawnedObjects[i] == null)
            {
                spawnedObjects.RemoveAt(i);
                currentSpawnCount= Mathf.Max(0, currentSpawnCount - 1);
                OnObjectDestroyed();
            }
        }
    }
    protected virtual void OnObjectDestroyed()
    {
        // Base implementation
    }

    public void StartSpawning() => isActive = true;
    public void StopSpawning() => isActive = false;

    public void SetSpawnInterval(float interval)
    {
        spawnInterval = Mathf.Max(0.1f, interval);
    }
    public void SetMaxSpawnCount(int count)
    {
        maxSpawnCount = Mathf.Max(1, count);
    }
    public int GetCurrentSpawnCount() => currentSpawnCount;
    public bool IsSpawning() => isActive;

    public IReadOnlyList<GameObject> GetSpawnedObjects() => spawnedObjects;
    public void SpawnImmediate()
    {
        if (CanSpawn())
        {
            SpawnObject();
        }
    }
    public void ClearAllSpawned()
    {
        foreach(var obj in spawnedObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        spawnedObjects.Clear();
        currentSpawnCount = 0;
    }
    public bool DidSpawnObjects(GameObject obj)
    {
        return spawnedObjects.Contains(obj);
    }
}
