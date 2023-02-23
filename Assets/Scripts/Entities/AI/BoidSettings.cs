/*******************************************************************************
* Project: Simulation
* File   : BoidSettings
* Date   : 29.05.2022
* Author : Marcel Klein
*
* Settings for the swarm behaviour of the AI agents
* 
* History:
*    29.05.2022    MK    Created
*******************************************************************************/


using UnityEngine;


[CreateAssetMenu(menuName ="BoidSettings")]
public class BoidSettings : ScriptableObject
{
    #region Variables
    [SerializeField]
    float separation = 0, alignment = 0, cohesion = 0;
    #endregion

    #region Properties
    public float Separation => separation;
    public float Alignment => alignment;
    public float Cohesion => cohesion;
    #endregion
}
