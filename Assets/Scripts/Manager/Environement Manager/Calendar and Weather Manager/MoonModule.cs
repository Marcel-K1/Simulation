/*********************************************************************************************
* Project: Simulation
* File   : MoonModule
* Date   : 25.05.2022
* Author : Marcel Klein
*
* Deriving class from ModuleBase to add moon feature to the environment manager
* 
* History:
*    25.05.2022    MK    Created
*********************************************************************************************/


using UnityEngine;


public class MoonModule : ModuleBase
{
    #region Variables

    [SerializeField]
    private Light moon = null;
    [SerializeField]
    private Gradient moonColor = null;
    [SerializeField]
    private float baseIntensity = 0f;
    [SerializeField]
    private GameObject moonTexture = null;
    [SerializeField]
    private EnvironmentManager dayNightCycle = null;

    #endregion

    #region Methods

    public override void UpdateModule(float intensity)
    {
        moon.color = moonColor.Evaluate(1 - intensity);
        moon.intensity = (1 - intensity) * baseIntensity + 0.05f;

        if (dayNightCycle.TimeOfDay > 0.75)
        {
            moonTexture.SetActive(true);
        }
        else if (dayNightCycle.TimeOfDay > 0.25)
        {
            moonTexture.SetActive (false);
        }
    }

    #endregion
}
