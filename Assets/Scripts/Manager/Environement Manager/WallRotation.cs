/*********************************************************************************************
* Project: Simulation
* File   : WallRotation
* Date   : 25.05.2022
* Author : Marcel Klein
*
* Used for rotating level props
* 
* History:
*    25.05.2022    MK    Created
*********************************************************************************************/


using UnityEngine;


public class WallRotation : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 0.5f;

    private void Update()
    {
        transform.Rotate(new Vector3(0, 45, 0) * rotationSpeed *  Time.deltaTime);
    }
}
