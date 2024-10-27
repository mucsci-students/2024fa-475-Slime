using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportSpawn : MonoBehaviour
{
    public GameObject player;
    public PauseMenu pauseMenu;
    public SetSpawn spawnScript;
    // public JumpKing playerScript;
    [SerializeField] private AudioClip teleportSound;

    public void TeleportToSpawn()
    {
        //if (spawnScript.currentSpawn != null && playerScript.isGrounded)
        if (spawnScript.currentSpawn)
        {
            player.transform.position = spawnScript.currentSpawn.transform.position;
            SoundFXManager.instance.PlaySoundFXClip(teleportSound, transform, 0.15f);
            pauseMenu.ResumeGame();
        }
    }
}
