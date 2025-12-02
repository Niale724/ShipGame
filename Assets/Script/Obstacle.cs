using UnityEngine;

public class Obstacle : MonoBehaviour
{

    protected int hpDamage; // protected so the children of Obstacle can have access to it
    protected float damageRadius; // how big the collision circle is

    // a way to detect collisions
    protected CircleCollider2D obstacleCollider;
    
    
    //protected for the inheretance and virtual so the children can 
    //overide this method (implement their own versions)
    protected virtual void Start()
    {
        obstacleCollider = GetComponent<CircleCollider2D>();

        //get or add a ciclecollider2d component
        if (obstacleCollider == null)
        {
            obstacleCollider = gameObject.AddComponent<CircleCollider2D>();
        }

        //set the collider to be trigger
        obstacleCollider.isTrigger = true;

        //set the radius
        obstacleCollider.radius = damageRadius;
    }

    public int GetDamage()
    {
        return hpDamage;
    }

    
    protected virtual void Update()
    {
        
    }
}
