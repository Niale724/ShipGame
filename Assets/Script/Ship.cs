using UnityEngine;

public class Ship : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Vector2 thrustDirection = new Vector2(1, 0);
    public float ThrustForce = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (Input.GetButton("Thrust"))
        {
            rb2d.AddForce(thrustDirection * ThrustForce, ForceMode2D.Force);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
