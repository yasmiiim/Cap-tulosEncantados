using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetManager : MonoBehaviour
{
    public List<nuvem> clouds = new List<nuvem>();
    public List<FallingPlatform> fallingPlatforms = new List<FallingPlatform>();
    public List<fallingSpike> spikes = new List<fallingSpike>();

    public void ResetAll()
    {
        foreach (var cloud in clouds)
        {
            if (cloud != null)
                cloud.ResetCloud();
        }

        foreach (var platform in fallingPlatforms)
        {
            if (platform != null)
                platform.ResetPlatform();
        }

        foreach (var spike in spikes)
        {
            if (spike != null)
                spike.ResetSpike();
        }
    }
}
