using UnityEngine;
using System.Collections.Generic;
public class ShieldSpawner : BaseSpawner
{
    [SerializeField] private float respawnTime = 30f;
    [SerializeField] private bool startWithShield = true;
    private Queue<float> spawnEvents = new Queue<float>();
    private bool isWaitingForSpawn = false;
    protected override void InitializeSpawner()
    {
        base.InitializeSpawner();
        maxSpawnCount = 1;

        if(startWithShield && CanSpawn())
        {
            SpawnImmediate();
            isWaitingForSpawn = false;
        }
        Debug.Log("ShieldSpawner initialized.");
    }

    protected override void Update()
    {
        if (!isActive) return;
        spawnTimer += Time.deltaTime;
        Debug.Log($"[BaseSpawner] Timer: {spawnTimer:F2}, Interval: {spawnInterval}, " +
              $"CurrentCount: {currentSpawnCount}, CanSpawn: {CanSpawn()}");
        if (spawnTimer >= spawnInterval && CanSpawn())
        {
            Debug.Log("[BaseSpawner] Auto-spawn triggered!");
            SpawnObject();
            spawnTimer = 0.0f;
        }
        CleanupDestroyedObjects();
        ProcessSpawnQueue();
    }

    private void ProcessSpawnQueue()
    {
        while (spawnEvents.Count > 0 && Time.time >= spawnEvents.Peek())
        {
            float scheduledTime = spawnEvents.Dequeue();
            isWaitingForSpawn = false;

            Debug.Log($"Processing shield spawn event. " +
                $"Scheduled: {scheduledTime}, Current: {Time.time}");

            if (currentSpawnCount == 0)
            {
                SpawnObject();
                Debug.Log("Shield respawned successfully.");
            }
            else
            {
                Debug.Log("Shield already present on field, skipping this spawn event");
            }
        }
    }
    protected override void OnObjectDestroyed()
    {
        if (currentSpawnCount == 0 && !isWaitingForSpawn)
        {
            float nextSpawnTime = Time.time + respawnTime;
            spawnEvents.Enqueue(nextSpawnTime);
            isWaitingForSpawn = true;

            Debug.Log($"Shield destroyed. Scheduling respawn " +
                $"in {respawnTime} seconds"); 
        }
    }
    protected override bool CanSpawn()
    {
        return base.CanSpawn() && currentSpawnCount < maxSpawnCount;
    }

    public float GetNextSpawnTime()
    {
        if (spawnEvents.Count > 0)
        {
            return spawnEvents.Peek();
        }
        return -1f;
    }

    public int GetQueueCount()
    {
        return spawnEvents.Count;
    }

    public float GetTimeUntilNextSpawn()
    {
        if (spawnEvents.Count > 0)
        {
            float nextTime = spawnEvents.Peek();
            float remaining = nextTime - Time.time;
            return Mathf.Max(0f, remaining);
        }
        return 0f;
    }

    public void ClearAllSpawnEvents()
    {
        spawnEvents.Clear();
        isWaitingForSpawn = false;
        Debug.Log("Cleared all shield spawn events");
    }
}
