using Unity.VisualScripting;
using UnityEngine;

public class CollisionManager : MonoBehaviour

{   
    //reference to the other system
    //we need references because we are gonna use these other system's methods when the collisions happen 
    //[SerializeField] private GameManager gameManager;
    [SerializeField] private HpSystem hpSystem;

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
        if (fishCollider == null) return;

        GameObject fishObj = fishCollider.gameObject;
        Debug.Log($"Fish Collected: {fishObj.name}");

        var collectible = fishObj.GetComponent<Collectible>();
        if (collectible == null)
        {
            Destroy(fishObj);
            return;
        }

        collectible.MarkAsCollected();

        if (hpSystem != null)
        {
            hpSystem.IncreaseHP(collectible.PtValue);
            Debug.Log($"HP +{collectible.PtValue}");
        }

        Destroy(fishObj);
    }
     private void ShieldCollision(Collider2D shieldCollider)
    {
        if (shieldCollider == null) return;

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
        Debug.Log("Obstacle Hit " + obstacleCollider.gameObject.name);

        Obstacle obstacle = obstacleCollider.GetComponent<Obstacle>();
        int damage = 1;
        if (obstacle != null)
        {
            damage = obstacle.GetDamage();
        }

        if(submarine != null && submarine.HasShield())
        {
            submarine.ConsumeShield();
            Debug.Log("Shield absorbed the damage. Remaining Shields: " + submarine.ShieldStacks);
        }
        else if (hpSystem != null)
        {
            hpSystem.DecreaseHP(damage);
            Debug.Log($"Submarine took {damage} damage. HP: {hpSystem.CurrentHp}");
        }

        Destroy(obstacleCollider.gameObject);
    }

   
    // Update is called once per frame
    /*
    void Update()
    {
        
    }*/

}
