/*******************************************************************************
* Project: Simulation
* File   : Destination
* Date   : 29.05.2022
* Author : Marcel Klein
*
* Gives the destination for the AI agents of type invader spezialized features like health 
* and sets it as a spawnpoint for the AI agents of type defender
* 
* History:
*    29.05.2022    MK    Created
*******************************************************************************/


using UnityEngine;


public class Destination : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private Transform destinationPosition = null;
    [SerializeField]
    private Defender defender = null;
    [SerializeField]
    private float currentHealth = 0f;
    [SerializeField]
    private float maxHealth = 500.0f;

    private float nextSpawnTime = 0f;
    #endregion

    #region Properties
    public float CurrentHealth { get => currentHealth; set => currentHealth = value; }
    #endregion

    #region Methods

    void Start()
    {
        //Start off with next spawn time being 'in 5 seconds'
        nextSpawnTime = Time.time + 5.0f;
        currentHealth = maxHealth;
    }
    void Update()
    {
        //Instantiate Defenders after 5 seconds
        if (this.gameObject != null)
        {
            if (Time.time > nextSpawnTime)
            {
                ++AIManager.Instance.DefenderNumber;
                var go = Instantiate(defender, destinationPosition.position, Quaternion.identity);
                go.name = $"Defender Number {AIManager.Instance.DefenderNumber}";
                go.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0,1,0), ForceMode.Impulse);
                go.GetComponent<AISensor>().Mesh = go.GetComponent<AISensor>().CreateWedgeMesh();
                AIManager.Instance.Entities.Add(go);
                AIManager.Instance.CreateStateMachine(go);

                //Increment nextSpawnTime for interval
                nextSpawnTime += 5.0f;
            }
        }

        //Healthcheck
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }

    }

    //Checks for collision from invader AI agents
    private void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.tag == "Invader")
        {
            currentHealth = currentHealth - other.gameObject.GetComponent<Invader>().DamageAmount;
        }
    }

    #endregion
}
