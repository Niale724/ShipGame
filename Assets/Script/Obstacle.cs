using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] protected int hpDamage=1; // protected so the children of Obstacle can have access to it
    [SerializeField] protected float damageRadius=0.5f; // how big the collision circle is

    // a way to detect collisions
    protected CircleCollider2D obstacleCollider;
    
    
    //protected for the inheretance and virtual so the children can 
    //overide this method (implement their own versions)
    protected virtual void Start()
    {
        obstacleCollider = GetComponent<CircleCollider2D>();

        //get or add a ciclecollider2d component for collision detection
        if (obstacleCollider == null)
        {
            obstacleCollider = gameObject.AddComponent<CircleCollider2D>();
        }

        //set the collider to be a trigger (this doesn't block the movement)
        obstacleCollider.isTrigger = true;

        //set the radius
        obstacleCollider.radius = damageRadius;
    }
     
     //this is called by collisionManager tl determine how much damage to apply for each obstacle
    public int GetDamage()
    {
        return hpDamage;
    }

    
    protected virtual void Update()
    {
        
    }
}
