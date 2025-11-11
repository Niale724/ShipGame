using UnityEngine;

public class Ship : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Vector2 thrustDirection = new Vector2(1, 0);
    public float ThrustForce = 5f;
    private float colliderRadius;
    private float colliderHalfWidth;
    private float colliderHalfHeight;
    public float RotateDegreesPerSecond = 180f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        colliderRadius = collider.radius;

        Vector3 circleColliderDim = collider.bounds.max - collider.bounds.min;
        colliderHalfWidth = circleColliderDim.x / 2;
        colliderHalfHeight = circleColliderDim.y / 2;
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
        float rotationInput = Input.GetAxis("Rotate");
        float rotationAmount = RotateDegreesPerSecond * Time.deltaTime;
        if (rotationInput < 0)
        {
            rotationAmount *= -1;
        }
        transform.Rotate(Vector3.forward, rotationAmount);

    }

    private void OnBecameInvisible()
    {
        WrapScreen();
    }

    void WrapScreen()
    {
        Vector2 currentPosition = transform.position;

        if (!ScreenUtils.IsInScreen(currentPosition, colliderHalfWidth, colliderHalfHeight))
        {
            if (currentPosition.x - colliderHalfWidth > ScreenUtils.ScreenRight)
            {
                currentPosition.x = ScreenUtils.ScreenLeft - colliderHalfWidth;
            }
            else if (currentPosition.x + colliderHalfWidth < ScreenUtils.ScreenLeft)
            {
                currentPosition.x = ScreenUtils.ScreenRight + colliderHalfWidth;
            }

            if (currentPosition.y - colliderHalfHeight > ScreenUtils.ScreenTop)
            {
                currentPosition.y = ScreenUtils.ScreenBottom - colliderHalfHeight;
            }
            else if (currentPosition.y + colliderHalfHeight < ScreenUtils.ScreenBottom)
            {
                currentPosition.y = ScreenUtils.ScreenTop + colliderHalfHeight;
            }

            transform.position = currentPosition;
        }
    }
}
