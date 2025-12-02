using UnityEngine;

public class ObstacleType1 : Obstacle 
{
    protected override void Start()
    {
        hpDamage = 2;
        damageRadius = 0.2f; //danger zone

        base.Start(); //call obstacle start method
    }
}
