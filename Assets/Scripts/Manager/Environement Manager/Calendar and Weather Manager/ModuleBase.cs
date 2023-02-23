/*********************************************************************************************
* Project: Simulation
* File   : ModuleBase
* Date   : 25.05.2022
* Author : Marcel Klein
*
* Base class for modules for the environment manager to further customize the time, calender and weather system
* 
* History:
*    25.05.2022    MK    Created
*********************************************************************************************/


using UnityEngine;


public abstract class ModuleBase : MonoBehaviour
{
    #region Variables

    protected EnvironmentManager dayNightControl = null;

    #endregion

    #region Methods
    private void OnEnable()
    {
        dayNightControl = this.GetComponent<EnvironmentManager>();
        if (dayNightControl != null)
            dayNightControl.AddModule(this);
    }
    private void OnDisable()
    {
        if (dayNightControl != null)
            dayNightControl.RemoveModule(this);
    }
    public abstract void UpdateModule(float intensity);

    #endregion
}
