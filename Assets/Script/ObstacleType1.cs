using UnityEngine;

public class ObstacleType1 : Obstacle 
{
    private float floatAmplitude = 0.17f; //movement distance
    private float floatFrequency =4f; //speed 
    
    private Vector3 startPosition; //remember where we spawned

    protected override void Start()
    {
        hpDamage = 2;
        damageRadius = 0.2f; //danger zone

        //used for floating calculation
        startPosition = transform.position;

        base.Start(); //call obstacle start method

    }

    protected override void Update()
    {
        base.Update();
        
        ApplyFloatEffect();
    }

    private void ApplyFloatEffect()
    {
        //sin wave is used to make the movement smooth
        float newY = startPosition.y + Mathf.Sin(Time.time * floatFrequency)*floatAmplitude;
    
        //applying the y changes to the position
        transform.position = new Vector3 (transform.position.x, newY, transform.position.z);
    }
}
