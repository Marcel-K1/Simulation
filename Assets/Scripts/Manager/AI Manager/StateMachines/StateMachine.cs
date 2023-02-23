/*********************************************************************************************
* Project: Simulation
* File   : StateMachine
* Date   : 25.05.2022
* Author : Marcel Klein
*
* Base class for setting up the behaviour of all AI entities in the simulation
* 
* History:
*    25.05.2022    MK    Created
*********************************************************************************************/


public abstract class StateMachine 
{
    #region Variables

    protected Entity entity = null;
    protected State currentState = null;

    #endregion

    #region Constructors

    protected StateMachine(Entity agent, State startState)
    {
        this.entity = agent;
        currentState = startState;
        this.entity.State = currentState.ToString();
    }

    #endregion

    #region Methods
    
    //Executing current state, than switching it according to states process in deriving classes for invader and defender
    public void Evaluate()
    {
        currentState?.Execute();
        SwitchState();
    }
    protected abstract void SwitchState();

    #endregion
}
