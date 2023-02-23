/*********************************************************************************************
* Project: Simulation
* File   : GameEvents
* Date   : 25.05.2022
* Author : Marcel Klein
*
* Used for controlling mesh generation via trigger zones
* 
* History:
*    25.05.2022    MK    Created
*********************************************************************************************/

using System;
using UnityEngine;


public class GameEvents : MonoBehaviour
{
    #region Variables

    public static GameEvents instance;

    public event Action OnMeshGeneratorTriggerEnter;
    public event Action OnPCGNoiseEnter;
    public event Action OnPCGSimpleEnter;
    public event Action OnProceduralMeshStartEnter;
    public event Action OnProceduralMeshStopEnter;
    public event Action OnNightStart;
    public event Action OnDayStart;

    public bool isDay = false;
    public bool isNight = true;

    #endregion

    #region Methods

    void Awake()
    {
        instance = this;
    }

    //Triggerzone events for mesh generation
    public void MeshGeneratorTriggerEnter()
    {
        if (OnMeshGeneratorTriggerEnter != null)
        {
            OnMeshGeneratorTriggerEnter();
        }
    
    }
    public void PCGNoiseEnter()
    {
        if (OnPCGNoiseEnter != null)
        {
            OnPCGNoiseEnter();
        }

    }
    public void PCGSimpleEnter()
    {
        if (OnPCGSimpleEnter != null)
        {
            OnPCGSimpleEnter();
        }

    }
    public void ProceduralMeshStartEnter()
    {
        if (OnProceduralMeshStartEnter != null)
        {
            OnProceduralMeshStartEnter();
        }

    }
    public void ProceduralMeshStopEnter()
    {
        if (OnProceduralMeshStopEnter != null)
        {
            OnProceduralMeshStopEnter();
        }

    }

    //Day Night Cycle events for light management
    //public void NightStart()
    //{
    //    if (OnNightStart != null)
    //    {
    //        OnNightStart();
    //    }

    //}
    //public void DayStart()
    //{
    //    if (OnDayStart != null)
    //    {
    //        OnDayStart();
    //    }

    //}

    #endregion
}
