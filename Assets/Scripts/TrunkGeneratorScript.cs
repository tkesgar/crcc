using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrunkGeneratorScript : MonoBehaviour {

    public enum Direction { Left = -1, Right = 1 };

    /// <summary>
    /// If true, the direction, speed, length, and interval will be randomized.
    /// </summary>
    public bool randomizeValues = false;

    /// <summary>
    /// The direction of trunk.
    /// </summary>
    public Direction direction;

    /// <summary>
    /// The speed of trunk, in units per second.
    /// </summary>
    public float speed = 2.0f;

    /// <summary>
    /// The length of trunk, in units.
    /// </summary>
    public float length = 2.0f;

    /// <summary>
    /// The interval time between trunk generation, in seconds.
    /// Note that this value should be greater than length / speed to avoid colliding trunks.
    /// </summary>
    public float interval = 2.0f;

    public float leftX = -20.0f;

    public float rightX = 20.0f;

    public GameObject trunkPrefab;

    private float elapsedTime;

    private List<GameObject> trunks;

    public void Start()
    {
	    if (randomizeValues)
        {
            direction = Random.value < 0.5f ? Direction.Left : Direction.Right;
            speed = Random.Range(2.0f, 4.0f);
            length = Random.Range(1, 4);
            interval = length / speed + Random.Range(2.0f, 4.0f);
        }

        elapsedTime = 0.0f;
        trunks = new List<GameObject>();

        // TODO Use some sort of warmup to avoid "empty river"
	}
	
    public void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > interval)
        {
            elapsedTime = 0.0f;

            var position = transform.position + new Vector3(direction == Direction.Left ? rightX : leftX, 0, 0);
            var o = (GameObject)Instantiate(trunkPrefab, position, Quaternion.identity);
            o.GetComponent<TrunkFloatingScript>().speedX = (int)direction * speed;

            var scale = o.transform.localScale;
            o.transform.localScale = new Vector3(scale.x * length, scale.y, scale.z);

            trunks.Add(o);
        }

        foreach (var o in trunks.ToArray())
        {
            if (direction == Direction.Left && o.transform.position.x < leftX ||
                direction == Direction.Right && o.transform.position.x > rightX)
            {
                Destroy(o);
                trunks.Remove(o);
            }
        }
	}

    void OnDestroy()
    {
        foreach (var o in trunks)
        {
            Destroy(o);
        }
    }
}
