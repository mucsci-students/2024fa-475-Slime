using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;

public class JumpKing : MonoBehaviour
{
    // Inputs
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask iceGroundLayer;
    [SerializeField] private LayerMask iceSlopesLayer;
    [SerializeField] private float maxJumpValue = 17.5f;
    // [SerializeField] private float jumpIncreaseValue = 0.025f;
    [SerializeField] private float splatVelocityValue = -20f;
    [SerializeField] private float jumpResetDelay = 0.25f;
    [SerializeField] private float horizontalDistance = 6f;

    // Non-Inputs
    private Rigidbody2D rb;
    private Rigidbody2D wb;
    private Animator anim;
    public float walkSpeed = 3f;
    private float moveInput;
    public bool isGrounded;
    public bool isOnIceGround;
    public bool isOnIceSlopes;
    private bool canJump = true;
    private bool canMove = true;
    private bool canFlip = true;
    public bool isJumping = false;
    private bool isWallBouncing = false;
    public bool isSplat = false;
    public bool isFalling = false;
    private bool isFacingRight = true;
    private float jumpResetTimer;
    public float jumpValue = 0.0f;

    // Tilemaps
    public TilemapCollider2D ground;
    public TilemapCollider2D walls;

    // Materials
    public PhysicsMaterial2D normalMat, bounceMat;

    // Audio
    [SerializeField] public AudioClip wallBounceSound;
    [SerializeField] public AudioClip jumpSound;

    // Ice 
    [SerializeField] private float iceSpeed;

    // ConstantForce
    public ConstantForce2D cf;
    public float cfTimer = 0.0f;

    // Princess
    public bool stopMovement = false;
    public bool stopJump = false;

    // Particles
    public GameObject particleSpawn;
    private GameObject currentParticles = null;
    private bool canNewParticle = true;


    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        wb = walls.GetComponent<Rigidbody2D>();
        cf = gameObject.GetComponent<ConstantForce2D>();
        anim = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        jumpResetTimer += Time.deltaTime;

        //Debug.Log ("Current X velocity is " + rb.velocity.x + "  |  Current Y velocity is " + rb.velocity.y);
        if (stopMovement == false)
        {
            moveInput = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            rb.velocity = new Vector2(0.0f, 0.0f);
            anim.SetBool("isRunning", false);
        }
        
        if (canFlip)
        {
            Flip();
        }

        //checks if on ice
        isOnIceGround = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.35f),
        new Vector2(0.45f, 0.2f), 0f, iceGroundLayer);
        isOnIceSlopes = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.35f),
        new Vector2(0.45f, 0.2f), 0f, iceSlopesLayer);

        //checks if grounded
        isGrounded = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.35f),
        new Vector2(0.45f, 0.2f), 0f, groundLayer);
        anim.SetBool("isGrounded", isGrounded);


        // Triggers the jump animation, increases the jump value by jumpIncreaseValue, with a max jumpValue of maxJumpValue
            if (canJump)
            {
                if (Input.GetKeyDown("space") && isGrounded)
                {
                    rb.velocity = new Vector2 (0.0f, rb.velocity.y);
                }

                if (Input.GetKey("space") && isGrounded && !stopJump)
                {
                    rb.velocity = new Vector2 (0.0f, rb.velocity.y);
                    anim.SetBool("isRunning", false);
                    anim.SetTrigger("TriggerJump");

                    if (jumpValue < maxJumpValue)
                    {
                        jumpValue += maxJumpValue * Time.deltaTime;
                    }
                }
                if (Input.GetKeyUp("space"))
                {
                    ground.enabled = false;
                    SoundFXManager.instance.PlaySoundFXClip(jumpSound, transform, 1f);
                    if (canNewParticle)
                    {
                        canNewParticle = !canNewParticle;
                        currentParticles = Instantiate(particleSpawn, transform.position, particleSpawn.transform.rotation);
                        StartCoroutine(destroyParticles());
                    }
                    
                    isJumping = true;
                    if(isGrounded)
                    {
                        rb.velocity = new Vector2 (moveInput * walkSpeed, jumpValue);
                        jumpValue = 0.0f;
                    }
                    canMove = true;
                    canJump = false;
                }
        }
        
        
        // If the player's velocity is increasing, trigger the leap animation, otherwise trigger the falling animation
        if (isJumping)
        {
            if (rb.velocity.y > 0f)
            {
                anim.SetTrigger("TriggerLeap");
            }
            else if (rb.velocity.y < -3.5f)
            {
                anim.SetTrigger("TriggerFall");
            }     
        }

        // Triggers the wall bounce state
        if (isWallBouncing)
        {
            anim.SetTrigger("TriggerWallBounce");
            
        }
        // else
        // {
        //     ground.enabled = true;
        // }

        // Sets the Splat boolean to true if the player is falling at high speeds
        if (rb.velocity.y < splatVelocityValue)
        {
            isSplat = true;
            canMove = false;
            canJump = false;
            canFlip = false;
        }

        // Allows the player to enter the falling animation when walking off of a platform
        if (!isGrounded && isJumping == false)
        {
            anim.SetBool("isRunning", false);
            anim.SetTrigger("TriggerFall");
        } 

        // Checks if the player is falling
        if (!isGrounded && !isJumping && !isWallBouncing)
        {
            isFalling = true;
        }

        // Adds blizzard velocity force to player
        cfTimer += Time.deltaTime;
        if (cfTimer >= 5)
        {
            cf.force = new Vector2(cf.force.x * -1, cf.force.y);
            cfTimer = 0.0f;
        }
    }

    // Allows the character to move left, right, and jump, which restrics horizontal movement
    void FixedUpdate()
    {
        if (isGrounded || isFalling)
        {
            wb.sharedMaterial = normalMat;
        }
        else if (!isGrounded && isWallBouncing)
        {
            wb.sharedMaterial = bounceMat;
        }
        // if (!stopMovement)
        // {
            if (!PauseMenu.isPaused)
            {
                if (jumpValue == 0.0f && isGrounded && canMove)
                {
                    if (jumpResetTimer >= jumpResetDelay)
                    {
                        if (Input.GetKey("space") && !isOnIceSlopes)
                        {
                            if (stopJump == false)
                            {
                                canJump = true;
                                canMove = false;
                                anim.SetBool("isSplat", false);
                                isSplat = false;
                                jumpResetTimer = 0f;
                            }
                            else
                            {
                                canJump = false;
                            }  
                        }
                    }
                    
                    if (moveInput < 0f)
                    {
                        anim.SetBool("isRunning", true);
                        anim.SetBool("isSplat", false);
                        isSplat = false;
                    }
                    else if (moveInput > 0f)
                    {
                        anim.SetBool("isRunning", true);
                        anim.SetBool("isSplat", false);
                        isSplat = false;
                    }
                    else
                    {
                        anim.SetBool("isRunning", false);
                    }

                    // checks if on ice
                    if (canMove && isJumping)
                    {
                        rb.velocity = new Vector2(moveInput * horizontalDistance, rb.velocity.y);
                        
                    }
                    else
                    {
                        if (isOnIceGround || isOnIceSlopes)
                        {
                            rb.AddForce(new Vector2 (moveInput * iceSpeed, rb.velocity.y));
                        }
                        else
                        {
                            rb.velocity = new Vector2(moveInput * walkSpeed, rb.velocity.y);
                        }
                    }
                }   
            }
        //}
    }

    //If you are on the ground, return to the idle state, if you hit a wall, trigger the wallBounce state
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ground")
        {
            ground.enabled = true;
            // Sets player back to idle state booleans
            wb.sharedMaterial = normalMat;
            isFalling = false;
            isJumping = false;
            isWallBouncing = false;            

            // Sets the face plant state in decreasing velocity < SplatVelocityValue, otherwise go to idle state
            if (isSplat)
            {
                anim.SetBool("isSplat", true);
                if (isOnIceGround || isOnIceSlopes)
                {
                    rb.AddForce(new Vector2 (moveInput * iceSpeed, rb.velocity.y));
                }
                else
                {
                    rb.velocity = new Vector2(0.0f, 0.0f);
                }
                wb.sharedMaterial = normalMat;
                StartCoroutine(preventMovement());
                
            }
            else 
            {
                // Triggers the idle animation
                anim.SetTrigger("TriggerIdle");
            }
        }
        // Sets up the wallBounce animation
        else if (other.tag == "Walls" && isJumping)
        {   
            SoundFXManager.instance.PlaySoundFXClip(wallBounceSound, transform, 0.75f);
            isJumping = false;
            isWallBouncing = true;
        }

        
    }
    // Prevents the player from moving once they are in the Splat state for a duration of time
    private IEnumerator preventMovement()
    {
        yield return new WaitForSeconds (2f);
        canMove = true;
        canJump = true;
        canFlip = true;
    }

    private IEnumerator destroyParticles()
    {
        yield return new WaitForSeconds (.25f);
        Destroy(currentParticles);
        canNewParticle = !canNewParticle;
    }

    // Rotates the sprite around to face the other direction
    void Flip()
    {
        if (isFacingRight && moveInput < 0f || !isFacingRight && moveInput > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }

    //Draw a box that shows the player's isGrounded hitbox
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.35f),
        new Vector2(0.5f, 0.2f));
         Gizmos.color = Color.blue;
        Gizmos.DrawCube(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.35f),
        new Vector2(0.45f, 0.2f));
    }

}
