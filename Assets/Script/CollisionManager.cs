using Unity.VisualScripting;
using UnityEngine;

public class CollisionManager : MonoBehaviour

{   
    //reference to the other system
    //we need references because we are gonna use these other system's methods when the collisions happen 
    //[SerializeField] private GameManager gameManager;
    //[SerializeField] private HPSystem hpSystem;

    //reference to submarine class

   
    [SerializeField] private Submarine submarine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    /*monobehavior class that can be attached to a gameobject*/

    void Start()
    {
        Debug.Log("Collision Manager started working");
    }
    //rigidbody2d unity this object participated IN PHYSICS.
    //collider2d defines the collision SHAPE.

    //use ontrigger because you DETECT when things touch
    //cause submarine is BOUNCING OFF obstacles

   /* void OnTriggerEnter2D(Collider2D another)
    {
        // This method is automatically called by unity when the submarine's trigger collider
        //touches another trigger collider

        CollisionManager collisionManager = FindAnyObjectByType<CollisionManager>();

        if (collisionManager != null)
        {
            collisionManager.h

        }

        Debug.Log("Collision detected with: " + another.gameObject.name);



    }
    */

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
            Debug.Log("Submarine was hit by " + otherCollider.gameObject.name);
            Obstacle obstacle = otherCollider.GetComponent<Obstacle>();
            if (obstacle != null)
            {   Debug.Log("Found obstacle coomponent on untagged object");
                ObstacleCollision(otherCollider);
            }
        }
        

    }

    private void FishCollision(Collider2D fishCollider)
    {
        Debug.Log("Fish Collected " + fishCollider.gameObject.name);

        Destroy(fishCollider.gameObject);
    }
     private void ShieldCollision(Collider2D shieldCollider)
    {
        Debug.Log("Shield Collected " + shieldCollider.gameObject.name);

        //add shield to our stack
        submarine.CollectShield();
        
        //remove shield from the game screen
        Destroy(shieldCollider.gameObject);
    }

    private void ObstacleCollision(Collider2D obstacleCollider)
    {
        Debug.Log("Obstacle Hit " + obstacleCollider.gameObject.name);

        //get obstacle component to identify obstacle type and damage value
        Obstacle obstacle = obstacleCollider.GetComponent<Obstacle>();

        if (obstacle != null)
        {
            //get damage value from obstacle
            int damage = obstacle.GetDamage();
            
           Debug.Log("HP deduction from this obstacle was " + damage + "HP");

            //checks shields and THEN the HP, this function is on submarine 
           submarine.ConsumeShield(); //!!! might have problem, go back to elains functions if anything, 
           // my function is commentated out just in case


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
