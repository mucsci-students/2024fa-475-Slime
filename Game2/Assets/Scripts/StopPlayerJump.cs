using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopPlayerJump : MonoBehaviour
{
    public JumpKing playerScript;
    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "PlayerBox")
        {
            playerScript.stopJump = true;
        }
    }

    void OnTriggerExit2D (Collider2D other)
    {
        if (other.tag == "PlayerBox")
        {
            playerScript.stopJump = false;
        }
    }
}
