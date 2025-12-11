using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class CollisionManager : MonoBehaviour

{   
 
    [SerializeField] private Submarine submarine;
    [SerializeField] private HpSystem hpSystem;

    /*monobehavior class that can be attached to a gameobject*/

    void Start()
    {
        Debug.Log("Collision Manager started working");
    }

    //rigidbody2d unity this object participated IN PHYSICS.
    //collider2d defines the collision SHAPE.

    //use ontrigger because you DETECT when things touch
    //cause submarine is BOUNCING OFF obstacles

    private void OnTriggerEnter2D(Collider2D another)
    {
        SubmarineCollision(another);
    }
    public void SubmarineCollision(Collider2D otherCollider)
    {
        Debug.Log("Hit: " + otherCollider.tag);
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
        if (fishCollider == null) return;

        var collectible = fishCollider.GetComponent<Collectible>();
        if (collectible == null) return;

        if (collectible.IsCollected)
        {
            Debug.Log("Fish already collected, ignoring duplicate collision.");
            return;
        }

        collectible.MarkAsCollected();
        GameManager.Instance.FishCollected(null);

        Debug.Log($"Fish Collected: {fishCollider.gameObject.name}");

        hpSystem?.IncreaseHP(collectible.PtValue);

        Destroy(fishCollider.gameObject);
    }


private void ShieldCollision(Collider2D shieldCollider)
    {
        if (shieldCollider == null) return;

        GameObject shieldObj = shieldCollider.gameObject;
        Debug.Log($"Shield Collected: {shieldObj.name}");

        var collectible = shieldObj.GetComponent<Collectible>();
        if (collectible != null)
        {
            if (collectible.IsCollected)
            {
                Debug.Log("Shield already collected, ignoring duplicate collision.");
                return;
            }

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

        if (obstacle == null)
            return;

        if (obstacle.HasDealtDamage)
        {
            Debug.Log("Obstacle already dealt damage, ignoring duplicate collision.");
            return;
        }

        obstacle.MarkAsHit();

        int damage = obstacle.GetDamage();

        if (submarine != null && submarine.HasShield())
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


}
