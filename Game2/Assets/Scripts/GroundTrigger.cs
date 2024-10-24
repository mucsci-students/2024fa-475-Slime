using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTrigger : MonoBehaviour
{
    [SerializeField] public AudioClip landSound;
    [SerializeField] public AudioClip splatSound;
    public JumpKing playerScript;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerBox" && playerScript.isSplat)
        {
            SoundFXManager.instance.PlaySoundFXClip(splatSound, transform, 0.75f);
        }
        else if (other.tag == "PlayerBox")
        {
            SoundFXManager.instance.PlaySoundFXClip(landSound, transform, 0.75f);
        }
    }
}
