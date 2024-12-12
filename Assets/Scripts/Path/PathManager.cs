using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PathManager
{
    private GameObject spherePrefab; // Prefab for spheres
    private Terrain terrain;
    private GameObject pointsParent;
    private float sphereSpacing; // Distance between spheres may vary (Random)

    private List<GameObject> spheres; // List to track generated spheres
    private List<Vector3> paths = new List<Vector3>();

    // Constructor to initialize with prefab and spacing
    public PathManager(GameObject prefab, Terrain terrainObj, GameObject pointsParentObj, float spacing)
    {
        spherePrefab = prefab;
        terrain = terrainObj;
        sphereSpacing = spacing;
        pointsParent = pointsParentObj;
        spheres = new List<GameObject>();
        paths = new List<Vector3>();
    }
    // Get All Paths
    public List<Vector3> GetPaths() { return paths; }
    // Get the list of all spheres
    public List<GameObject> GetSpheres() { return spheres; }
    // Get the last generated sphere
    public GameObject GetLastSphere()
    {
        if (spheres.Count > 0)
            return spheres[spheres.Count - 1];
        return null;
    }
    // Get the first generated sphere
    public GameObject GetFirstSphere()
    {
        if (spheres.Count > 0)
            return spheres[0];
        return null;
    }
    // Generate the next sphere
    public void GenerateNextSphere(float minZaxis, float maxZaxis)
    {
        float z = Random.Range(-minZaxis, maxZaxis);
        float x = 0f; // Default X-axis position

        // Determine the X-axis based on the last sphere
        if (spheres.Count > 0)
        {
            GameObject lastSphere = spheres[spheres.Count - 1];
            Vector3 lastPosition = lastSphere.transform.position;

            // Add random spacing based on the previous sphere
            x = lastPosition.x + sphereSpacing;
        }

        // Instantiate the sphere directly at the desired position
        Vector3 initialPosition = new Vector3(x, 0, z); // Use the intended position
        GameObject newSphere = Object.Instantiate(spherePrefab, initialPosition, Quaternion.identity);
        newSphere.transform.SetParent(pointsParent.transform, false);

        // Add the sphere to the list
        spheres.Add(newSphere);

        if (spheres.Count > 2) { GeneratePath(); }
    }
    // Generate the previous sphere
    public void GeneratePreviousSphere(float minZaxis, float maxZaxis)
    {
        if (spheres.Count == 0)
        {
            Debug.LogWarning("No spheres available to generate previous!");
            return;
        }

        // Get the position of the first sphere
        GameObject firstSphere = spheres[0];
        Vector3 firstPosition = firstSphere.transform.position;

        // Calculate new position for the previous sphere
        float z = Random.Range(-minZaxis, maxZaxis);
        float x = firstPosition.x - sphereSpacing;

        Vector3 newPosition = new Vector3(x, 0f, z);

        GameObject previousSphere = Object.Instantiate(spherePrefab, newPosition, Quaternion.identity);
        previousSphere.transform.SetParent(pointsParent.transform, false);

        // Insert at the beginning of the list
        spheres.Insert(0, previousSphere);

        if(spheres.Count > 2) { GeneratePath(); }
    }
    // The player should move along the path - Not Outside the Path
    public void GeneratePath()
    {
        paths.Clear();

        // Use a HashSet to track unique path points
        HashSet<Vector3> uniquePoints = new HashSet<Vector3>();

        // For each pair of spheres, generate a path
        for (int i = 0; i < spheres.Count - 1; i++)
        {
            Vector3 startPosition = spheres[i].transform.position;
            Vector3 endPosition = spheres[i + 1].transform.position;

            // Create a Path object for each pair of spheres
            Path path = new Path(startPosition, endPosition);
            path.DrawPath(Color.white);

            // Generate path points and add them to the HashSet to avoid duplicates
            foreach (var point in path.GeneratePath())
            {
                // Only add points that aren't already in the set
                if (uniquePoints.Add(point))
                {
                    paths.Add(point);
                }
            }

            Debug.Log($"Paths Count: {paths.Count}");
            foreach (var _path in paths)
            {
                Debug.Log($"Path Point: {_path}");
            }
        }
    }
    // Place the player on the first point of the path
    public void SetPlayerDefaultPosition(Transform player)
    {
        if (paths.Count > 0)
        {
            player.position = paths[0];
        }
    }
    public Vector3 GetPlayerPositionOnPath(float t)
    {
        if (paths.Count == 2)
        {
            // If there are only two points, just interpolate directly between them
            return Vector3.Lerp(paths[0], paths[1], t);
        }

        // Otherwise, interpolate between the points using t to determine the current segment
        int numSections = paths.Count - 1;
        int currentSection = Mathf.FloorToInt(t * numSections);
        float sectionT = (t * numSections) - currentSection;

        // Ensure we don't go beyond the last segment
        if (currentSection >= paths.Count - 1)
        {
            return paths[paths.Count - 1];  // Return the last point if we're at or beyond the last segment
        }

        // Interpolate between the current segment's start and end points
        return Vector3.Lerp(paths[currentSection], paths[currentSection + 1], sectionT);
    }
    // If the Player is in the path -> Get the Next Point & Previous Point
    public (Vector3? previousPoint, Vector3? nextPoint) GetAdjacentPoints(Vector3 playerPosition)
    {
        for (int i = 0; i < paths.Count - 1; i++)
        {
            Vector3 startPoint = paths[i];
            Vector3 endPoint = paths[i + 1];

            // Check if player is within the segment
            if (IsBetween(playerPosition, startPoint, endPoint))
            {
                Vector3? previousPoint = i > 0 ? paths[i - 1] : (Vector3?)null;
                Vector3? nextPoint = i + 2 < paths.Count ? paths[i + 2] : (Vector3?)null;
                return (previousPoint, nextPoint);
            }
        }

        return (null, null);
    }

    // Simple helper to check if the player is between two points
    private bool IsBetween(Vector3 playerPos, Vector3 start, Vector3 end)
    {
        return Vector3.Distance(start, playerPos) + Vector3.Distance(playerPos, end) <= Vector3.Distance(start, end) + 0.01f;
    }
}
