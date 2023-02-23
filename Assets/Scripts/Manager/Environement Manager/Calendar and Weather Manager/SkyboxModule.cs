/*********************************************************************************************
* Project: Simulation
* File   : SkyboxModule
* Date   : 25.05.2022
* Author : Marcel Klein
*
* Deriving class from ModuleBase to add a customizable skybox to the environment manager
* 
* History:
*    25.05.2022    MK    Created
*********************************************************************************************/


using UnityEngine;


public class SkyboxModule : ModuleBase
{
    #region Variables

    [SerializeField]
    private Gradient skyColor = null;
    [SerializeField]
    private Gradient horizonColor = null;

    #endregion

    #region Methods
    public override void UpdateModule(float intensity)
    {
        RenderSettings.skybox.SetColor("_SkyTint", skyColor.Evaluate(intensity));
        RenderSettings.skybox.SetColor("_GroundColor", horizonColor.Evaluate(intensity));
    }

    #endregion
}
