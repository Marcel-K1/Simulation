/*********************************************************************************************
* Project: Simulation
* File   : SearchingState
* Date   : 25.05.2022
* Author : Marcel Klein
*
* Deriving class from base class state, setting up the execute method for the state machine class to evaluate
* 
* History:
*    25.05.2022    MK    Created
*********************************************************************************************/


using UnityEngine;

public class SearchingState : State
{
    #region Constructors
    public SearchingState(Entity entity): base(entity) { }
    #endregion

    #region Methods
    public override void Execute()
    {
        if (((Defender)entity).Sensor.Targets.Count > 0)
            ((Defender)entity).Target = ((Defender)entity).Sensor.Targets[0].transform;

        else
            ((Defender)entity).Target = null;
    }
    #endregion
}
