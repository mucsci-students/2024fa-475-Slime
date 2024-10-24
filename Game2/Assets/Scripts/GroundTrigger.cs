using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTrigger : MonoBehaviour
{
    //Grass
    [SerializeField] public AudioClip landSound;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerBox")
        {
            SoundFXManager.instance.PlaySoundFXClip(landSound, transform, 0.75f);
        }
    }
}
