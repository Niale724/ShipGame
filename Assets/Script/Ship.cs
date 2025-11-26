using UnityEngine;

public class Ship : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private float colliderRadius;
    //half width because we want to check from center to edge
    private float colliderHalfWidth;
    private float colliderHalfHeight;
    public float MoveForce = 10f;
    public float BoostForce = 10f;
    public float BreakForce = 8f;
    public float MaxSpeed = 25f;
    public float MaxBoostedSpeed = 50f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Reusing these from other projects
        //Some might not be used, but that's okay

        //This is just getting rigidbody
        rb2d = GetComponent<Rigidbody2D>();

        //Getting CircleCollider2D and its dimensions
        //For screen wrapping
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        colliderRadius = collider.radius;
        Vector3 circleColliderDim = collider.bounds.max - collider.bounds.min;
        colliderHalfWidth = circleColliderDim.x / 2;
        colliderHalfHeight = circleColliderDim.y / 2;

        //Some friction to slow down the ship over time
        //More realistic movement
        //*applause* *applause*
        //Could just change drag in Rigidbody2D, but why not here?
        rb2d.linearDamping = 1.5f;
        rb2d.angularDamping = 2f;
    }

    void FixedUpdate()
    {
        //Three main things to do here:
        //1. Get Movement Input from player
        Vector2 moveInput = GetMovementInput();

        //2. Apply Basic Movement
        ApplyMovement(moveInput);

        //3. Handle Boosting and Breaking
        HandleBoostAndBreak(moveInput);

        //Wrote 3 functions just to make this easier to read
        //And fill it with comments
        //Yay!
    }
    // Update is called once per frame
    void Update()
    {
        //All we need to do is get input and apply movement
        //Easy peasy lemon squeezy
        UpdateShipRotation();
    }

    Vector2 GetMovementInput()
    {
        //Handling Movement Input
        //Could it start with one? Sure! Not a good idea, but sure.
        Vector2 moveInput = Vector2.zero;

        // Keyboard Input for movement (WASD or Arrow Keys)
        //So many options for the player ^w^
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            moveInput.y += 1f;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            moveInput.y -= 1f;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            moveInput.x -= 1f;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            moveInput.x += 1f;
        }

        //Report to your officers, moveInput is ready for action
        //Roger that!
        return moveInput;
    }

    void ApplyMovement(Vector2 moveInput)
    {
        if (moveInput != Vector2.zero)
        {
            //Make sure force is applied evenly in all directions
            //Meaning diagonal movement is not faster
            //Oh physics, you tricky thing you
            Vector2 normalizedInput = moveInput.normalized;

            //Applying Force to Rigidbody2D
            rb2d.AddForce(normalizedInput * MoveForce, ForceMode2D.Force);

            //Clamping Speed to MaxSpeed
            //Because we don't want the ship to go too fast
            //Trust me, I tried, no good
            if (rb2d.linearVelocity.magnitude > MaxSpeed)
            {
                rb2d.linearVelocity = rb2d.linearVelocity.normalized * MaxSpeed;
            }
        }
    }

    void ApplyBoost(Vector2 moveInput)
    {
        if (moveInput != Vector2.zero)
        {
            //Make sure force is applied evenly in all directions
            //Same as above
            //**cough**cough**physics**
            Vector2 normalizedInput = moveInput.normalized;

            //Applying Force to Rigidbody2D
            rb2d.AddForce(normalizedInput * BoostForce, ForceMode2D.Force);
        }
        else if(rb2d.linearVelocity.sqrMagnitude>0.01f)
        {
            //If no input, boost in the current direction of movement
            //So that the ship can boost while coasting
            //Imagine the ship just flies forward
            //I know, so fun ^w^
            Vector2 boostForce =rb2d.linearVelocity.normalized * BoostForce;
            rb2d.AddForce(boostForce, ForceMode2D.Force);
        }

        //Clamping Speed to MaxBoostedSpeed
        //Same as movement, but for boosted speed
        if (rb2d.linearVelocity.magnitude > MaxBoostedSpeed)
        {
            rb2d.linearVelocity = rb2d.linearVelocity.normalized * MaxBoostedSpeed;
        }
    }

    void ApplyBreak(Vector2 moveInput)
    {
        if (rb2d.linearVelocity.sqrMagnitude > 0.01f)
        {
            //Applying Break Force opposite to current velocity
            //So that the ship slows down
            Vector2 breakForce = -rb2d.linearVelocity.normalized * BreakForce;
            rb2d.AddForce(breakForce, ForceMode2D.Force);
        }
        else
        {
            //If nearly stopped, just set velocity to zero
            //I mean, why not?
            rb2d.linearVelocity = Vector2.zero;
        }
    }
    void HandleBoostAndBreak(Vector2 moveInput)
    {
        //Space for Boosting
        //So creative. I know, I know
        if (Input.GetKey(KeyCode.Space))
        {
           ApplyBoost(moveInput);
        }

        //Shift for Breaking
        //Maybe Ctrl would be better? But Shift is more accessible
        //Just my opinion though
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            ApplyBreak(moveInput);
        }
    }
    void UpdateShipRotation()
    {
        //For rotating the ship to face movement direction
        //Only udate rotation if the ship is moving
        //Otherwise it looks weird when the ship is stationary
        if (rb2d.linearVelocity.sqrMagnitude > 0.01f)
        {
            //Calculate angle in degrees
            //Atan2 returns radians, so convert to degrees
            //It's abbreviation for "Arc Tangent 2"
            float angle = Mathf.Atan2(rb2d.linearVelocity.y, rb2d.linearVelocity.x) * Mathf.Rad2Deg;

            //Apply rotation to transform
            //Quaternion.AngleAxis creates a rotation which rotates angle degrees around axis
            //In simpler terms, it makes the ship face the direction of movement
            //Useful for our submarine since it has a front and back
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
    private void OnBecameInvisible()
    {
        WrapScreen();
    }

    void WrapScreen()
    {
        //Get the current position of the ship
        //Spot light!
        //Dun dun dun!
        Vector2 currentPosition = transform.position;

        //Check if the ship is out of screen bounds
        if (!ScreenUtils.IsInScreen(currentPosition, colliderHalfWidth, colliderHalfHeight))
        {
            //The horizontal wrapping
            if (currentPosition.x - colliderHalfWidth > ScreenUtils.ScreenRight)
            {
                currentPosition.x = ScreenUtils.ScreenLeft - colliderHalfWidth;
            }
            else if (currentPosition.x + colliderHalfWidth < ScreenUtils.ScreenLeft)
            {
                currentPosition.x = ScreenUtils.ScreenRight + colliderHalfWidth;
            }

            //The vertical wrapping
            if (currentPosition.y - colliderHalfHeight > ScreenUtils.ScreenTop)
            {
                currentPosition.y = ScreenUtils.ScreenBottom - colliderHalfHeight;
            }
            else if (currentPosition.y + colliderHalfHeight < ScreenUtils.ScreenBottom)
            {
                currentPosition.y = ScreenUtils.ScreenTop + colliderHalfHeight;
            }
            //And then apply the new position
            transform.position = currentPosition;
        }
    }
}
