using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    public GameObject pickupPrefab; // Assign your pickup prefab in the Inspector
    public int numberOfPickups = 10; // Number of pickups to spawn
    public Vector3 playAreaMin = new Vector3(-100, 0, -100); // Minimum bounds of the play area
    public Vector3 playAreaMax = new Vector3(100, 0, 100); // Maximum bounds of the play area
    public LayerMask obstacleLayer; // Layer for obstacles
    public float pickupRadius = 1f; // Radius to check for obstacles around the pickup

    private List<Vector3> validSpawnPositions = new List<Vector3>();

    void Start()
    {
        // OnDrawGizmos();
        GenerateValidSpawnPositions();
        SpawnPickups();
    }

    void GenerateValidSpawnPositions()
    {

        validSpawnPositions.Clear();


        for (float x = playAreaMin.x; x <= playAreaMax.x; x += 1f)
        {
            for (float z = playAreaMin.z; z <= playAreaMax.z; z += 1f)
            {
                Vector3 position = new Vector3(x, playAreaMin.y, z);


                if (!Physics.CheckSphere(position, pickupRadius, obstacleLayer))
                {
                    validSpawnPositions.Add(position);
                }
            }
        }


        if (validSpawnPositions.Count < numberOfPickups)
        {
            Debug.LogWarning("Not enough valid spawn positions for all pickups! Adjust play area or reduce number of pickups.");
        }
    }

    void SpawnPickups()
    {
        if (validSpawnPositions.Count < numberOfPickups)
        {
            Debug.LogError("Not enough valid spawn positions for pickups!");
            return;
        }

        for (int i = 0; i < numberOfPickups; i++)
        {

            int randomIndex = Random.Range(0, validSpawnPositions.Count);
            Vector3 spawnPosition = validSpawnPositions[randomIndex];
            validSpawnPositions.RemoveAt(randomIndex); // Remove to avoid duplicates


            Instantiate(pickupPrefab, spawnPosition, Quaternion.identity);
        }
    }
    
    // void OnDrawGizmos()
    // {
    //     // Set the color of the Gizmo (e.g., green)
    //     Gizmos.color = Color.green;
    //
    //     // Calculate the center and size of the play area
    //     Vector3 center = (playAreaMax + playAreaMin) / 2;
    //     Vector3 size = playAreaMax - playAreaMin;
    //
    //     // Draw a wireframe cube to represent the play area
    //     Gizmos.DrawWireCube(center, size);
    // }
}