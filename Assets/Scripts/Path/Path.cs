using System.Collections.Generic;
using UnityEngine;

public class Path
{
    // Two paths generate a path to allow the player to follow
    private Vector3 startPoint;
    private Vector3 endPoint;
    private int pathResolution = 10; // Number of paths on the path
    Terrain terrain;

    // Generate a path based on two paths
    public Path(Vector3 start, Vector3 end, int resolution = 10)
    {
        startPoint = start;
        endPoint = end;
        pathResolution = Mathf.Max(2, resolution);
        terrain = Terrain.activeTerrain;
    }
    public List<Vector3> GeneratePath()
    {
        List<Vector3> pathPoints = new List<Vector3>();

        // Add the start point explicitly, adjusting for terrain height
        Vector3 startPos = new Vector3(startPoint.x, terrain.SampleHeight(startPoint), startPoint.z);
        pathPoints.Add(startPos);

        // Generate the intermediate points along the path, adjusting for terrain height
        for (int i = 1; i < pathResolution - 1; i++) // Exclude the first and last points
        {
            float t = (float)i / (pathResolution - 1); // Interpolation factor (0 to 1)
            Vector3 interpolatedPoint = Vector3.Lerp(startPoint, endPoint, t);

            // Adjust the y-coordinate based on the terrain height
            float terrainHeight = terrain.SampleHeight(interpolatedPoint);
            pathPoints.Add(new Vector3(interpolatedPoint.x, terrainHeight, interpolatedPoint.z));
        }

        // Add the end point explicitly, adjusting for terrain height
        Vector3 endPos = new Vector3(endPoint.x, terrain.SampleHeight(endPoint), endPoint.z);
        pathPoints.Add(endPos);

        return pathPoints;
    }
    public void DrawPath(Color color)
    {
        List<Vector3> pathPoints = GeneratePath();
        for (int i = 0; i < pathPoints.Count - 1; i++)
        {
            Debug.DrawLine(pathPoints[i], pathPoints[i + 1], color, 50f);
        }
    }
}