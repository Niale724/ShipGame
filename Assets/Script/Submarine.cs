using Unity.VisualScripting;
using UnityEngine;

public class Submarine : MonoBehaviour
{
    //fields
    [SerializeField] private float speed;

   // [SerializeField] private HPSystem hpSystem; // drag and drop the HPSystem class here.
    private bool isShieldOn = false;
    private int shieldStacks = 0;
    private HpSystem hpSystem;
    public int ShieldStacks
    {
        get=> shieldStacks;
        set
        {
            shieldStacks=Mathf.Max(0,value);
            isShieldOn = shieldStacks > 0;
        }
    }
    public bool IsShieldActive => isShieldOn;

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

        InitializeHpSystem();
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

    //New methods for HP
    private void InitializeHpSystem()
    {
        if (hpSystem == null)
        {
            hpSystem = GetComponent<HpSystem>();
            if (hpSystem == null)
            {
                Debug.LogError("HpSystem component not found on Submarine GameObject.");
            }
        }
        if (hpSystem != null)
        {
            hpSystem.OnHpChanged += HandleHpChanged;
            hpSystem.OnDeath += HandleDeath;
        }
    }
    public void HandleHpChanged(int currentHp, int changeHp)
    {
        Debug.Log($"Submarine HP changed. Current HP: {currentHp}, Change in HP: {changeHp}");
        if (changeHp < 0)
        {
            Debug.Log("Submarine took damage.");
        }
        else if (changeHp > 0)
        {
            Debug.Log("Submarine healed.");
        }
    }

    public void HandleDeath()
    {
        Debug.Log("Submarine destroyed.");
        //Disable submarine controls or trigger death animation
        enabled = false;
        //Additional logic for submarine death can be added here
    }

    //New method to collect shield
    public void CollectShield()
    {
        shieldStacks++;
        Debug.Log($"Shield collected. Available shields: {shieldStacks}");
        //Additional logic for activating shield effects can be added here
    }

    /*
    public void ActivateShield(Obstacle obstacle)
     {
         if (isShieldOn||shieldStacks > 0)
         {
            if (shieldStacks > 0)
                Debug.Log("Shield absorbed damage");
            return;
        }
         
         hpSystem.DecreaseHP(obstacle.GetDamage());
      } 
    */

    /*

    //bridge with collision system and hp/shield system
    public void TakeDamage(int damage)
    {
        //check if there're shields to absorb the damage
        
        if (shieldStacks > 0)
        {   

            //shield absorbs the damage
           shieldStacks--;
           Debug.Log($"Shield absorbed {damage} damage! Shields left: {shieldStacks}");
           return; 

        }
        // as there's no shields available, apply the HP system damage
         if(hpSystem!= null)
        {
            hpSystem.DecreaseHP(damage);
        }


    }
    */

    public void ConsumeShield()
    {
        if (shieldStacks > 0)
        {
            shieldStacks--;
            isShieldOn = shieldStacks > 0;
            Debug.Log($"Shield absorbed damage. Remaining shields: {shieldStacks}");
        }
        else
        {
            Debug.Log("No shields available to absorb damage.");
        }
    }


}
