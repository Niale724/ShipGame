using UnityEngine;

public class ObstacleType3 : Obstacle
{
    protected override void Start()
    {
        hpDamage =5;
        damageRadius = 1f; //danger zone is larger here

        base.Start(); //call obstacle start method

    }
}
