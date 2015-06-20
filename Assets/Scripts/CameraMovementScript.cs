using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerMovementScript))]
public class CameraMovementScript : MonoBehaviour
{
    /// <summary>
    /// Minimum Z-position of player the camera is allowed to track.
    /// </summary>
    public float minZ = 0.0f;

    /// <summary>
    /// Speed of Z-increment.
    /// </summary>
    public float speedIncrementZ = 1.0f;

    /// <summary>
    /// Speed of camera offset when player is moving.
    /// </summary>
    public float speedOffsetZ = 4.0f;

    /// <summary>
    /// If true, the camera offset will move.
    /// </summary>
    public bool moving = false;

    private GameObject player;
    private PlayerMovementScript playerMovement;

    private Vector3 offset;
    private Vector3 initialOffset;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovementScript>();

        // TODO this position and rotation is baked, extract it
        initialOffset = new Vector3(2.5f, 10.0f, -7.5f);
        offset = initialOffset;
	}
	
	public void Update() {

        if (moving)
        {
            Vector3 playerPosition = player.transform.position;
            transform.position = new Vector3(playerPosition.x, 0, Mathf.Max(minZ, playerPosition.z)) + offset;

            // Increase z over time if moving.
            offset.z += speedIncrementZ * Time.deltaTime;

            // Increase/decrease z when player is moving south/north.
            if (playerMovement.IsMoving)
            {
                if (playerMovement.MoveDirection == "north")
                {
                    offset.z -= speedOffsetZ * Time.deltaTime;
                }
            }
        }
	}

    public void Reset()
    {
        // TODO This kind of reset is dirty, refactor might be needed.
        moving = false;
        offset = initialOffset;
        transform.position = new Vector3(2.5f, 10.0f, -7.5f);
    }
}
