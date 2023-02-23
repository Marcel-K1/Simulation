/*******************************************************************************
* Project: Simulation
* File   : AISensor
* Date   : 29.05.2022
* Author : Marcel Klein
*
* Creates a sensor for AI agents to check for sightability of enemies.
* 
* History:
*    29.05.2022    MK    Created
*******************************************************************************/


using System.Collections.Generic;
using UnityEngine;


public class AISensor : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private List<Invader> targets;
    [SerializeField]
    Collider[] colliders = new Collider[40];

    private float distance = 10;
    private float angle = 30;
    private float height = 1.0f;
    private Color meshColor = Color.red;
    private int scanFrequency = 30;
    private LayerMask layers;
    private LayerMask occlusionLayers;
    private Mesh mesh = null;
    private float scanInterval = 0f;
    private float scanTimer = 0f;

    #endregion

    #region Properties

    public List<Invader> Targets 
    { 
        get 
        {
            targets.RemoveAll(obj => !obj);
            return targets; 
        } 
    }
    public float Distance => distance;
    public float Angle => angle;
    public float Height => height;
    public Color MeshColor => meshColor;
    public int ScanFrequency => scanFrequency;
    public LayerMask Layers => layers;
    public LayerMask OcclusionLayers => occlusionLayers;
    public Mesh Mesh { get => mesh; set => mesh = value; }

    #endregion

    #region Methods

    void Start()
    {
        targets = new List<Invader>();
        scanInterval = 1.0f / scanFrequency;
    }
    void Update()
    {
        scanTimer -= Time.deltaTime;
        if (scanTimer < 0)
        {
            scanTimer += scanInterval;
            Scan();
        }
    }

    //Scans for colliders in range of defender radius
    private void Scan()
    {
        //count = Physics.OverlapSphereNonAlloc(transform.position, distance, colliders, layers, QueryTriggerInteraction.Collide);
        colliders = Physics.OverlapSphere(transform.position, distance, layers);
        targets.Clear();
        for (int i = 0; i < colliders.Length; ++i)
        {
            //Debug.Log($"colider {i}");
            if (colliders[i].gameObject.GetComponent<Invader>())
            {
                //Debug.Log($"collider {i} has Invader");
                Invader obj = colliders[i].GetComponent<Invader>();
                if (IsInSight(obj))
                {
                    //Debug.Log($"collider {i} is in Sight");
                    targets.Add(obj);
                }
            }
        }
    }

    //Checks if colliders of type invader aren't blocked by walls or out of sight
    public bool IsInSight(Invader obj)
    {
        Vector3 origin = transform.position;
        Vector3 dest = obj.transform.position;
        Vector3 direction = dest - origin;
        //if (direction.y < 0 || direction.y > height)
        //{
        //    return false;
        //}

        direction.y = 0;
        float deltaAngle = Vector3.Angle(direction, transform.forward);
        if (deltaAngle > angle)
        {
            return false;
        }

        origin.y += height / 2;
        dest.y = origin.y;
        if (Physics.Linecast(origin, dest, occlusionLayers))
        {
            return false;
        }

        return true;
    }

    //Creates an optical representation of the sightradius
    public Mesh CreateWedgeMesh()
    {
        Mesh mesh = new Mesh();

        int segments = 10;
        int numTriangles = (segments * 4) + 2 + 2;
        int numVertices = numTriangles * 3;

        Vector3[] vertices = new Vector3 [numVertices];
        int[] triangles = new int [numVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;

        int vert = 0;

        //Left Side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;

        //Rigth Side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segments;
        for (int i = 0; i < segments; ++i)
        {
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;

            topRight = bottomRight + Vector3.up * height;
            topLeft = bottomLeft + Vector3.up * height;

            currentAngle += deltaAngle;

            //Far Side
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;

            //Top
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;

            //Bottom
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;
        }

        for (int i = 0; i < numVertices; i++)
        {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }
    private void OnValidate()
    {
        Mesh = CreateWedgeMesh();
        scanInterval = 1.0f / scanFrequency;
    }
    private void OnDrawGizmos()
    {
        if (Mesh)
        {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(Mesh, transform.position, transform.rotation);
        }

        //Gizmos.DrawWireSphere(transform.position, distance);

        //Gizmos.color = Color.white;
        //for (int i = 0; i < count; ++i)
        //{
        //    Gizmos.DrawSphere(colliders[i].transform.position + new Vector3(0, 5, 0), 0.5f);
        //}

        Gizmos.color = Color.red;
        foreach (var obj in Targets)
        {
            Gizmos.DrawSphere(obj.transform.position + new Vector3 (0,5,0), 0.5f);
        }
    }

    #endregion
}
