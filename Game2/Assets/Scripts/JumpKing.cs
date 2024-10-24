using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class JumpKing : MonoBehaviour
{
    // Inputs
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float maxJumpValue = 17.5f;
    [SerializeField] private float jumpIncreaseValue = 0.025f;
    [SerializeField] private float facePlantVelocityValue = -20f;
    [SerializeField] private float jumpResetDelay = 0.25f;
    [SerializeField] private float horizontalDistance = 6f;

    // Non-Inputs
    private Rigidbody2D rb;
    private Rigidbody2D wb;
    private Animator anim;
    public float walkSpeed = 3f;
    private float moveInput;
    public bool isGrounded;
    private bool canJump = true;
    private bool canMove = true;
    private bool canFlip = true;
    public bool isJumping = false;
    private bool isWallBouncing = false;
    private bool isFacePlant = false;
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
    [SerializeField] public AudioClip facePlantSound;

    private bool isFalling = false;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        wb = walls.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        jumpResetTimer += Time.deltaTime;

        Debug.Log ("Current X velocity is " + rb.velocity.x + "  |  Current Y velocity is " + rb.velocity.y);
        moveInput = Input.GetAxisRaw("Horizontal");
        
        if (canFlip)
        {
            Flip();
        }

        isGrounded = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.35f),
        new Vector2(0.5f, 0.2f), 0f, groundLayer);
        anim.SetBool("isGrounded", isGrounded);

        if (isGrounded || isFalling)
        {
            wb.sharedMaterial = normalMat;
        }
        else
        {
            wb.sharedMaterial = bounceMat;
        }

        // Triggers the jump animation, increases the jump value by jumpIncreaseValue, with a max jumpValue of maxJumpValue
        if (canJump)
        {
            if (Input.GetKeyDown("space") && isGrounded)
            {
                rb.velocity = new Vector2 (0.0f, rb.velocity.y);
            }
            if (Input.GetKey("space") && isGrounded)
            {
                rb.velocity = new Vector2 (0.0f, rb.velocity.y);
                anim.SetBool("isRunning", false);
                anim.SetTrigger("TriggerJump");

                if (jumpValue == 0.0f)
                {
                    jumpValue += 2f;
                }
                else if (jumpValue < maxJumpValue)
                {
                    jumpValue += jumpIncreaseValue;
                }
            }
            if (Input.GetKeyUp("space"))
            {
                SoundFXManager.instance.PlaySoundFXClip(jumpSound, transform, 1f);
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
            ground.enabled = false;
            anim.SetTrigger("TriggerWallBounce");
        }
        else
        {
            ground.enabled = true;
        }

        // Sets the facePlant boolean to true if the player is falling at high speeds
        if (rb.velocity.y < facePlantVelocityValue)
        {
            isFacePlant = true;
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

        // Prevents the player from bouncing upon landing from high place
        if (rb.velocity.y < -10)
        {
            isFalling = true;
        }
    }

    // Allows the character to move left, right, and jump, which restrics horizontal movement
    void FixedUpdate()
    {
        if (!PauseMenu.isPaused)
        {
            if (jumpValue == 0.0f && isGrounded && canMove)
            {
                if (jumpResetTimer >= jumpResetDelay)
                {
                    if (Input.GetKey("space"))
                    {
                        canJump = true;
                        canMove = false;
                        anim.SetBool("isFacePlant", false);
                        isFacePlant = false;
                        jumpResetTimer = 0f;
                    }
                }
                
                if (moveInput < 0f)
                {
                    anim.SetBool("isRunning", true);
                    anim.SetBool("isFacePlant", false);
                    isFacePlant = false;
                }
                else if (moveInput > 0f)
                {
                    anim.SetBool("isRunning", true);
                    anim.SetBool("isFacePlant", false);
                    isFacePlant = false;
                }
                else
                {
                    anim.SetBool("isRunning", false);
                }

                if (canMove && isJumping)
                {
                    rb.velocity = new Vector2(moveInput * horizontalDistance, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(moveInput * walkSpeed, rb.velocity.y);
                }
            }   
        }
    }

    //If you are on the ground, return to the idle state, if you hit a wall, trigger the wallBounce state
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ground")
        {
            // Sets wall bounce
            isJumping = false;
            isWallBouncing = false;
            isFalling = false;

            // Sets the face plant state in decreasing velocity < facePlantVelocityValue, otherwise go to idle state
            if (isFacePlant)
            {
                anim.SetBool("isFacePlant", true);
                SoundFXManager.instance.PlaySoundFXClip(facePlantSound, transform, 0.5f);
                rb.velocity = new Vector2(0.0f, 0.0f);
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

    // Prevents the player from moving once they are in the facePlant state for a duration of time
    private IEnumerator preventMovement()
    {
        yield return new WaitForSeconds (2f);
        canMove = true;
        canJump = true;
        canFlip = true;
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
    }

}
