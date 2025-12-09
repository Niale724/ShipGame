using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
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

    [Header("Game State")]

    private bool isGameRunning = false;

     private void InitializedObjectData()
    {  
        dataObstacle.Clear(); //gets rid of any existing entries

        dataObstacle.Add("Type1",2);
        dataObstacle.Add("Type2", 3);
        dataObstacle.Add("Type3", 5);
    }

    void Start()
    {
        Debug.Log("GamaManager Start() called");
        InitializedObjectData();
        StartGame();

    }   
     public void StartGame()
    {
        isGameRunning = true;
        Debug.Log("Game Started");
    }

    public void StopGame()
    {
        isGameRunning =false;
        Debug.Log("Game Stopped");
    }


    //it removes obstacles from the list
    public void RemoveObstacle(Obstacle obstacle)
    {
        if (activeObstacle.Contains(obstacle))
        {
            activeObstacle.Remove(obstacle);
        }
    }



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


    public void RegisterObstacle(Obstacle obstacle)
    {
        
        if (obstacle!=null && !activeObstacle.Contains(obstacle))
        {
            activeObstacle.Add(obstacle);
            Debug.Log($"Obstacle registered with GamaManager: {obstacle.name}");
        }
    }
}