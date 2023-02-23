/*********************************************************************************************
* Project: Simulation
* File   : IMeshGenerator
* Date   : 25.05.2022
* Author : Marcel Klein
*
* Interface to further customize what the MapGenerator script will be able to generate
* 
* History:
*    25.05.2022    MK    Created
*********************************************************************************************/


using UnityEngine;


public interface IMeshGenerator
{
    public Chunk GenerateNewChunk(Vector3 _rootPos, float _chunkSize, int _chunkRes, Material _chunkMat, Transform _parent);
}
