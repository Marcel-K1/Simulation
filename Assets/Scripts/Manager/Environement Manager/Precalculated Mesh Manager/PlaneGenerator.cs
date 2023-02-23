/*********************************************************************************************
* Project: Simulation
* File   : PlaneGenerator
* Date   : 25.05.2022
* Author : Marcel Klein
*
* Builds a mesh in depandency from the main mesh to create a water plane
* 
* History:
*    25.05.2022    MK    Created
*********************************************************************************************/


using UnityEngine;


public class PlaneGenerator : MonoBehaviour
{
    private MeshRenderer meshRen;
    private MeshFilter meshFilter;
    //private MeshCollider meshCollider;
    private Mesh mesh;
    private float size = 0;
    private int resolution = 0;
    private float waterPlaneOffset = 0f;

    private Vector3[] vertices = null;
    private int[] triangles = null;
    private Vector2[] uvs = null;

    [SerializeField]
    private Material waterMaterial = null;
    [SerializeField]
    private MeshGenerator meshGenerator = null;

    private void Awake()
    {
        meshRen = this.gameObject.AddComponent<MeshRenderer>();
        meshFilter = this.gameObject.AddComponent<MeshFilter>();
        //meshCollider = this.gameObject.AddComponent<MeshCollider>();
        mesh = new Mesh();
        mesh.name = "WaterPlane";
        meshFilter.sharedMesh = mesh;
        size = meshGenerator.Size;
        resolution = meshGenerator.Resolution;
        waterPlaneOffset = meshGenerator.WaterPlaneOffset;
    }

    public void GenerateMesh()
    {
        Vector3 meshStartPos = (new Vector3(size, 0, size) / 2f) * -1;

        vertices = new Vector3[resolution * resolution];
        triangles = new int[(resolution - 1) * (resolution - 1) * 2 * 3];

        int triIdx = 0;

        for (int y = 0, i = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++, i++)
            {
                Vector2 percent = new Vector2(x, y) / (resolution - 1);

                Vector3 planePos = meshStartPos + Vector3.right * percent.x * size + Vector3.forward * percent.y * size;

                vertices[i] = new Vector3(planePos.x, planePos.y + waterPlaneOffset, planePos.z);

                if (x != resolution - 1 && y != resolution - 1)
                {
                    //Berechne ein Quad
                    //Triangle 1
                    triangles[triIdx + 0] = i;
                    triangles[triIdx + 1] = i + resolution + 1;
                    triangles[triIdx + 2] = i + 1;

                    //Triangle 2
                    triangles[triIdx + 3] = i;
                    triangles[triIdx + 4] = i + resolution;
                    triangles[triIdx + 5] = i + resolution + 1;

                    triIdx += 6;
                }
            }
        }

        uvs = new Vector2[vertices.Length];

        for (int y = 0, i = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++, i++)
            {
                uvs[i] = new Vector2((float)x / resolution, (float)y / resolution);
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        meshFilter.mesh = mesh;
        mesh.RecalculateNormals();

        meshRen.material = waterMaterial;
        //meshCollider.sharedMesh = mesh;

    }

}
