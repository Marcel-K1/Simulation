/*********************************************************************************************
* Project: Simulation
* File   : PrecalculatedMeshGenerator
* Date   : 25.05.2022
* Author : Marcel Klein
*
* Builds a precalculated mesh according to the settings in this script 
* 
* History:
*    25.05.2022    MK    Created
*********************************************************************************************/


using UnityEngine;


[System.Serializable]
public class TerrainType
{
    public string name;
    public float height;
    //public Color color;
}

//[ExecuteAlways]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PrecalculatedMeshGenerator : MonoBehaviour
{
    #region Variables

    [SerializeField] 
    private TerrainType[] terrainTypes;
    [SerializeField]
    private float grassMinBorder = 0.33f, grassMaxBorder = 0.66f;
    [SerializeField] 
    private Material terrainMaterial = null;
    [SerializeField] 
    private GameObject grassObject = null;
    //[SerializeField]
    //private Transform startPosition = null;
    [SerializeField]
    private AnimationCurve heightCurve = null;
    [SerializeField]
    private int xSize = 100;
    [SerializeField]
    private int ySize = 100;
    [SerializeField]
    private float maxheight = 32;
    [SerializeField]
    private float PerlinScale = 64;

    #endregion

    #region Methods

    //Triggering the Precalculated Mesh Generation
    //void Start()
    //{
    //    GameEvents.instance.onMeshGeneratorTriggerEnter += OnMeshGeneratorTriggered;
    //}
    //private void OnMeshGeneratorTriggered()
    //{
    //    Generator();
    //}

    //private void Update()
    //{
    //    Generator();
    //}

    //Generating the Mesh accordingly
    public float[,] GenerateNoiseMap(int zGrid, int xGrid, float scale)
    {
        float[,] noise = new float[zGrid, xGrid];
        for (int zIndex = 0; zIndex < zGrid; zIndex++)
        {
            for (int xIndex = 0; xIndex < xGrid; xIndex++)
            {
                float sampleX = xIndex / scale;
                float sampleZ = zIndex / scale;

                noise[zIndex, xIndex] = Mathf.PerlinNoise(sampleX, sampleZ);
            }
        }
        return noise;
    }
    private TerrainType ChooseTerrainType(float height)
    {
        foreach (TerrainType terType in terrainTypes)
        {
            if (height < terType.height)
                return terType;
        }
        return terrainTypes[terrainTypes.Length - 1];
    }
    private void Generator()
    {
        Vector3[] vertexbuffer = new Vector3[(xSize + 1) * (ySize + 1)];
        int[] indexbuffer = new int[xSize * ySize * 6];
        Vector2[] uvbuffer = new Vector2[(xSize + 1) * (ySize + 1)];

        float[,] heightmap = GenerateNoiseMap(ySize + 1, xSize + 1, PerlinScale);
        //Texture2D heightTex = BuildTexture(heightmap);

        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                vertexbuffer[i] = new Vector3(x, heightCurve.Evaluate(heightmap[x, y]) * maxheight, y);
                //Debug.Log(vertexbuffer[i]);
                uvbuffer[i] = new Vector2((float)y / (float)ySize, (float)x / (float)xSize);
                //Debug.Log("i:" + i + " - " + uvbuffer[i]);
            }
        }

        for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                indexbuffer[ti] = vi;
                indexbuffer[ti + 3] = indexbuffer[ti + 2] = vi + 1;
                indexbuffer[ti + 4] = indexbuffer[ti + 1] = vi + xSize + 1;
                indexbuffer[ti + 5] = vi + xSize + 2;
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertexbuffer;
        mesh.triangles = indexbuffer;
        mesh.uv = uvbuffer;
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = mesh;
        //GetComponent<MeshRenderer>().material.mainTexture = heightTex;
        GetComponent<MeshRenderer>().material = terrainMaterial;
        GetComponent<MeshCollider>().sharedMesh = mesh;

        int depth = heightmap.GetLength(0);
        int width = heightmap.GetLength(1);
        var currentPos = new Vector3();
        //for (int x = 0; x < depth; x+=10)
        //{
        //    for (int z = 0; z < width; z+=10)
        //    {
        //        if (heightmap[x,z] >= grassMinBorder && heightmap[x,z] <= grassMaxBorder)
        //        {
        //            currentPos = new Vector3(transform.position.x + x, transform.position.y + heightmap[x, z], transform.position.z + z);
        //            Instantiate(grassObject, currentPos, Quaternion.identity);
        //        }
               
        //    }
        //}
        //renderer.material.mainTexture = heightTex;

    }

    //Used for creating a colormap out of the different heightvalues of the mesh
    //private Texture2D BuildTexture(float[,] heightmap)
    //{
    //    int depth = heightmap.GetLength(0);
    //    int width = heightmap.GetLength(1);

    //    Color[] colorMap = new Color[depth * width];
    //    //Material[] materials = new Material[depth * width];
    //    for (int zIndex = 0; zIndex < depth; zIndex++)
    //    {
    //        for (int xIndex = 0; xIndex < width; xIndex++)
    //        {
    //            int colorIndex = zIndex * width + xIndex;
    //            //int materialIndex = zIndex * width + xIndex;
    //            float height = heightmap[zIndex, xIndex];
    //            TerrainType terrType = ChooseTerrainType(height);
    //            colorMap[colorIndex] = Color.Lerp(Color.black, Color.white, height);
    //            colorMap[colorIndex] = terrType.color;
    //            //materials[materialIndex] = terrType.material;
    //        }
    //    }

    //Texture2D tex = new Texture2D(width, depth);
    //tex.wrapMode = TextureWrapMode.Clamp;
    //tex.SetPixels(colorMap);
    //tex.Apply();
    //return tex;
    //}

    //Used for showing the vertices on the mesh
    //private void OnDrawGizmos()
    //{

    //    Gizmos.color = Color.black;

    //    for (int i = 0; i < GetComponent<MeshFilter>().sharedMesh.vertexCount; i++)
    //        Gizmos.DrawSphere(GetComponent<MeshFilter>().sharedMesh.vertices[i], 0.1f);
    //}

    #endregion
}

