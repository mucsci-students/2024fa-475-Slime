using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        audioSource.Pause();
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "PlayerBox")
        {
            audioSource.Play();
        }
    }

    void OnTriggerExit2D (Collider2D other)
    {
        if (other.tag == "PlayerBox")
        {
            audioSource.Pause();
        }
    }
}
