using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StopPlayerMovement : MonoBehaviour
{
    [SerializeField] public AudioClip jumpSound;
    [SerializeField] public AudioClip kissSound;
    public GameObject winMenu;
    public GameObject pauseMenu;
    public JumpKing playerScript;
    public GameObject princess;
    public GameObject princessAnimator;
    private Rigidbody2D rb;
    private Animator anim;
    public GameObject targetPoint;
    private bool startTimer = false;
    private bool canKiss = true;
    private bool canJump = true;
    private float timer = 0.0f;
    
    void Start()
    {
        anim = princessAnimator.GetComponent<Animator>();
        rb = princess.GetComponent<Rigidbody2D>();
        winMenu.SetActive(false);
    }
    void Update()
    {
        if (startTimer)
        {
            timer += Time.deltaTime;
            if (timer >= 12)
            {
                Destroy(pauseMenu);
                winMenu.SetActive(true);
            }
            else if (timer >= 10)
            {
                anim.SetTrigger("TriggerKiss");
                if (canKiss && timer > 10.075)
                {
                    canKiss = !canKiss;
                    SoundFXManager.instance.PlaySoundFXClip(kissSound, transform, 0.75f);
                }
                
            }
            else if (timer >= 5)
            {
                rb.velocity = new Vector2(-1.2f, rb.velocity.y);
            }
            else if (timer >= 3.25)
            {
                anim.SetTrigger("TriggerIdle");
            }
            else if (timer >= 2)
            {
                anim.SetTrigger("TriggerJump");
                if (canJump && timer > 2.1)
                {
                    canJump = !canJump;
                    SoundFXManager.instance.PlaySoundFXClip(jumpSound, transform, 0.5f);
                }
            }

            if (Mathf.Abs(princess.transform.position.x - targetPoint.transform.position.x) < 0.2f)
            {
                rb.velocity = new Vector2(0f, rb.velocity.y);
            }
        }
    }
    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "PlayerBox")
        {
            playerScript.stopMovement = true;
            startTimer = !startTimer;
        }
    }
    
}
