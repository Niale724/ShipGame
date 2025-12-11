using UnityEngine;

public class FSpawner : BaseSpawner
{
    [SerializeField] private WeightedPrefab[] fishPrefabs;

    protected override void SpawnObject()
    {
        if (fishPrefabs.Length == 0) return;

        Vector3 randomOffset = new Vector3(
        Random.Range(-spawnAreaOffset.x, spawnAreaOffset.x),
        Random.Range(-spawnAreaOffset.y, spawnAreaOffset.y),
        0f);

        GameObject fishToSpawn = GetWeightedRandomFish();

        Instantiate(fishToSpawn, transform.position + randomOffset, Quaternion.identity);

        spawnTimer = 0f;
        Debug.Log("Spawned Fish (Weighted): " + fishToSpawn.name);

    }

    private GameObject GetWeightedRandomFish()
    {
        int totalWeight = 0;

        foreach (var w in fishPrefabs)
            totalWeight += w.weight;

        int randomValue = Random.Range(0, totalWeight);

        foreach (var w in fishPrefabs)
        {
            if (randomValue < w.weight)
                return w.prefab;

            randomValue -= w.weight;
        }

        return fishPrefabs[0].prefab;
    }

    private void Start()
    {
        prefabToSpawn = new GameObject[fishPrefabs.Length];
        for (int i = 0; i < fishPrefabs.Length; i++)
        {
            prefabToSpawn[i] = fishPrefabs[i].prefab;
        }
    }
}


