using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    public GameObject player;
    public PauseMenu pauseMenu;
    public void Position()
    {
        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + .4f, 0);
        pauseMenu.ResumeGame();
    }
}
