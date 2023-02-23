/*********************************************************************************************
* Project: Simulation
* File   : SetDestinationState
* Date   : 25.05.2022
* Author : Marcel Klein
*
* Deriving class from base class state, setting up the execute method for the state machine class to evaluate
* 
* History:
*    25.05.2022    MK    Created
*********************************************************************************************/


using UnityEngine;

public class SetDestinationState : State
{
    #region Constructors
    public SetDestinationState(Entity entity) : base(entity) { }
    #endregion

    #region Methods
    public override void Execute()
    {
        if (entity != null)
        {
            if (entity is Invader)
            {
                //Used for walking to a defined destination
                if (entity.DefinedDestination != null)
                {
                    entity.Agent.SetDestination(entity.DefinedDestination.position);        
                }
                else
                {
                    if (entity.Agent != null)
                    {
                        entity.Agent.SetDestination(entity.RandomDestinationOnNavMesh);
                    }
                }
            }
            else if (entity is Defender)
            {
                //Used for walking to a random destination
                entity.Agent.SetDestination(entity.RandomDestinationOnNavMesh);
            }
        }

        ////Used for walking along a random direction
        //Vector2 randomDirection = Random.insideUnitCircle;
        //entity.MovementDirection = new Vector3(randomDirection.x, 0, randomDirection.y);
        //entity.CurrentWalkTime = 0f;
        //entity.MaxWalkTime = Random.Range(5f, 15f);
    }
    #endregion
}
