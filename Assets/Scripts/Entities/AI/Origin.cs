/*******************************************************************************
* Project: Simulation
* File   : Origin
* Date   : 29.05.2022
* Author : Marcel Klein
*
* Sets up the origin for the AI agents of type invader making it a spawnpoint for the prefabs
* 
* History:
*    29.05.2022    MK    Created
*******************************************************************************/


using UnityEngine;
using UnityEngine.AI;


public class Origin : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private Transform originPosition = null;
    [SerializeField]
    private Invader invader = null;
    [SerializeField]
    private Transform definedDestination = null;

    private float nextSpawnTime = 0f;
    #endregion

    #region Methods

    private void Start()
    {
        nextSpawnTime = Time.time + 5.0f;

    }
    void Update()
    {
        //Instantiate Invaders after a certain amount of time
        if (Time.time > nextSpawnTime)
        {
            ++AIManager.Instance.InvaderNumber;
            var go = Instantiate(invader, originPosition.position, Quaternion.identity);
            go.name = $"Invader Number {AIManager.Instance.InvaderNumber}";
            go.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 1, 0), ForceMode.Impulse);
            NavMeshAgent goAgent = go.GetComponent<NavMeshAgent>();
            if(definedDestination != null)
            {
                goAgent.destination = definedDestination.position;
            }
            AIManager.Instance.Entities.Add(go);
            AIManager.Instance.CreateStateMachine(go);
            
            nextSpawnTime += 5.0f;
        }
    }

    #endregion
}
