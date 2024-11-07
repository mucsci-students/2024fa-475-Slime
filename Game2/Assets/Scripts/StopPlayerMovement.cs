using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StopPlayerMovement : MonoBehaviour
{
    public JumpKing playerScript;
    public GameObject princess;
    private Animator anim;
    private bool startTimer = false;
    private float timer = 0.0f;
    void Start()
    {
        anim = princess.GetComponent<Animator>();
    }
    void Update()
    {
        if (startTimer)
        {
            timer += Time.deltaTime;
            if (timer >= 2)
            {
                anim.SetTrigger("TriggerJump");
            }
            else if (timer >= 5)
            {
                anim.SetTrigger("TriggerKiss");
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
