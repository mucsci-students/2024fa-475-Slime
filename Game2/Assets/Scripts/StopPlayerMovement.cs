using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StopPlayerMovement : MonoBehaviour
{
    public JumpKing playerScript;
    public GameObject princess;
    public GameObject princessAnimator;
    private Rigidbody2D rb;
    private Animator anim;
    public GameObject targetPoint;
    private bool startTimer = false;
    private float timer = 0.0f;
    
    void Start()
    {
        anim = princessAnimator.GetComponent<Animator>();
        rb = princess.GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (startTimer)
        {
            timer += Time.deltaTime;
            if (timer >= 8)
            {
                anim.SetTrigger("TriggerKiss");
            }
            else if (timer >= 4)
            {
                rb.velocity = new Vector2(-1.5f, rb.velocity.y);
            }
            else if (timer >= 3)
            {
                anim.SetTrigger("TriggerIdle");
            }
            else if (timer >= 2)
            {
                anim.SetTrigger("TriggerJump");
            }

            if (Mathf.Abs(princess.transform.position.x - targetPoint.transform.position.x) < 0.2f)
            {
                rb.velocity = new Vector2(0f, rb.velocity.y);
            }
        }
    }
    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerScript.stopMovement = true;
            startTimer = !startTimer;
        }
    }
    
}
