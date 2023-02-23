/*********************************************************************************************
* Project: Simulation
* File   : WalkingState
* Date   : 25.05.2022
* Author : Marcel Klein
*
* Deriving class from base class state, setting up the execute method for the state machine class to evaluate
* 
* History:
*    25.05.2022    MK    Created
*********************************************************************************************/


using UnityEngine;

public class WalkingState:State
{
    #region Constructors
    public WalkingState(Entity entity) : base(entity) { }
    #endregion

    #region Methods
    public override void Execute()
    {
        //Used for walking along a random direction
        //entity.Agent.destination = entity.transform.position + entity.MovementDirection;
        //entity.CurrentWalkTime += Time.deltaTime;

        entity.Agent.speed = entity.WalkingSpeed;
        if (entity is Invader)
        {
            entity.Renderer.material.color = Color.white;
        }

        if (entity is Defender)
        {
            entity.Renderer.material.color = Color.blue;
        }
    }
    #endregion
}
