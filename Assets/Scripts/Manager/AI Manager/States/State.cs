/*********************************************************************************************
* Project: Simulation
* File   : State
* Date   : 25.05.2022
* Author : Marcel Klein
*
* Base class state, making it possible to compare current states and setting next states in the different state machines
* 
* History:
*    25.05.2022    MK    Created
*********************************************************************************************/


public abstract class State
{
    #region Attributes

    protected Entity entity = null;

    #endregion

    #region Constructors

    public State(Entity entity)
    {
        this.entity = entity;
    }

    #endregion

    #region Methods

    public abstract void Execute();

    #endregion
}
