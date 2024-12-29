using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PursuitFOV : MonoBehaviour
{
	public Camera frustum;
	public LayerMask mask;

	public Rigidbody ghostRb;

	//private Rigidbody rigid;
	
	private Transform targetTransform;
	
	private PatrollingAI patrollAI;
	
	private NavMeshAgent navMesh;
	

	//public ZombieSpawner blackboard;

	//public Wander wander_component;


	private float slowDownRadius = 3.0f;

	private void Awake()
	{
		//rigid = GetComponent<Rigidbody>();
		navMesh = GetComponent<NavMeshAgent>();
		patrollAI = GetComponent<PatrollingAI>();
		frustum.enabled = false;
	}

	void Update()
	{
		bool detect = false;

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

					targetTransform = hit.collider.transform;
						detect = true;
						// Seek
					Arrive(hit.collider.transform.position);
						
					navMesh.destination = hit.collider.transform.position;

						Debug.DrawLine(ray.origin, hit.collider.transform.position);
                        

					//			StartCoroutine("FaceTarget");

						// notify all other zombies



				}
			}

		}

		if (!detect)
		{
			patrollAI.enabled = true;
			//wander_component.Move();
		}
		else
		{
			patrollAI.enabled = false;
		}

	}

	bool rotating = false;

	public void DetectCaptain(GameObject player)
	{
		StartCoroutine("FaceTarget");
	}

	private IEnumerator FaceTarget() /* Slowly turn to face target */
	{   if (!rotating)
	{

		rotating = true;
		for (int i = 0; i < 50; ++i)
		{

			Quaternion targetRotation = Quaternion.LookRotation(( targetTransform.position - transform.position), Vector3.up);

			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 400 * Time.deltaTime);
			yield return null;
		}
		rotating = false;
	}
		yield return null;
	}

	private void Arrive(Vector3 colliderPos)
	{
		Vector3 direction = (colliderPos - transform.position).normalized;


		ghostRb.velocity = new Vector3(direction.x, Mathf.Clamp(ghostRb.velocity.y, -2.0f, 0.01f), direction.z) * 5;

		// ARRIVE
		var distance = Vector3.Distance(colliderPos, transform.position);

		if (distance < slowDownRadius) /*Slow down when get closer than "slowDownRadius" */
		{
			ghostRb.velocity *= distance / slowDownRadius;
			if (distance < slowDownRadius / 2)
			{
				ghostRb.velocity = Vector3.zero;
			}
		}
	}

}