using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkTrigger : MonoBehaviour
{
    // Land
    public GroundTrigger groundTriggerScript;
    public JumpKing playerScript;
    [SerializeField] private AudioClip landLandSound;
    [SerializeField] private AudioClip landJumpSound;
    [SerializeField] private AudioClip landFacePlantSound;

    // Snow
    [SerializeField] private AudioClip snowLandSound;
    [SerializeField] private AudioClip snowJumpSound;
    [SerializeField] private AudioClip snowFacePlantSound;

    // Ice
    [SerializeField] private AudioClip iceLandSound;
    [SerializeField] private AudioClip iceJumpSound;
    [SerializeField] private AudioClip iceFacePlantSound;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerBox" && gameObject.tag == "LandChunks")
        {
            groundTriggerScript.landSound = landLandSound;
            playerScript.jumpSound = landJumpSound;
            playerScript.facePlantSound = landFacePlantSound;
        }
        else if (other.tag == "PlayerBox" && gameObject.tag == "SnowChunks")
        {
            groundTriggerScript.landSound = snowLandSound;
            playerScript.jumpSound = snowJumpSound;
            playerScript.facePlantSound = snowFacePlantSound;
        }
        else if (other.tag == "PlayerBox" && gameObject.tag == "IceChunks")
        {
            groundTriggerScript.landSound = iceLandSound;
            playerScript.jumpSound = iceJumpSound;
            playerScript.facePlantSound = iceFacePlantSound;
        }
    }
}
