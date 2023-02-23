/*******************************************************************************
* Project: Simulation
* File   : Invader
* Date   : 29.05.2022
* Author : Marcel Klein
*
* Derived class for making entity to type invader
* 
* History:
*    29.05.2022    MK    Created
*******************************************************************************/


using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Invader:Entity
{
    #region Variables

    [SerializeField]
    private BoidSettings invaderBoidSettings = null;
    [SerializeField]
    private float fleeingSpeed = 20.0f;
    [SerializeField]
    private float maxSpeed = 15;
    [SerializeField]
    private Vector3 distanceToDestination = Vector3.zero;
    [SerializeField]
    private float destinationRange = 10.0f;

    private List<Invader> neighbours = null;
    private Vector3 currentVelocity = Vector3.zero, desiredVelocity = Vector3.zero;
    private float damageAmount = 10f;

    #endregion

    #region Properties

    public float FleeingSpeed => fleeingSpeed;
    public float DamageAmount { get => damageAmount; set => damageAmount = value; }
    public Vector3 DistanceToDestination { get => distanceToDestination; set => distanceToDestination = value; }

    #endregion

    #region Methods

    private void Start()
    {
        agent =GetComponent<NavMeshAgent>();
        meshRenderer = GetComponent<MeshRenderer>();
        neighbours = new List<Invader>();
        DefinedDestination = GameObject.Find("Destination").transform;
        agent.SetDestination(DefinedDestination.position);    
    }
    private void Update()
    {
        if (this.DefinedDestination == null)
        {
            AIManager.Instance.UnregisterEntity(this);
            Destroy(this.gameObject);
        }

        if (this.DefinedDestination!= null)
        {
            distanceToDestination = this.DefinedDestination.position - this.transform.position;
        }

        //Checking for destination on NavMesh
        if (RandomPoint(transform.position, destinationRange, out Vector3 point))
        {
            RandomDestinationOnNavMesh = point;
        }
        else //Invert destination
        {
            RandomDestinationOnNavMesh = -point;
        }


        Alignment();
        Cohesion();
        Separation();

        //Moving agent according to boid settings
        if (neighbours.Count != 0)
        {
            currentVelocity += desiredVelocity - currentVelocity;
            desiredVelocity = Vector3.zero;

            currentVelocity -= transform.position - DefinedDestination.position;
        }
        currentVelocity = Vector3.ClampMagnitude(currentVelocity, maxSpeed);
        agent.Move(currentVelocity * Time.deltaTime);

        //Check for reaching the destination
        if (DistanceToDestination.magnitude < 1.25f)
        {
            AIManager.Instance.UnregisterEntity(this);
            Destroy(gameObject);
        }
    }

    //Checking for random point on NavMesh and give it out
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 20.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = this.transform.position;
        return false;
    }

    //Boid methods controlling swarm behaviour
    private void Alignment()
    {
        if (neighbours.Count == 0) return;
        Vector3 alignment = Vector3.zero;

        for (int i = 0; i < neighbours.Count; i++)
        {
            alignment += neighbours[i].currentVelocity;
        }

        alignment /= neighbours.Count;

        desiredVelocity += alignment.normalized * maxSpeed * invaderBoidSettings.Alignment;
    }
    private void Cohesion()
    {
        if (neighbours.Count == 0) return;
        Vector3 center = Vector3.zero;

        for (int i = 0; i < neighbours.Count; i++)
        {
            center += neighbours[i].transform.position;
        }

        center /= neighbours.Count;

        desiredVelocity += (center - transform.position).normalized * maxSpeed * invaderBoidSettings.Cohesion;
    }
    private void Separation()
    {
        if (neighbours.Count == 0) return;
        Vector3 direction = Vector3.zero;
        Vector3 distance;

        for (int i = 0; i < neighbours.Count; i++)
        {
            distance = transform.position - neighbours[i].transform.position;
            direction += distance / distance.sqrMagnitude;
        }

        direction /= neighbours.Count;
        desiredVelocity += direction.normalized * maxSpeed * invaderBoidSettings.Separation;
    }

    //Adding boids to list
   private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Invader")
        {
            Invader boid = other.GetComponent<Invader>();
            if (boid != null)
                neighbours.Add(boid);

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Invader")
        {
            Invader boid = other.GetComponent<Invader>();
            if (boid != null)
                neighbours.Remove(boid);
        }
    }

    #endregion
}
