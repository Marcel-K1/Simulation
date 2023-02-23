/*********************************************************************************************
* Project: Simulation
* File   : FleeingState
* Date   : 25.05.2022
* Author : Marcel Klein
*
* Deriving class from base class state, setting up the execute method for the state machine class to evaluate
* 
* History:
*    25.05.2022   MK    Created
*********************************************************************************************/


using UnityEngine;


public class FleeingState : State
{
    #region Attributes
    private Transform defender = null;
    #endregion

    #region Constructors
    public FleeingState(Invader invader, Transform defender):base(invader)
    {
        this.defender = defender;
    }
    #endregion

    #region Methods
    public override void Execute()
    {
        if (defender != null)
        {
            entity.Agent.destination = entity.transform.position + (entity.transform.position - defender.position).normalized;
            entity.Agent.speed = ((Invader)entity).FleeingSpeed;
            entity.Renderer.material.color = Color.green;
        }
        else
        {
            return;
        }
    }
    #endregion

}
