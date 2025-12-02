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
        //i used getaxisraw for instant response 
        //this makes direction changes immediate
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical"); 


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
        if (directionMovement.sqrMagnitude > 0.01f)
            {
                // handle horizontal flipping
                if (directionMovement.x < 0)
                {
                    spriteRenderer.flipX = true; //flip to face left
                }
                else if (directionMovement.x > 0)
                {   
                    spriteRenderer.flipX = false; //flip right
                }
                
                //calculate angle on vertical movement
                float angle = 0f;
                
                if (directionMovement.y> 0.01f)
                    {
                        //move up
                        angle = 90f;
                    }

                else if (directionMovement.y <- 0.01f)
                    {   //move down
                        angle = -90f;
                    }
                else
                    {
                        //move horizontally
                        angle = 0f;
                    }
            
                //if sprite is flipped, invert the angle. 
                //if this isn't done, the submarine would be upside down after pressing the left key and trying to move up or down
                if (spriteRenderer.flipX)
                    {
                        angle = -angle;
                    }
                
                //apply rotation
                transform.rotation = Quaternion.Euler(0, 0, angle);
            
            }
        }

    
    //why do we add this here? 
    //because unity calls this method on the GAMEOBJECT that HAS the collider that was hit
    void OnTriggerEnter2D(Collider2D anotherCollider)
    {
        CollisionManager collisionManager = FindObjectOfType<CollisionManager>();

        if (collisionManager != null)
        {
            collisionManager.SubmarineCollision(anotherCollider);

        }

        else
        {
            Debug.Log("No CollisionManager found in the scene");
        }
    }


}
