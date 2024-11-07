using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopPlayerJump : MonoBehaviour
{
    public JumpKing playerScript;
    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerScript.stopJump = true;
        }
    }
}
