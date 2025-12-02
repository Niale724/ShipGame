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
            Debug.Log("Submarine was hit by tagless" + otherCollider.gameObject.name);
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

        Destroy(shieldCollider.gameObject);
    }

    private void ObstacleCollision(Collider2D obstacleCollider)
    {
        Debug.Log("Obstacle Hit " + obstacleCollider.gameObject.name);

        Destroy(obstacleCollider.gameObject);
    }

    

   


    // Update is called once per frame
    /*
    void Update()
    {
        
    }*/
}
