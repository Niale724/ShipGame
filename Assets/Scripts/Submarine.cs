using Unity.VisualScripting;
using UnityEngine;

public class Submarine : MonoBehaviour
{
    //fields
    [SerializeField] private float speed;

   // [SerializeField] private HPSystem hpSystem; // drag and drop the HPSystem class here.
    private bool isShieldOn;

    //these fields are for maintaining the submarine on the screen
    private float minX, maxX, minY, maxY;
    private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Camera mainCamera = Camera.main;


        //have to convert screen coordinates(pixels) to world coordinates (inity units)
        Vector3 topRightCorner = mainCamera.ScreenToWorldPoint(new Vector3 (Screen.width, Screen.height,mainCamera.transform.position.z));

        minX = -topRightCorner.x;
        minY = -topRightCorner.y;
        maxY = topRightCorner.y;
        maxX = topRightCorner.x;

         
        spriteRenderer = GetComponent<SpriteRenderer>();
         
         //to determine how big the sprite is in the world space
        Bounds spriteBounds = spriteRenderer.bounds;

        Vector3 spriteSize = spriteBounds.size;

        float halfWidth = spriteSize.x/2;
        float halfHeight = spriteSize.y/2;

        minX += halfWidth;
        minY += halfHeight;
        maxY -= halfHeight;
        maxX -= halfWidth;
    }

    // Update is called once per frame
    void Update()
    {
        
        //get input from the keys
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical"); 


        //calculate the movement direction
        Vector3 directionMovement = new Vector3(horizontalInput, verticalInput, 0);


        UpdateShipRotation(directionMovement);

        
        //move the submarine
        transform.position += directionMovement *speed* Time.deltaTime;


        //check boundaries
        if (transform.position.x > maxX)
        {
            transform.position = new Vector3(maxX, transform.position.y,transform.position.z);
        }

        if (transform.position.x < minX)
        {
            transform.position = new Vector3(minX, transform.position.y,transform.position.z);
        }

        if (transform.position.y > maxY)
        {
            transform.position = new Vector3(transform.position.x, maxY,transform.position.z);
        }

        if (transform.position.y < minY)
        {
            transform.position = new Vector3(transform.position.x, minY,transform.position.z);
        }

        
    }

    //elained used a rb2d.linearVelocity which is the physics velocity,
    //while i used directionMovement which is input based direction
    //that's why i nned to pass the vector3 directionMovement as a paramenter on this function.
        void UpdateShipRotation(Vector3 directionMovement)
    {
        //For rotating the ship to face movement direction
        //Only udate rotation if the ship is moving
        //Otherwise it looks weird when the ship is stationary
        if (directionMovement.sqrMagnitude > 0.01f)
        {
            //Calculate angle in degrees
            //Atan2 returns radians, so convert to degrees
            //It's abbreviation for "Arc Tangent 2"
            

                if (directionMovement.x < 0)
            {
                spriteRenderer.flipX = true;
            }

                else if (directionMovement.x> 0)
            {
                spriteRenderer.flipX =false;
            }

              float angle = Mathf.Atan2(directionMovement.y, Mathf.Abs(directionMovement.x)) * Mathf.Rad2Deg;
        
        // Only rotate for vertical component
        if (Mathf.Abs(directionMovement.y) > 0.01f)
        {
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            // If only moving horizontally, keep it flat
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
            //Apply rotation to transform
            //Quaternion.AngleAxis creates a rotation which rotates angle degrees around axis
            //In simpler terms, it makes the ship face the direction of movement
            //Useful for our submarine since it has a front and back
             /*float angle = Mathf.Atan2(directionMovement.x, directionMovement.y) * Mathf.Rad2Deg;
             transform.rotation = Quaternion.AngleAxis(-angle+90f, Vector3.forward);*/
        }
    }
}
