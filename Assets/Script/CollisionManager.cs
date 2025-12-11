using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class CollisionManager : MonoBehaviour

{   
  
   
    [SerializeField] private Submarine submarine;

    /*monobehavior class that can be attached to a gameobject*/

    void Start()
    {
        Debug.Log("Collision Manager started working");
    }
    // this function determines what type of object hits the submarine and take it to the appropiate handler

    //we do this by the usage of tags 
    public void SubmarineCollision(Collider2D otherCollider)
    {
        if (otherCollider == null)
        {
            Debug.Log("CollisionManager received a null collider");
            return;
        }

        if (otherCollider.CompareTag("Obstacle"))
        {
            ObstacleCollision(otherCollider);
        }
        else if (otherCollider.CompareTag("Fish"))
        {
            FishCollision(otherCollider);
        }
        else if (otherCollider.CompareTag("Shield"))
        {
            ShieldCollision(otherCollider);
        }
        else

        {
           // Debug.Log("Submarine was hit by " + otherCollider.gameObject.name);
            Obstacle obstacle = otherCollider.GetComponent<Obstacle>();
            if (obstacle != null)
            {   Debug.Log("Found obstacle coomponent on untagged object");
                ObstacleCollision(otherCollider);
            }
        }
        

    }

//in this method we track fish HP for our victory method
//connect to hp healing system and gets pointvalue from basefish
    private void FishCollision(Collider2D fishCollider)
    {
        BaseFish fish = fishCollider.GetComponent<BaseFish>();

        if (fish != null)
        {
            //tell GameManager to add HP
            GameManager.Instance.FishCollected(fish);
            
            //apply hp healing
            fish.ApplyCollectionEffects(submarine);

            //remove fish
            Destroy(fishCollider.gameObject);
        }
    }
     private void ShieldCollision(Collider2D shieldCollider)
    {
        Debug.Log("Shield Collected " + shieldCollider.gameObject.name);

        GameObject shieldObj = shieldCollider.gameObject;
        Debug.Log($"Shield Collected: {shieldObj.name}");

        var collectible = shieldObj.GetComponent<Collectible>();
        if (collectible != null)
        {
            collectible.MarkAsCollected();
        }

        if (submarine != null)
        {
            submarine.CollectShield();
            Debug.Log("Shield Collected " + shieldCollider.gameObject.name);
        }

        Destroy(shieldObj);
    }

    private void ObstacleCollision(Collider2D obstacleCollider)
    {


        //get obstacle component to identify obstacle type and damage value
        Obstacle obstacle = obstacleCollider.GetComponent<Obstacle>();

        if (obstacle != null)
        {
            //get damage value from obstacle
            int damage = obstacle.GetDamage();
            
           Debug.Log("HP deduction from this obstacle was " + damage + "HP");

            //checks shields and THEN the HP, this function is on submarine 
           submarine.ConsumeShield(); 

            //apply damage through gameManager 
            GameManager.Instance.TakeDamage(damage);

            //remove obstacle from gameManager tracking list
            GameManager.Instance.RemoveObstacle(obstacle);

            //removes the obstacle from the game screen
            Destroy(obstacleCollider.gameObject);
            
        }
        else
        {
            
            Debug.Log("No obstacle component " + obstacleCollider.gameObject.name);
        

        }
        

    }


}
