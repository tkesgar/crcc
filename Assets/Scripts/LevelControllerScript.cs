using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelControllerScript : MonoBehaviour {

    /// <summary>
    /// Minimum line Z-position to be generated.
    /// </summary>
    public int minZ = 3;

    /// <summary>
    /// Number of lines should be kept ahead.
    /// </summary>
    public int lineAhead = 40;

    /// <summary>
    /// Number of lines should be kept behind.
    /// </summary>
    public int lineBehind = 20;

    /// <summary>
    /// Line prefabs for generation by controller.
    /// </summary>
    public GameObject[] linePrefabs;

    private Dictionary<int, GameObject> lines;

    private GameObject player;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        lines = new Dictionary<int, GameObject>();
	}
	
    public void Update()
    {
        // Generate lines based on player position.
        var playerZ = (int)player.transform.position.z;
        for (var z = Mathf.Max(minZ, playerZ - lineBehind); z <= playerZ + lineAhead; z++)
        {
            if (!lines.ContainsKey(z))
            {
                var line = (GameObject)Instantiate(
                    linePrefabs[Random.Range(0, linePrefabs.Length)],
                    new Vector3(0, 0, z), Quaternion.identity
                );
                lines.Add(z, line);
            }
        }

        // Remove lines based on player position.
        foreach (var line in new List<GameObject>(lines.Values))
        {
            var lineZ = line.transform.position.z;
            if (lineZ < playerZ - lineBehind)
            {
                lines.Remove((int)lineZ);
                Destroy(line);
            }
        }
	}

    public void Reset()
    {
        // TODO This kind of reset is dirty, refactor might be needed.
        if (lines != null)
        {
            foreach (var line in new List<GameObject>(lines.Values))
            {
                Destroy(line);
            }
            Start();
        }
    }
}
