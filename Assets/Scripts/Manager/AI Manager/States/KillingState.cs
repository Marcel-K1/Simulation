/*********************************************************************************************
* Project: Simulation
* File   : KillingState
* Date   : 25.05.2022
* Author : Marcel Klein
*
* Deriving class from base class state, setting up the execute method for the state machine class to evaluate
* 
* History:
*    25.05.2022    MK    Created
*********************************************************************************************/


using UnityEngine;

public class KillingState : State
{
    #region Attributes
    private Entity invader;
    #endregion

    #region Constructors
    public KillingState(Defender defender, Entity invader):base(defender)
    {
        this.invader = invader;
    }
    #endregion

    #region Methods
    public override void Execute()
    {
        //AIManager.Instance.UnregisterEntity(invader);
        if (invader!= null)
        {
            AIManager.Instance.UnregisterEntity(invader);
            GameObject.Destroy(invader.gameObject);
        }
        ((Defender)entity).Saturation = 10f;
    }
    #endregion

}
