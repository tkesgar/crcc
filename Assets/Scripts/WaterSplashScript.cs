using UnityEngine;
using System.Collections;

public class WaterSplashScript : MonoBehaviour {

    /// <summary>
    /// The splash prefab to be instantiated.
    /// </summary>
    public GameObject splashPrefab;

    /// <summary>
    /// The duration of splash object to be kept before destroyed, in seconds.
    /// </summary>
    public float splashLifetime;

    void OnTriggerEnter(Collider other)
    {
        // Ignores other colliders unless it is player
        if (other.tag == "Player")
        {
            // Note to self: splash direction is ignored
            var o = (GameObject)Instantiate(splashPrefab, other.transform.position, Quaternion.Euler(-90, 0, 0));
            Destroy(o, splashLifetime);

            other.SendMessage("GameOver");
        }
    }

}
