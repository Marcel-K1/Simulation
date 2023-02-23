/*********************************************************************************************
* Project: Simulation
* File   : HuntingState
* Date   : 25.05.2022
* Author : Marcel Klein
*
* Deriving class from base class state, setting up the execute method for the state machine class to evaluate
* 
* History:
*    25.05.2022    MK    Created
*********************************************************************************************/


using UnityEngine;


public class HuntingState : State
{
    #region Constructors
    public HuntingState(Entity entity) : base(entity) { }
    #endregion

    #region Methods
    public override void Execute()
    {
        if (((Defender)entity).Target != null)
        {
            entity.Agent.destination = entity.transform.position + (((Defender)entity).Target.position - entity.transform.position).normalized;
            entity.Agent.speed = ((Defender)entity).ChaseSpeed;
            entity.Renderer.material.color = Color.red;
        }
        else
        {
            return;
        }
    }
    #endregion
}
