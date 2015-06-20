using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenericGridObjectGeneratorScript : MonoBehaviour {

    /// <summary>
    /// The minimum X, Y, Z position of objects to be generated.
    /// </summary>
    public Vector3 minPosition;

    /// <summary>
    /// The maximum X, Y, Z position of objects to be generated.
    /// </summary>
    public Vector3 maxPosition;

    /// <summary>
    /// The grid size used as steps for iterating the coordinates.
    /// </summary>
    public Vector3 gridSize = new Vector3(1, 1, 1);

    /// <summary>
    /// The probability of objects to be generated in a grid.
    /// </summary>
    public float density = 0.5f;

    /// <summary>
    /// If true, the position of generated objects is relative to this object.
    /// </summary>
    public bool relative = true;

    /// <summary>
    /// If true, the objects generated will also be destroyed when this object is destroyed.
    /// </summary>
    public bool destroyWhenDestroyed = true;

    /// <summary>
    /// The object prefabs to be generated.
    /// </summary>
    public GameObject[] prefabs;

    private List<GameObject> generatedObjects;

    public void Start()
    {
        generatedObjects = new List<GameObject>();

	    for (var x = minPosition.x; x <= maxPosition.x; x += gridSize.x)
        for (var y = minPosition.y; y <= maxPosition.y; y += gridSize.y)
        for (var z = minPosition.z; z <= maxPosition.z; z += gridSize.z)
        {
            bool generate = Random.value < density;
            if (generate)
            {
                GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
                var o = (GameObject)Instantiate(
                    prefab,
                    relative ? transform.position + new Vector3(x, y, z) : new Vector3(x, y, z),
                    Quaternion.identity
                );

                generatedObjects.Add(o);
                OnInstantiate(o);
            }
        }
	}

    void OnDestroy()
    {
        if (destroyWhenDestroyed)
        {
            foreach (var o in generatedObjects)
            {
                Destroy(o);
            }
        }
    }

    /// <summary>
    /// Override this to perform actions to the object just generated.
    /// </summary>
    /// <param name="o">The object generated.</param>
    protected virtual void OnInstantiate(GameObject o) { }
}
