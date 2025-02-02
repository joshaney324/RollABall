using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    public GameObject pickupPrefab; // Assign your pickup prefab in the Inspector
    public int numberOfPickups = 10; // Number of pickups to spawn
    public Vector3 playAreaMin = new Vector3(-10, 0, -10); // Minimum bounds of the play area
    public Vector3 playAreaMax = new Vector3(10, 0, 10); // Maximum bounds of the play area
    public LayerMask obstacleLayer; // Layer for obstacles
    public float pickupRadius = 0.5f; // Radius to check for obstacles around the pickup

    private List<Vector3> validSpawnPositions = new List<Vector3>();

    void Start()
    {
        GenerateValidSpawnPositions();
        SpawnPickups();
    }

    void GenerateValidSpawnPositions()
    {
        // Clear the list of valid positions
        validSpawnPositions.Clear();

        // Iterate through the play area to find valid positions
        for (float x = playAreaMin.x; x <= playAreaMax.x; x += 1f)
        {
            for (float z = playAreaMin.z; z <= playAreaMax.z; z += 1f)
            {
                Vector3 position = new Vector3(x, playAreaMin.y, z);

                // Check if the position is free of obstacles using a 3D overlap sphere
                if (!Physics.CheckSphere(position, pickupRadius, obstacleLayer))
                {
                    validSpawnPositions.Add(position);
                }
            }
        }

        // Log a warning if there aren't enough valid positions
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
            // Randomly select a position from the valid list
            int randomIndex = Random.Range(0, validSpawnPositions.Count);
            Vector3 spawnPosition = validSpawnPositions[randomIndex];
            validSpawnPositions.RemoveAt(randomIndex); // Remove to avoid duplicates

            // Instantiate the pickup at the selected position
            Instantiate(pickupPrefab, spawnPosition, Quaternion.identity);
        }
    }
}