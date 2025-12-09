using UnityEngine;

public class OSpawner : BaseSpawner
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void InitializeSpawner()
    {
        base.InitializeSpawner(); // we're calling the base class version
        Debug.Log("Obstacle Spawner initialized"); // might erase later
    }

    // Update is called once per frame
    protected override void RegisterSpawnedObject(GameObject spawnedObject)
    {
        base.RegisterSpawnedObject(spawnedObject);

        Obstacle obstacle = spawnedObject.GetComponent<Obstacle>();

        if (obstacle != null && GameManager.Instance != null)
        {
            GameManager.Instance.RegisterObstacle(obstacle);
        }
    }   
    
        
    
}
