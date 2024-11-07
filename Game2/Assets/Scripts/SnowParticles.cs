using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SnowParticles : MonoBehaviour
{
    public JumpKing playerScript;
    public GameObject particleSpawn;
    public GameObject currentParticles = null;
    
    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "PlayerBox")
        currentParticles = Instantiate(particleSpawn, new Vector3(playerScript.transform.position.x, playerScript.transform.position.y - .25f, 0), particleSpawn.transform.rotation);
        StartCoroutine(destroyParticles());
    }

    // Update is called once per frame
    private IEnumerator destroyParticles()
    {
        yield return new WaitForSeconds (.4f);
        Destroy(currentParticles);
    }
}
