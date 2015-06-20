using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoadCarGenerator : MonoBehaviour {

    // ===================================================
    // TODO Genericize RoadCarGenerator and TrunkGenerator
    // ===================================================

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
    /// The interval time between car generation, in seconds.
    /// Note that this value should be greater than length / speed to avoid colliding cars.
    /// </summary>
    public float interval = 2.0f;

    public float leftX = -20.0f;

    public float rightX = 20.0f;

    public GameObject[] carPrefabs;

    private float elapsedTime;

    private List<GameObject> cars;

    public void Start()
    {
        if (randomizeValues)
        {
            direction = Random.value < 0.5f ? Direction.Left : Direction.Right;
            speed = Random.Range(2.0f, 4.0f);
            interval = Random.Range(3.0f, 6.0f);
        }

        elapsedTime = 0.0f;
        cars = new List<GameObject>();
    }

    public void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > interval)
        {
            elapsedTime = 0.0f;

            // TODO extract 0.375f and -0.5f to outside -- probably along with genericization
            var position = transform.position + new Vector3(direction == Direction.Left ? rightX : leftX, 0.375f, -0.5f);
            var o = (GameObject)Instantiate(carPrefabs[Random.Range(0, carPrefabs.Length)], position, Quaternion.identity);
            o.GetComponent<CarScript>().speedX = (int)direction * speed;

            cars.Add(o);
        }

        foreach (var o in cars.ToArray())
        {
            if (direction == Direction.Left && o.transform.position.x < leftX ||
                direction == Direction.Right && o.transform.position.x > rightX)
            {
                Destroy(o);
                cars.Remove(o);
            }
        }
    }

    void OnDestroy()
    {
        foreach (var o in cars)
        {
            Destroy(o);
        }
    }
}
