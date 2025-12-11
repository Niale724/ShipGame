using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using System;
public class GameManager: MonoBehaviour
{
    //Singleton -- one instance of a class exists on the entire game
    public static GameManager Instance { get; private set; } // all scripts can access this static variable
   
//game state

   public int totalFishScoredCollected { get; private set; } //tracks total of fish points collected
    public static Action<int> OnFishCollected { get; internal set; }

    private const int winScore =100; // winning condition

//references

   private HpSystem hpSystem;
   
    [SerializeField] private Submarine submarine;
   
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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

//obstacle management

    [Header("Active Obstacles")]
    private List <Obstacle> activeObstacle = new List <Obstacle>();

    //remember to take out an obstacle from the scene when the submarine and the submarine collide

    [Header("Obstacle Data")]
    private Dictionary<string, int> dataObstacle = new Dictionary<string, int>();

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

         if (submarine !=null)
        {
            //gets hp system component
            hpSystem=submarine.GetComponent<HpSystem>();

            if (hpSystem != null)
            {
                hpSystem.OnDeath +=TriggerGameOver; //GameManager executes the HpSystem death event to 
                //trigger game over
            }

        InitializedObjectData();
        StartGame();

    }   }


//game state control

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

    

//obstacle data dictionary usage
    

    //retrieve Obstacle HPvalues to use debug.log

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

//obstacle list management
    public void RegisterObstacle(Obstacle obstacle)
    {
        
        if (obstacle!=null && !activeObstacle.Contains(obstacle))
        {
            activeObstacle.Add(obstacle);
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
    

//damage system
public void TakeDamage(int damage)
    {
        //checks if there're shields 
        if (submarine.ShieldStacks > 0)
        {
            submarine.ConsumeShield();
            Debug.Log($"Shield absorbed{damage} damage!");
            return; // if there is, no HP is taken
        }
        //if not, the HP is applied
        if (hpSystem != null)
        {
            hpSystem.DecreaseHP(damage);
        }
        
    }


//score tracking
public void FishCollected (BaseFish fish)
    {
        totalFishScoredCollected++;

        GameHUD hud = FindObjectOfType<GameHUD>();
        hud?.SetFish(totalFishScoredCollected);

        IsWin();
    }


private void IsWin()
    {
        
        if (totalFishScoredCollected>= winScore)
        {
         TriggerVictory();   
        }
    }


//game over handling

    //these two methods handle victory or defeat
private void TriggerGameOver()
    {
        StopGame();

        //saves data for the gameoverscene
        PlayerPrefs.SetInt("FinalScore", totalFishScoredCollected);
        PlayerPrefs.SetInt("FinalHP", hpSystem !=null ? hpSystem.GetCurrentHp() :0);
        PlayerPrefs.SetString("GameResult", "Defeat");
        PlayerPrefs.Save();

        //load the gameoverscene
        SceneManager.LoadScene("GameOverScene");
    }
private void TriggerVictory()
    {   StopGame();

        //saves data for the gameoverscene
        PlayerPrefs.SetInt("FinalScore", totalFishScoredCollected);
        PlayerPrefs.SetInt("FinalHP", hpSystem !=null ? hpSystem.GetCurrentHp() :0);
        PlayerPrefs.SetString("GameResult", "Victory");
        PlayerPrefs.Save();

        //load the gameoverscene
        SceneManager.LoadScene("GameOverScene");
    }
    }
