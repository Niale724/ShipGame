using UnityEngine;

public class ObstacleType2 : Obstacle
{
    private float moveSpeed = 7f;
    private float circleRadius = 0.5f; //the size of the size the obstacle moves around

    private float rotationSpeed =130f; // how fast the blade would spin
    private Vector2 centerposition;
    private float angle = 0f; 

    //override because the class it's using its own version 
    protected override void Start()
    {
        hpDamage =3;
        damageRadius =0.2f; // danger zone
        base.Start(); //this is calling the obstacle start method 
        //remember where we started
        centerposition = transform.position;
    
    }

  
    protected override void Update()
    {
        //makes the obstacle rotate on its own center
        transform.Rotate(0, 0, rotationSpeed  * Time.deltaTime);

        //increase the angle over time
        angle += moveSpeed * Time.deltaTime;

        //calculate new position on circle 
        float x = centerposition.x + Mathf.Cos(angle) * circleRadius;
        float y = centerposition.y + Mathf.Sin(angle) * circleRadius;
        //it doesn't matter how big the angle is because cos and sin oscilate between 1 and -1
        // circleRadius is always the same 
        // angle determines the position along the circle


        //move to the new position 
        transform.position = new Vector2(x, y);
    }
}
