/*********************************************************************************************
* Project: Simulation
* File   : DefenderStateMachine
* Date   : 25.05.2022
* Author : Marcel Klein
*
* Deriving class from base class StateMachine for setting up the processing of different states for entity type of defender
* 
* History:
*    25.05.2022   MK    Created
*********************************************************************************************/

public class DefenderStateMachine : StateMachine
{
    #region Constructors
    public DefenderStateMachine(Defender agent) : base(agent, new SetDestinationState(agent)) { }
    #endregion

    #region Methods
    protected override void SwitchState()
    {
        Defender defender = (Defender)entity;

        //Walking Loop
        if (currentState is SetDestinationState)
            currentState = new WalkingState(entity);
        else if (currentState is WalkingState && entity.Agent.remainingDistance <= 1.25f)
            currentState = new SetDestinationState(entity);
        else if (currentState is WalkingState && defender.Target != null)
            currentState = new HuntingState(entity);

        //Hunting Loop
        else if (currentState is HuntingState && defender.Target != null && (defender.Target.position - entity.transform.position).sqrMagnitude <= 1.25f )
            currentState = new KillingState(defender, defender.Target.GetComponent<Invader>());
        else if (currentState is HuntingState && defender.Target != null && (defender.Target.position - entity.transform.position).sqrMagnitude >= entity.AttentionRadius * entity.AttentionRadius )
            currentState = new SetDestinationState(entity);
        //else if (currentState is HuntingState && ((defender.Target == null) && (defender.Targets.Count == 0)))
        //    currentState = new SetDestinationState(entity);

        //Killing Loop
        else if (currentState is KillingState && ((defender.Target == null) || (defender.Targets.Count == 0)))
            currentState = new SetDestinationState(entity);

         this.entity.State = currentState.ToString();
    }
    #endregion
}
