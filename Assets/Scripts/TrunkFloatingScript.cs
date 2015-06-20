using UnityEngine;
using System.Collections;

public class TrunkFloatingScript : MonoBehaviour {

    // ==================================================================
    // TODO Make generic Sinerp() -- or use iTween or Animator instead :v
    // ==================================================================

    /// <summary>
    /// The X-speed of floating trunk, in units per second.
    /// </summary>
    public float speedX = 0.0f;

    /// <summary>
    /// Time for sinking animation, in seconds.
    /// </summary>
    public float animationTime = 0.1f;

    /// <summary>
    /// Distance of the trunk sinking, in units.
    /// </summary>
    public float animationDistance = 0.1f;

    /// <summary>
    /// The water splash prefab to be instantiated.
    /// </summary>
    public GameObject splashPrefab;

    private float originalY;
    private bool sinking;
    private float elapsedTime;
    private Rigidbody playerBody;

    public void Start()
    {
        originalY = transform.position.y;
    }

    public void Update()
    {
        transform.position += new Vector3(speedX * Time.deltaTime, 0.0f, 0.0f);

        elapsedTime += Time.deltaTime;
        if (elapsedTime > animationTime)
        {
            sinking = false;
            transform.position = new Vector3(transform.position.x, originalY, transform.position.z);
        }

        if (sinking)
        {
            float y = Sinerp(originalY, originalY - animationDistance, (elapsedTime < animationTime) ? (elapsedTime / animationTime) : 1);
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerBody = collision.gameObject.GetComponent<Rigidbody>();

            if (!sinking)
            {
                var o = (GameObject)Instantiate(splashPrefab, transform.position, Quaternion.Euler(-90, 0, 0));
                o.transform.localScale = transform.localScale;

                sinking = true;
                elapsedTime = 0.0f;
            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerBody.position += new Vector3(speedX * Time.deltaTime, 0.0f, 0.0f);
        }
    }

    private float Sinerp(float min, float max, float weight)
    {
        return min + (max - min) * Mathf.Sin(weight * Mathf.PI);
    }

}
