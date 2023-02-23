/*********************************************************************************************
* Project: Simulation
* File   : MapGenerator
* Date   : 25.05.2022
* Author : Marcel Klein
*
* Actually building the chunks by calculating the vertices and triangles of the mesh
* 
* History:
*    25.05.2022    MK    Created
*********************************************************************************************/


using UnityEngine;


public class MapGenerator : MonoBehaviour, IMeshGenerator
{
    private enum EGenerationMode
    {
        simple,
        noise
    }

    #region Variables

    [SerializeField]
    private EGenerationMode currGenMode = EGenerationMode.simple;
    [SerializeField]
    private Vector2 mNoiseOffset = Vector2.zero;
    [SerializeField]
    private float mNoiseFrequency = 0f;
    [SerializeField]
    private float mNoiseStrength = 0f;

    #endregion

    #region Methods
    //Triggering Procedural Mesh Generation
    void Start()
    {
        GameEvents.instance.OnPCGNoiseEnter += OnNoiseTriggered;
        GameEvents.instance.OnPCGSimpleEnter += OnSimpleTriggered;
    }
    private void OnNoiseTriggered()
    {
        currGenMode = EGenerationMode.noise;
        Debug.Log("Noise PCG Mode activated");
    }
    private void OnSimpleTriggered()
    {
        currGenMode = EGenerationMode.simple;
        Debug.Log("Simple PCG Mode activated");
    }

    //Methods for actually building the chunks in simple or noise mode
    public Chunk GenerateNewChunk(Vector3 _rootPos, float _chunkSize, int _chunkRes, Material _chunkMat, Transform _parent)
    {
        switch (currGenMode)
        {
            case EGenerationMode.simple:
                return GenerateSimpleChunk(_rootPos, _chunkSize, _chunkRes, _chunkMat, _parent);

            case EGenerationMode.noise:
                return GenerateNoiseChunk(_rootPos, _chunkSize, _chunkRes, _chunkMat, _parent);
        }

        return GenerateSimpleChunk(_rootPos, _chunkSize, _chunkRes, _chunkMat, _parent);
    }
    private Chunk GenerateSimpleChunk(Vector3 _rootPos, float _chunkSize, int _chunkRes, Material _chunkMat, Transform _parent)
    {
        Vector3[] verts = new Vector3[_chunkRes * _chunkRes];
        int[] tris = new int[(_chunkRes - 1) * (_chunkRes - 1) * 2 * 3];

        Vector3 startPos = new Vector3(_chunkSize, 0, _chunkSize) * -0.5f;

        int triIdx = 0;
        for (int y = 0, vertIdx = 0; y < _chunkRes; y++)
        {
            for (int x = 0; x < _chunkRes; x++, vertIdx++)
            {
                Vector2 percent = new Vector2(x, y) / (_chunkRes - 1);
                Vector3 vertPos = startPos + new Vector3(percent.x, 0, percent.y) * _chunkSize;

                verts[vertIdx] = vertPos;

                if (x < _chunkRes - 1 && y < _chunkRes - 1)
                {
                    tris[triIdx + 0] = vertIdx;
                    tris[triIdx + 1] = vertIdx + _chunkRes + 1;
                    tris[triIdx + 2] = vertIdx + 1;

                    tris[triIdx + 3] = vertIdx;
                    tris[triIdx + 4] = vertIdx + _chunkRes;
                    tris[triIdx + 5] = vertIdx + _chunkRes + 1;

                    triIdx += 6;
                }
            }
        }

        GameObject newChunkObj = new GameObject($"Chunk_{_rootPos.x}|{_rootPos.y}|{_rootPos.z}");
        newChunkObj.transform.SetParent(_parent);

        Chunk newChunk = newChunkObj.AddComponent<Chunk>();
        newChunk.Init(_rootPos, verts, tris, _chunkMat);

        return newChunk;
    }
    private Chunk GenerateNoiseChunk(Vector3 _rootPos, float _chunkSize, int _chunkRes, Material _chunkMat, Transform _parent)
    {
        Vector3[] verts = new Vector3[_chunkRes * _chunkRes];
        int[] tris = new int[(_chunkRes - 1) * (_chunkRes - 1) * 2 * 3];

        Vector3 startPos = new Vector3(_chunkSize, 0, _chunkSize) * -0.5f;

        int triIdx = 0;
        for (int y = 0, vertIdx = 0; y < _chunkRes; y++)
        {
            for (int x = 0; x < _chunkRes; x++, vertIdx++)
            {
                Vector2 percent = new Vector2(x, y) / (_chunkRes - 1);
                Vector3 vertPos = startPos + new Vector3(percent.x, 0, percent.y) * _chunkSize;

                //Noise pls
                Vector2 noisePos = new Vector2(vertPos.x + _rootPos.x, vertPos.z + _rootPos.z) * mNoiseFrequency + mNoiseOffset;
                float noiseValue = Mathf.PerlinNoise(noisePos.x, noisePos.y);
                vertPos += Vector3.up * noiseValue * mNoiseStrength;

                verts[vertIdx] = vertPos;

                if (x < _chunkRes - 1 && y < _chunkRes - 1)
                {
                    tris[triIdx + 0] = vertIdx;
                    tris[triIdx + 1] = vertIdx + _chunkRes + 1;
                    tris[triIdx + 2] = vertIdx + 1;

                    tris[triIdx + 3] = vertIdx;
                    tris[triIdx + 4] = vertIdx + _chunkRes;
                    tris[triIdx + 5] = vertIdx + _chunkRes + 1;

                    triIdx += 6;
                }
            }
        }

        GameObject newChunkObj = new GameObject($"Chunk_{_rootPos.x}|{_rootPos.y}|{_rootPos.z}");
        newChunkObj.transform.SetParent(_parent);

        Chunk newChunk = newChunkObj.AddComponent<Chunk>();
        newChunk.Init(_rootPos, verts, tris, _chunkMat);

        return newChunk;
    }

    #endregion
}
