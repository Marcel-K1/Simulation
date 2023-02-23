/*********************************************************************************************
* Project: Simulation
* File   : Chunk
* Date   : 25.05.2022
* Author : Marcel Klein
*
* Initializes each chunk with the calculated values in the MapGenerator script 
* 
* History:
*    25.05.2022    MK    Created
*********************************************************************************************/


using UnityEngine;


public class Chunk : MonoBehaviour
{
    #region Variables

    private MeshRenderer meshRenderer = null;
    private MeshFilter meshFilter = null;
    private Mesh mesh = null;
    private MeshCollider meshCollider = null;
    private Vector3[] meshPositions = null;
    private int[] triIdxs = null;
    private bool isGenerated = false;

    #endregion

    #region Methods

    //Initializing each chunk with the given values from the MapGenerator
    public void Init(Vector3 _meshPos, Vector3[] _meshPositions, int[] _triIdxs, Material _mat)
    {
        this.transform.localPosition = _meshPos;

        meshFilter = this.gameObject.AddComponent<MeshFilter>();
        meshFilter.sharedMesh = new Mesh();
        mesh = meshFilter.sharedMesh;
        meshRenderer = this.gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = _mat;

        mesh.name = $"ChunkMesh_{_meshPos.x}|{_meshPos.y}|{_meshPos.z}";

        meshPositions = _meshPositions;
        triIdxs = _triIdxs;
        isGenerated = false;
    }

    //Methods to control the rendering of visible chunks of the mesh according to the viewers position
    public void Show()
    {
        if (!isGenerated)
        {
            mesh.Clear();
            mesh.vertices = meshPositions;
            mesh.triangles = triIdxs;
            mesh.RecalculateNormals();
            isGenerated = true;
            meshCollider = this.gameObject.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = mesh;
        }
        else
            meshRenderer.enabled = true;
    }
    public void Hide()
    {
        meshRenderer.enabled = false;
    }

    #endregion
}
