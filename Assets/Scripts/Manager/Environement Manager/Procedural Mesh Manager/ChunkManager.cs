/*********************************************************************************************
* Project: Simulation
* File   : ChunkManager
* Date   : 25.05.2022
* Author : Marcel Klein
*
* Tells which chunk needs to be shown according to the viewer position
* 
* History:
*    25.05.2022    MK    Created
*********************************************************************************************/


using System.Collections.Generic;
using UnityEngine;


public class ChunkManager : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private MapGenerator mGenerator = null;
    [SerializeField]
    private Transform mViewer = null;
    [SerializeField]
    private Transform mStartPos = null;
    [SerializeField]
    private Material mChunkMat = null;
    [SerializeField]
    private bool mStartPCG = false;
    [SerializeField]
    private int mChunkViewDist = 0;
    [SerializeField]
    private float mChunkSize = 0f;
    [SerializeField, Range(2, 256)]
    private int mChunkResolution = 2;

    private Dictionary<Vector2Int, Chunk> allChunks = null;
    private List<Vector2Int> currLoadedChunks = null;

    #endregion

    #region Properties
    public float maxViewDist => mChunkSize * mChunkViewDist;
    public Vector2 ViewerPos => new Vector2(mViewer.position.x, mViewer.position.z);

    #endregion

    #region Methods
    private void Awake()
    {
        allChunks = new Dictionary<Vector2Int, Chunk>();
        currLoadedChunks = new List<Vector2Int>();
        GameEvents.instance.OnProceduralMeshStartEnter += OnPCGStarted;
        GameEvents.instance.OnProceduralMeshStopEnter += OnPCGStopped;
    }

    //Triggering the Procedural Mesh Generation
    private void Update()
    {
        if (mStartPCG)
        {
            UpdateChunks();
        }
        else
        {
            //Delete Dictionary???
        }
        
    }
    private void OnPCGStarted()
    {
        mStartPCG = true;
        //Debug.Log("PCG Started");
    }
    private void OnPCGStopped()
    {
        mStartPCG = false;
        //Debug.Log("PCG Stopped");
    }

    //Tells which chunk needs to be shown according to viewer position
    private void UpdateChunks()
    {
        Vector2Int viewerChunkCoord = new Vector2Int(
            Mathf.RoundToInt(ViewerPos.x / mChunkSize),
            Mathf.RoundToInt(ViewerPos.y / mChunkSize));

        List<Vector2Int> newLoadedChunks = new List<Vector2Int>();

        for (int y = -mChunkViewDist; y <= mChunkViewDist; y++)
        {
            for (int x = -mChunkViewDist; x <= mChunkViewDist; x++)
            {
                //Added mViewer.position.y to set chunks at y offset when player isn't on position.y = 0
                Vector2Int currChunkCoord = viewerChunkCoord + new Vector2Int(x, y);
                Vector3 currChunkWorldPos = new Vector3(currChunkCoord.x, 0, currChunkCoord.y) * mChunkSize;
                currChunkWorldPos.y = mStartPos.position.y;

                //Prohibts Update to generate new chunk each frame, by checking the chunkdictionary.
                if (!currLoadedChunks.Contains(currChunkCoord))
                {
                    if (allChunks.ContainsKey(currChunkCoord))
                        allChunks[currChunkCoord].Show();
                    else
                    {
                        Chunk newChunk = mGenerator.GenerateNewChunk(currChunkWorldPos, mChunkSize, mChunkResolution, mChunkMat, this.transform);
                        newChunk.Show();

                        allChunks.Add(currChunkCoord, newChunk);
                    }
                }
                else
                {
                    currLoadedChunks.Remove(currChunkCoord);
                }

                newLoadedChunks.Add(currChunkCoord);
            }
        }

        foreach (Vector2Int chunkCoord in currLoadedChunks)
        {
            allChunks[chunkCoord].Hide();
        }

        currLoadedChunks = newLoadedChunks;
    }

    //Used for Visualization of the mesh grid
    //private void OnDrawGizmosSelected()
    //{
    //    if (allChunks != null)
    //    {
    //        foreach (KeyValuePair<Vector2Int, Chunk> chunkPair in allChunks)
    //        {
    //            Chunk chunk = chunkPair.Value;

    //            Gizmos.color = Color.green;
    //            Gizmos.DrawWireCube(chunk.transform.position, new Vector3(mChunkSize, 0.1f, mChunkSize));
    //        }
    //    }
    //}

    #endregion
}
