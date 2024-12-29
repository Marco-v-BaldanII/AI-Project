using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;
using MBT;

[AddComponentMenu("")]
[MBTNode(name = "DetectTarget")]
public class MBT_DetectTarget : Leaf
{

	Camera frustum;
	Blackboard board;
	public LayerMask mask;


	void Awake()
    {
	    frustum = GetComponent<Camera>();
	    board = GetComponent<Blackboard>();
    }


	public override NodeResult Execute()
    {
	    bool detect = false;
		Transform detectTrasnform = null;
        TransformVariable trs = board.GetVariable<TransformVariable>("target");

        

        Collider[] colliders = Physics.OverlapSphere(transform.position, frustum.farClipPlane, mask);
	    Plane[] planes = GeometryUtility.CalculateFrustumPlanes(frustum);
	    foreach (Collider col in colliders)
	    {
		    if (col.gameObject != gameObject && GeometryUtility.TestPlanesAABB(planes, col.bounds))
		    {
			    RaycastHit hit;
			    Ray ray = new Ray();
			    ray.origin = transform.position;
			    ray.direction = (col.transform.position - transform.position).normalized;
			    ray.origin = ray.GetPoint(frustum.nearClipPlane);




			    if (Physics.Raycast(ray, out hit, frustum.farClipPlane, mask))
			    {
				    //targetTransform = hit.collider.transform;

					detectTrasnform = hit.collider.transform;





                }
		    }

	    }
	    

		trs.Value = detectTrasnform;
	    return NodeResult.failure;
    }
}
