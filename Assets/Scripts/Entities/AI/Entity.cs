/*******************************************************************************
* Project: Simulation
* File   : Entity
* Date   : 29.05.2022
* Author : Marcel Klein
*
* Base class for all AI agents, setting up properties for the deriving classes
* 
* History:
*    29.05.2022    MK    Created
*******************************************************************************/


using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent), typeof(MeshRenderer))]
public abstract class Entity : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private Transform definedDestination = null;
    [SerializeField]
    private float walkingSpeed = 10f;
	[SerializeField]
    private float attentionRadius;
    [SerializeField]
    private LayerMask mask;
    [SerializeField]
    private string state;

    protected NavMeshAgent agent = null;
    protected MeshRenderer meshRenderer = null;


    private Vector3 movementDirection = Vector3.zero;
    private Vector3 randomDestinationOnNavMesh = Vector3.zero;
    private bool isDying = false;
	
    //Used for walking along a random direction as long as currentWalkTime < maxWalkTime
    //[SerializeField]
    //private float currentWalkTime, maxWalkTime;
    #endregion

    #region Properties
    public MeshRenderer Renderer => meshRenderer;
    public NavMeshAgent Agent => agent;
    public LayerMask Mask => mask;
    public float WalkingSpeed => walkingSpeed;
    public Vector3 MovementDirection { get => movementDirection; set => movementDirection = value; }
    public Vector3 RandomDestinationOnNavMesh { get => randomDestinationOnNavMesh; set => randomDestinationOnNavMesh = value;}
    public Transform DefinedDestination { get => definedDestination; set => definedDestination = value; }
    public float AttentionRadius { get => attentionRadius; }
    public string State { get => state; set => state = value; }
    public bool IsDying { get => isDying; set => isDying = value; }

    //Used for walking along a random direction as long as currentWalkTime < maxWalkTime
    //public float CurrentWalkTime { get => currentWalkTime; set => currentWalkTime = value; }
    //public float MaxWalkTime { get => maxWalkTime; set => maxWalkTime = value; }
    #endregion

}
