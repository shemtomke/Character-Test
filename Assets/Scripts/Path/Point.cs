using UnityEngine;

public class Point : MonoBehaviour
{
    Terrain terrain;

    private void Start()
    {
        terrain = FindFirstObjectByType<Terrain>();
    }
    private void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(transform.position);
        Vector3 newPos = Vector3.zero;

        if (terrain.GetComponent<TerrainCollider>().Raycast(ray, out hit, Mathf.Infinity))
        {
            newPos.y = Terrain.activeTerrain.SampleHeight(transform.position);
        }

        Vector3 pos = new Vector3(transform.position.x, newPos.y, transform.position.z);
        transform.position = pos;
    }
}
