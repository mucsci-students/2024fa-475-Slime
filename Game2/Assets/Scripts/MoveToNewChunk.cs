using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToNewChunk : MonoBehaviour
{
    public GameObject cam;

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "PlayerBox")
        {
            cam.SetActive(true);
        }
    }

    void OnTriggerExit2D (Collider2D other)
    {
        if (other.tag == "PlayerBox")
        {
            cam.SetActive(false);
        }
    }
}
