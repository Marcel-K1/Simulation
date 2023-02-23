/*********************************************************************************************
* Project: Simulation
* File   : MeshGenerator
* Date   : 25.05.2022
* Author : Marcel Klein
*
* Builds a precalculated mesh
* 
* History:
*    25.05.2022    MK    Created
*********************************************************************************************/


using UnityEngine;


public class MeshGenerator : MonoBehaviour
{
    #region Variables
    private MeshRenderer meshRen; 
    private MeshFilter meshFilter; 
    private MeshCollider meshCollider; 
    private Mesh mesh; 

    [SerializeField, Range(100, 200)]
    private float size;
    [SerializeField, Range(2, 64)]
    private int resolution;
    [SerializeField]
    private string seed;
    private Vector2 seedNoiseOffset;
    [SerializeField]
    private Vector2 noiseCenter;
    [SerializeField, Range(0.05f, 1)]
    private float noiseScale = 0.05f;
    [SerializeField, Range(0, 20)]
    private float noiseStrength = 15f;
    [SerializeField]
    private Material terrainMaterial = null; 
    [SerializeField]
    private Material meshMaterial = null;
    [SerializeField]
    private Gradient colorGradient = null;
    [SerializeField]
    private GameObject waterPlane = null;
    [SerializeField]
    private GameObject grassObject = null;
    [SerializeField]
    private PlaneGenerator waterPlaneGenerator = null;
    [SerializeField, Range(0.5f, 10f)]
    private float waterPlaneOffset = 5f;
    private Vector3[] vertices = null;
    private int[] triangles = null;
    private Vector2[] uvs = null;
    private Vector3 meshStartPos = Vector3.zero;
    private Vector3 planePos = Vector3.zero;
    #endregion

    #region Properties

    public Vector3  MeshStartPos => meshStartPos;
    public Vector3 PlanePos => planePos;
    public float Size { get => size; set => size = value; }
    public int Resolution { get => resolution; set => resolution = value; }
    public float WaterPlaneOffset { get => waterPlaneOffset; set => waterPlaneOffset = value; }

    #endregion

    #region Methods
    private void Awake()
    {
        meshRen = this.gameObject.AddComponent<MeshRenderer>();
        meshFilter = this.gameObject.AddComponent<MeshFilter>();
        meshCollider = this.gameObject.AddComponent<MeshCollider>();       
        mesh = new Mesh();
        mesh.name = "CustomPlane";
        meshFilter.sharedMesh = mesh;

        int seed;
        if (!string.IsNullOrEmpty(this.seed) && !string.IsNullOrWhiteSpace(this.seed))
            seed = this.seed.GetHashCode();
        else
            seed = (int)System.DateTime.Now.Ticks;

        seed %= 256;

        seedNoiseOffset = new Vector2(seed, seed);
    }

    void Start()
    {
        GameEvents.instance.OnMeshGeneratorTriggerEnter += OnMeshGeneratorTriggered;
        //GenerateMesh();
        //waterPlaneGenerator.GenerateMesh();
    }
    private void OnMeshGeneratorTriggered()
    {
        GenerateMesh();
        waterPlaneGenerator.GenerateMesh();
    }

    private void GenerateMesh()
    {
        Vector3 meshStartPos = (new Vector3(Size, 0, Size) / 2f) * -1;

        vertices = new Vector3[Resolution * Resolution];
        triangles = new int[(Resolution - 1) * (Resolution - 1) * 2 * 3];


        int triIdx = 0;

        for (int y = 0, i = 0; y < Resolution; y++)
        {
            for (int x = 0; x < Resolution; x++, i++)
            {
                Vector2 percent = new Vector2(x, y) / (Resolution - 1);

                Vector3 planePos = meshStartPos + Vector3.right * percent.x * Size + Vector3.forward * percent.y * Size;

                Vector2 noiseValuePosition = noiseCenter + seedNoiseOffset + new Vector2(planePos.x, planePos.z) * noiseScale;
                Vector3 noisePos = planePos + Vector3.up * Mathf.PerlinNoise(noiseValuePosition.x, noiseValuePosition.y) * noiseStrength;

                vertices[i] = noisePos;

                //For placing grass objects on mesh
                //foreach (Vector3 vertice in vertices)
                //{
                //    if (i % 2 == 0)
                //    {
                //        Instantiate(grassObject, new Vector3(vertices[i].x + this.transform.position.x, vertices[i].y + this.transform.position.y, vertices[i].z + this.transform.position.z), Quaternion.identity);
                //    }
                //}

                if (x != Resolution - 1 && y != Resolution - 1)
                {
                    //Berechne ein Quad
                    //Triangle 1
                    triangles[triIdx + 0] = i;
                    triangles[triIdx + 1] = i + Resolution + 1;
                    triangles[triIdx + 2] = i + 1;

                    //Triangle 2
                    triangles[triIdx + 3] = i;
                    triangles[triIdx + 4] = i + Resolution;
                    triangles[triIdx + 5] = i + Resolution + 1;

                    triIdx += 6;
                }
            }
        }

        uvs = new Vector2[vertices.Length];
        //Color[] colors = new Color[verts.Length];

        for (int y = 0, i = 0; y < Resolution; y++)
        {
            for (int x = 0; x < Resolution; x++, i++)
            {
                uvs[i] = new Vector2((float)x / Resolution, (float)y / Resolution);
                //float height = verts[i].y;
                //colors[i] = colorGradient.Evaluate(height);
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        //mesh.colors = colors;
        meshFilter.mesh = mesh;
        mesh.RecalculateNormals();
        meshRen.material = terrainMaterial;
        meshCollider.sharedMesh = mesh;

    }
    #endregion
}
