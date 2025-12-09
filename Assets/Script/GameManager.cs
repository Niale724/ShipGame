using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class GameManager: MonoBehaviour
{   
    //Singleton -- one instance of a class exists on the entire game
    public static GameManager Instance { get; private set; } // all scripts can access this static variable
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    [Header("Obstacles Prefabs")]

    [SerializeField] private GameObject obstacleType1PF;
    [SerializeField] private GameObject obstacleType2PF;
    [SerializeField]private GameObject obstacleType3PF;


    [Header("Spawn Settings")]

    [SerializeField] private float spawnInterval= 5f; //seconds between spawning

    //spawning "boundaries"
    [SerializeField] private float minX =-8f; //left boundary 
    [SerializeField] private float maxX =8f; // right boundary

    [SerializeField] private float minY=-4f; // bottom boundary 

    [SerializeField] private float maxY=4f; // top boundary 


    
    public void OnShieldCollected(Shield collectedShield)
    {
        Debug.Log("Shield collected: " + collectedShield.name);
        // Awiats further implementation
    }


    [Header("Active Obstacles")]
    private List <Obstacle> activeObstacle = new List <Obstacle>();

    //remember to take out an obstacle from the scene when the submarine and the submarine collide

    [Header("Obstacle Data")]
    private Dictionary<string, int> dataObstacle = new Dictionary<string, int>();

    private void InitializedObjectData()
    {  
        dataObstacle.Clear(); //gets rid of any existing entries

        dataObstacle.Add("Type1",2);
        dataObstacle.Add("Type2", 3);
        dataObstacle.Add("Type3", 5);
    }

    [Header("Game State")]

    private bool isGameRunning = false;
    /*usage of coroutines!!!
    
    what are they? a functions that allows you to pause and resume over time.
    
    useful information:
    IEnumerator is the type that ienumerator returns
    yield return pasuse the coroutine
    new WairForSeconds(spawnInterval) waits the spawninterval time

    why to use them?
    because we would have to track the manually otherwise, also cleaner code

    difference?
    coroutine = function pauses itself for the spawnInterval time, unity handles the time
    time.deltaTime approach = manually set time till spawnInterval. 

    */

    void Start()
    {
        Debug.Log("GamaManager Start() called");
        InitializedObjectData();
        StartGame();

    }   
     public void StartGame()
    {
        isGameRunning = true;
        StartCoroutine(SpawnObstacles());
    }

    public void StopGame()
    {
        isGameRunning =false;
        StopAllCoroutines();
    }

    [Header("Obstacle Waves Settings")]
    
    [SerializeField] private int obstaclesPerWave =5;
    [SerializeField] private int minRemainingObstacles = 3; 

    public IEnumerator SpawnObstacles()
    {
        while (isGameRunning)
        {
            int currentObstacles = activeObstacle.Count;

            if (currentObstacles <= minRemainingObstacles)
            {
                obstaclesPerWave++;

                for(int i=0; i<obstaclesPerWave; i++)
                {
                    SpawnAnObstacle();
                }
            } 
            yield return new WaitForSeconds(spawnInterval);

        }
        
    }
    /* might have to take the randomization method out and make a spawning f(x) for every prefab is the
    randomization is not proportionate
    private void SpawnVariousObstacles()
    {
        
        for(int i = 0; i < 3; i++)
        {
           
        }



    }
    */
    private void SpawnAnObstacle()
    {
        //check spawing boundaries 
        if(minX>=maxX || minY >= maxY)
        {
            Debug.Log("Invalid spawn boundaries, check the inspector");
        }

        //set random position inside the boundaries
        Vector2 randomPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

        // instantiate the obstacle and get component
        GameObject prefabToSpawn = GetRandomObstaclePrefab();

        Obstacle obstacle = Instantiate(prefabToSpawn, randomPosition, Quaternion.identity).GetComponent<Obstacle>();
        //Quaternion.identity = no rotation

        //add obstacle object to the active object list

        if (obstacle != null)
        {
            activeObstacle.Add(obstacle);
        }

       
    }
/*genesis notes: a gameObject container that holds things
vs component parts or pieces that make the gameObject do things*/
 GameObject GetRandomObstaclePrefab()
        {
            int randomPrefab = Random.Range(0,3);

            if(randomPrefab == 0)
            {
                return obstacleType1PF;
            }
            else if (randomPrefab == 1)
            {
                return obstacleType2PF;
            }
            else
            {
                return obstacleType3PF;
            }

        }



    //it removes obstacles from the list
    public void RemoveObstacle(Obstacle obstacle)
    {
        if (activeObstacle.Contains(obstacle))
        {
            activeObstacle.Remove(obstacle);
        }
    }




    //add the last method 

    //when doing the game over screen we need to empty the entire list 

    public void DeleteAllActiveObstacles()
    {
        foreach (Obstacle obstacle in activeObstacle)
        {
            if (obstacle != null)
            {
                
                Destroy(obstacle.gameObject); // destroys the visual objects
            }
        }
        activeObstacle.Clear(); //clears the list 
    }


    //might take out, returns the list of active obstacle
    public List<Obstacle> GetActiveObstacles()
    {
        return activeObstacle;
    }
    

    //retrieve Obstacle HPvalues to use dubug.log

    public int GetObstacleHPValue (string obstacleType)
    {
        if(dataObstacle.ContainsKey(obstacleType))
        {
         return dataObstacle[obstacleType];
        }
        else
        {
            Debug.Log("This obstacle type wasn't found" + obstacleType);
            return 0;
        }
    }

}