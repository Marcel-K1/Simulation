/*********************************************************************************************
* Project: Simulation
* File   : TerrainGenerator
* Date   : 25.05.2022
* Author : Marcel Klein
*
* Builds a precalculated terrain
* 
* History:
*    25.05.2022    MK    Created
*********************************************************************************************/


using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private int width = 128;
    [SerializeField]
    private int height = 128;
    [SerializeField]
    private int depth = 20;
    [SerializeField]
    private float offsetX = 100f;
    [SerializeField]
    private float offsetY = 100f;
    [SerializeField]
    private Material terrainMaterial = null;
    [SerializeField]
    private float scale = 20f;
    #endregion

    #region Methods
    void Start()
    {
        offsetX = Random.Range(0f, 9999f);
        offsetY = Random.Range(0f, 9999f);
    }
    void Update()
    {
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
        terrain.materialTemplate = terrainMaterial;
        Terrain top = terrain.topNeighbor;
        Terrain right = terrain.rightNeighbor;
        Terrain left = terrain.leftNeighbor;
        Terrain bottom = terrain.bottomNeighbor;
        terrain.SetNeighbors(left, top, right, bottom);
        terrain.allowAutoConnect = true;    
        //terrain.terrainData.heightmapTexture.
    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;   
        terrainData.size = new Vector3(width, depth, height);

        terrainData.SetHeights(0, 0, GenerateHeights());

        return terrainData;

    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                heights[x, y] = CalculateHeight(x, y);
            }
        }

        return heights;
    }

    float CalculateHeight( int x, int y)
    {
        float xCoord = (float)x / width * scale + offsetX;
        float yCoord = (float)y / height * scale + offsetY;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }
    #endregion
}
