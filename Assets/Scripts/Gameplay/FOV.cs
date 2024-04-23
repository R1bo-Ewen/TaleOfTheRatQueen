using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class FOV : MonoBehaviour
{
    [Header("Field Of View")]
	public float viewRadius = 10f;
	[Range(0,360)]
	public float viewAngle = 80f;
	public LayerMask targetMask;
    public LayerMask zombieMask;
	public LayerMask obstacleMask;
    
    [Header("FOV Mesh")]
    public float meshResolution = 1f;
    public int edgeResolveIterations = 6;
    public float edgeDistanceThreshold = 0.5f;
    public bool shadowByTarget = true;
    /*public MeshFilter viewMeshFilter;
    private Mesh viewMesh;*/

    [Header("Read-Only")]
    [ReadOnly]
	public Transform target;
    //[ReadOnly]
	//public Transform zombieTarget;
    
    /*void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
    }*/

    void LateUpdate() // Draw Field Of View
    {
        target = null;
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();

        for(int i = 0; i <= stepCount; i++)
        {
            float angle = 180 - transform.eulerAngles.z - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            if(i > 0)
            {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.distance - newViewCast.distance) > edgeDistanceThreshold;
                if(oldViewCast.hit != newViewCast.hit || (oldViewCast.hit == newViewCast.hit && edgeDstThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if(edge.pointA != oldViewCast.point)
                        viewPoints.Add(edge.pointA);
                    if(edge.pointB != newViewCast.point)
                        viewPoints.Add(edge.pointB);
                }
            }

            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        // Building the mesh
        /*int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;

        for(int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i+1] = transform.InverseTransformPoint(viewPoints[i]);
            
            if(i < vertexCount - 2)
            {
                triangles[i*3] = 0;
                triangles[i*3+1] = i + 1;
                triangles[i*3+2] = i + 2;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateBounds();*/
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 direction = DirFromAngle(globalAngle, true);
        float distance = viewRadius;
        Vector2 hitPoint = transform.position + direction * distance;
        Ray ray = new Ray(transform.position, transform.position + direction);
        Physics.Raycast(ray, out var hitObstacle, viewRadius, obstacleMask);
        if(hitObstacle.distance > 0)
        {
            distance = hitObstacle.distance;
            hitPoint = hitObstacle.point;
        }

        /*RaycastHit2D hitZombie = Physics2D.Raycast(transform.position, direction, distance, zombieMask);
        if(hitZombie.distance > 0)
        {
            zombieTarget = hitZombie.transform;

            if(shadowByTarget)
            {
                distance = hitZombie.distance;
                hitPoint = hitZombie.point;
            }
        }*/
        ray = new Ray(transform.position, transform.position + direction);
        Physics.Raycast(ray, out var hitTarget, viewRadius, targetMask);
        if(hitTarget.distance > 0)
        {
            target = hitTarget.transform;

            if(shadowByTarget)
            {
                distance = hitTarget.distance;
                hitPoint = hitTarget.point;
            }
        }

        Debug.DrawLine(transform.position, transform.position + direction * distance);
        return new ViewCastInfo(true, hitPoint, distance, globalAngle);
    }

    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        for(int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minViewCast.angle + maxViewCast.angle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.distance - newViewCast.distance) > edgeDistanceThreshold;
            if(newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
            {
                Debug.DrawLine(transform.position, transform.position + DirFromAngle(angle, true) * newViewCast.distance, Color.green);
                minViewCast = newViewCast;
            }
            else
            {
                Debug.DrawLine(transform.position, transform.position + DirFromAngle(angle, true) * newViewCast.distance, Color.blue);
                maxViewCast = newViewCast;
            }
        }

        return new EdgeInfo(minViewCast.point, maxViewCast.point);
    }

	public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
	{
		if(!angleIsGlobal)
		{
			angleInDegrees -= transform.eulerAngles.z;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0f);
	}

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float distance;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _distance, float _angle)
        {
            hit = _hit;
            point = _point;
            distance = _distance;
            angle = _angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }
}
