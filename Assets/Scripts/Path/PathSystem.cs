using UnityEngine;

public class PathSystem : MonoBehaviour
{
    public Terrain terrain;
    public GameObject player;
    public GameObject spherePrefab;
    public GameObject pointsParent;

    // Distance between the spheres/Points
    [Range(1f, 10f)]
    public float pointSpacing;

    // Have a range of the Z axis since we are going in x axis
    [Range(0f, 50f)]
    public float minZaxis, maxZaxis;

    private PathManager pathManager; // Instance of the PathManager
    public PathManager GetPathManager() {  return pathManager; }

    private void Awake()
    {
        // Initialize PathManager with the prefab and player
        pathManager = new PathManager(spherePrefab, terrain, pointsParent, pointSpacing);
        //pathManager = new PathManager(player.transform);

        // Generate first 2 points
        GenerateNextSphere();
        GeneratePreviousSphere();

        pathManager.GeneratePath();

        Debug.Log("Path System has : " + pathManager.GetPaths().Count);
    }
    // Have the Next and Previous Sphere GUI Button
    public void GenerateNextSphere()
    {
        // When you press Create Next it check if there was no Next sphere created before and if it's true it makes a new red 3D sphere that a player can move in that path
        pathManager.GenerateNextSphere(minZaxis, maxZaxis);
    }
    public void GeneratePreviousSphere()
    {
        pathManager.GeneratePreviousSphere(minZaxis, maxZaxis);
    }

}
