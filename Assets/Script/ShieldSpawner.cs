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

        if (startWithShield)
        {
            ScheduleImmediateSpawn();
        }
    }

    protected override void Update()
    {
        base.Update();
        ProcessSpawnQueue();
    }

    private void ProcessSpawnQueue()
    {
        while (spawnEvents.Count > 0 && Time.time >= spawnEvents.Peek())
        {
            float scheduledTime = spawnEvents.Dequeue();
            Debug.Log($"Processing shield spawn event. " +
                $"Scheduled: {scheduledTime}, Current: {Time.time}");

            isWaitingForSpawn = false;

            if (currentSpawnCount == 0)
            {
                spawnTimer = spawnInterval;
            }
            else
            {
                Debug.Log("Shield already present on field, skipping this spawn event");
                ScheduleNextSpawnEvent(respawnTime * 0.5f);
            }
        }
    }
    protected override void OnObjectDestroyed()
    {
        if (currentSpawnCount == 0)
        {
            Debug.Log($"Shield destroyed. Scheduling respawn " +
                $"in {respawnTime} seconds");
            ScheduleNextSpawnEvent(respawnTime);
        }
    }
    private void ScheduleNextSpawnEvent(float delay)
    {
        if (isWaitingForSpawn) return;

        float spawnTime = Time.time + delay;
        spawnEvents.Enqueue(spawnTime);
        isWaitingForSpawn = true;

        Debug.Log($"Scheduled shield spawn event at: {spawnTime} " +
            $"(delay: {delay} seconds)");
    }

    public void ScheduleImmediateSpawn()
    {
        ScheduleNextSpawnEvent(0.1f);
        Debug.Log("Scheduled immediate shield spawn");
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
