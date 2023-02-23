/*********************************************************************************************
* Project: Simulation
* File   : InvaderStateMachine
* Date   : 25.05.2022
* Author : Marcel Klein
*
* Deriving class from base class StateMachine for setting up the processing of different states for entity type of invader
* 
* History:
*    25.05.2022    MK    Created
*********************************************************************************************/


using UnityEngine;


public class InvaderStateMachine : StateMachine
{
    #region Constructors
    public InvaderStateMachine(Invader agent):base(agent,new SetDestinationState(agent)) { }
    #endregion

    #region Methods
    protected override void SwitchState()
    {
        Collider[] colliders;
        if ((colliders = Physics.OverlapSphere(entity.transform.position, entity.AttentionRadius, entity.Mask.value)).Length > 0)
        {
            if (colliders[0] != null)
            {
                currentState = new FleeingState((Invader)entity, colliders[0].transform);
            }
            else
            {
                currentState = new SetDestinationState(entity);
            }
        }
        else
        {
            if (currentState is FleeingState)
                currentState = new SetDestinationState(entity);

            else if (currentState is SetDestinationState)
                currentState = new WalkingState(entity);

            //Used for walking along a random direction for MaxWalkTime
            //else if (currentState is WalkingState && entity.CurrentWalkTime >= entity.MaxWalkTime)
            //    currentState = new SetDestinationState(entity);

            else if (currentState is WalkingState && entity.Agent.remainingDistance <= 1.25f)
            {
                currentState = new SetDestinationState(entity);
            }

            this.entity.State = currentState.ToString();
        }
    }
    #endregion
}
