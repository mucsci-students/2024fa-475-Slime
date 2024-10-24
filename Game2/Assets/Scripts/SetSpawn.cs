using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSpawn : MonoBehaviour
{
    public GameObject spawnObject;
    public GameObject player;
    public PauseMenu pauseMenu;
    public JumpKing playerScript;
    public GameObject currentSpawn = null;
    [SerializeField] private AudioClip checkPointSound;

    public void SpawnObject()
    {
        if (playerScript.isGrounded)
        {
            if (currentSpawn == null)
            {
                currentSpawn = Instantiate(spawnObject, player.transform.position, spawnObject.transform.rotation);
                SoundFXManager.instance.PlaySoundFXClip(checkPointSound, transform, 0.05f);
            }
            else
            {
                Destroy(currentSpawn);
                currentSpawn = Instantiate(spawnObject, player.transform.position, spawnObject.transform.rotation);
                SoundFXManager.instance.PlaySoundFXClip(checkPointSound, transform, 0.05f);
            }
            pauseMenu.ResumeGame();
        }        
    }
}
