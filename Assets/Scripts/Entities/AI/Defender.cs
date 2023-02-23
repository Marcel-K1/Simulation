/*******************************************************************************
* Project: Simulation
* File   : Defender
* Date   : 29.05.2022
* Author : Marcel Klein
*
* Derived class for making entity to type defender
* 
* History:
*    29.05.2022    MK    Created
*******************************************************************************/


using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;


public class Defender : Entity
{
    #region Variables
    [SerializeField]
    private float chaseSpeed = 25f;

    [SerializeField]
    private float saturation = 20.0f;

    [SerializeField]
    private float health = 20.0f;

    [SerializeField]
    private float destinationRange = 20.0f;

    [SerializeField]
    private List<Invader> targets = null;

    [SerializeField]
    private Transform target = null;

    [SerializeField]
    private AISensor sensor = null;

    private bool canSeeInvader = false;
    #endregion

    #region Properties
    public float ChaseSpeed => chaseSpeed;
    public float Saturation { get => saturation; set => saturation = value; }
    public float Health { get => health; set => health = value; }
    public Transform Target { get => target; set => target = value; }
    public AISensor Sensor { get => sensor; set => sensor = value; }
    public List<Invader> Targets { get => targets; set => targets = sensor.Targets; }
    public bool CanSeeInvader { get => canSeeInvader; set => canSeeInvader = value; }


    #endregion

    #region Methods
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        meshRenderer = GetComponent<MeshRenderer>();
        sensor = GetComponent<AISensor>();
    }

    private void Update()
    {
        //Setting and checking saturation of AI agents
        Saturation -= Time.deltaTime;
        if (Saturation <= 0)
        {
            Health -= Time.deltaTime;
        }

        //Checking for destroying conditions
        if (health < 0)
        {
            //AIManager.Instance.UnregisterEntity(this);
            if (this != null)
            {
                AIManager.Instance.UnregisterEntity(this);
                Destroy(this.gameObject);
            }
        }

        //Checking for random point on NavMesh and setting it as destination for AI agents
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
        if (RandomPoint(transform.position, destinationRange, out Vector3 point))
        {
            RandomDestinationOnNavMesh = point;
        }
        else //Invert destination
        {
            RandomDestinationOnNavMesh = -point;
        }


        //Checking for invaders in sight and setting the first one in sight as Target
        //1.Way:
        if (Sensor.Targets.Count > 0)
        {
            target = Sensor.Targets[0].transform;
        }

        //2.Way:
        //colliders = Physics.OverlapSphere(transform.position, AttentionRadius, Mask.value);
        //if (colliders.Length > 0)
        //{
        //    for (int i = 0; i < colliders.Length; i++)
        //    {
        //        Vector3 sight = colliders[i].transform.position - transform.position;
        //        sight.y = 0;

        //        float dot = Vector3.Dot(sight, transform.right);
        //        float forwardLineOfSight = 10f;

        //        //Setting conditions for defenders to see invaders
        //        if (dot < forwardLineOfSight && dot > -forwardLineOfSight)
        //        {
        //            //Preparing raycast
        //            RaycastHit hit;

        //            //Checking to see if raycast hit invader
        //            if (Physics.Raycast(transform.position, (sight).normalized, out hit, forwardLineOfSight))
        //            {
        //                Debug.DrawRay(transform.position, (sight).normalized, Color.green);

        //                if (hit.collider.tag == "Invader" && hit.collider.tag != "Wall")
        //                {
        //                    Debug.DrawRay(transform.position, (sight).normalized, Color.red);

        //                    CanSeeInvader = true;
        //                    Target = colliders[i].transform;
        //                }
        //                else if (hit.collider.tag == "Invader" && hit.collider.tag == "Wall")
        //                {
        //                    CanSeeInvader = false;
        //                }
        //            }
        //        }
        //    }
        //}

        //3.Way:
        ////RaycastCheck for invaders in sight
        ////Declaring the defenders forward line of sight
        //if (Target != null)
        //{
        //    Vector3 sight = Target.transform.position - transform.position;
        //    sight.y = 0;

        //    float dot = Vector3.Dot(sight, transform.right);
        //    float forwardLineOfSight = 5f;

        //    //Setting conditions for defenders to see invaders
        //    if (dot < forwardLineOfSight && dot > -forwardLineOfSight)                
        //        {
        //        //Preparing raycast
        //        RaycastHit hit;

        //        //Checking to see if raycast hit invader
        //        if (Physics.Raycast(transform.position, (sight).normalized, out hit, forwardLineOfSight))
        //        {
        //            Debug.DrawRay(transform.position, (sight).normalized, Color.green);

        //            if (hit.collider.tag == "Invader" && hit.collider.tag != "Wall")
        //            {
        //                Debug.DrawRay(transform.position, (sight).normalized, Color.red);

        //                CanSeeInvader = true;
        //            }
        //            else if (hit.collider.tag == "Invader" && hit.collider.tag == "Wall")
        //            {   
        //                CanSeeInvader = false;
        //            }
        //        }
        //    }
        //}
        //foreach (var collider in Colliders)
        //{
        //    if (CanSeeInvaders(collider.transform))
        //    {

        //        Target = collider.transform;

        //    }
        //}

        //4.Way:
        //colliders = Physics.OverlapSphere(transform.position, AttentionRadius, Mask.value);
        //if (colliders.Length > 0)
        //{
        //        Target = Colliders[0].transform;
        //}

        ////5.Way:
        //colliders = Physics.OverlapSphere(transform.position, AttentionRadius, Mask.value);
        //for (int i = 0; i < colliders.Length; i++)
        //{
        //    if (CanSeeInvaders(colliders[i].transform) && colliders[i] != null)
        //    {

        //        Target = colliders[i].transform;

        //    }
        //    else Target = null;
        //}

        //bool CanSeeInvaders(Transform target)
        //{
        //    Vector3 sight = target.transform.position - transform.position;
        //    sight.y = 0;

        //    float dot = Vector3.Dot(sight, transform.right);
        //    float forwardLineOfSight = 10f;

        //    //Setting conditions for defenders to see invaders
        //    if (dot < forwardLineOfSight && dot > -forwardLineOfSight)
        //    {
        //        //Preparing raycast
        //        RaycastHit hit;

        //        //Checking to see if raycast hit invader
        //        if (Physics.Raycast(transform.position, (sight).normalized, out hit, forwardLineOfSight))
        //        {
        //            Debug.DrawRay(transform.position, (sight).normalized, Color.green);

        //            if (hit.collider.tag == "Invader" && hit.collider.tag != "Wall")
        //            {
        //                Debug.DrawRay(transform.position, (sight).normalized, Color.red);
        //                return true;
        //            }
        //            else if (hit.collider.tag == "Invader" && hit.collider.tag == "Wall")
        //            {
        //                return false;
        //            }
        //            else
        //                return false;
        //        }
        //        else
        //            return false;
        //    }
        //    else
        //        return false;
        //}
    }

    #endregion
}
