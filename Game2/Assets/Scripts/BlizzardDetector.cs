using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlizzardDetection : MonoBehaviour 
{
    public JumpKing playerScript;
    void OnTriggerEnter2D (Collider2D other)
    {
        // Adds blizzard force
        if (other.tag == "ThickSnow")
        {
            playerScript.cf.enabled = false;
        }
        else if (other.tag == "IceChunks" || other.tag == "LandChunks")
        {
            playerScript.cf.enabled = false;
        }
        else if (other.tag == "SnowChunks" || other.tag == "Snow")
        {
            playerScript.cf.enabled = true;
        }
    }

    void OnTriggerExit2D (Collider2D other)
    {
        if (other.tag == "ThickSnow")
        {
            playerScript.cf.enabled = true;
        }
        else if (other.tag == "SnowChunks")
        {
            playerScript.cf.enabled = false;
        }
    }
}
