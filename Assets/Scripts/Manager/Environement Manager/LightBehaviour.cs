/*********************************************************************************************
* Project: Simulation
* File   : LightBehaviour
* Date   : 25.05.2022
* Author : Marcel Klein
*
* Manages Lights according to Day Night Cycle
* 
* History:
*    25.05.2022    MK    Created
*********************************************************************************************/


using UnityEngine;


public class LightBehaviour : MonoBehaviour
{
    #region Variables

    //private Light pointLight = null;
    [SerializeField]
    private EnvironmentManager environmentManager;

    #endregion

    #region Methods

   void Start()
    {
        //pointLight = GetComponent<Light>();
        //GameEvents.instance.OnNightStart += OnNightStart;
        //GameEvents.instance.OnDayStart += OnDayStart;
    }

    //private void OnNightStart()
    //{
    //    if (!pointLight.enabled)
    //    {
    //        pointLight.enabled = true;
    //    }
    //}

    //private void OnDayStart()
    //{
    //    if (pointLight.enabled)
    //    {
    //        pointLight.enabled = false;
    //    }
    //}

    private void Update()
    {
        if (environmentManager.isDay)
        {
            gameObject.GetComponent<Light>().intensity = 0f;
        }
        else if (environmentManager.isNight)
        {
            gameObject.GetComponent<Light>().intensity = 10f;
        }
    }
    #endregion
}
