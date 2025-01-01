
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MBT;

[AddComponentMenu("")]
[MBTNode(name = "Patroll")]
public class MBT_Patroll : Leaf
{
	
	public Transform[] waypoints;  // Array de puntos de patrullaje

	[Header("Patrolling related fields")]
	private float max_speed ;
	public float moveSpeed;
	public float averageWaitTime = 1f;
	private int currentWaypointIndex;
	public float closenessThreshold = 2.1f;
	public  GameObject ghost;
	const int MAX_WIDTH = 28;
	public NavMeshAgent nav_mesh_agent;
	
	public float controlPointStrenght ;

	[Header("Non monoB dependency classes")]

	public UniqueNumberGenerator num_generator;
	public BezierPathCreator bezier;

    private void Awake()
    {
        max_speed = moveSpeed;
        num_generator = new UniqueNumberGenerator();
        bezier = new BezierPathCreator(closenessThreshold);

        // Pick random waypoint
        new_destination();
    }
    public override void  OnEnter()
	{
		print("enter");

		// Assign random starting position withtin the bounds of the map
		//transform.position = new Vector3  (Random.RandomRange(-MAX_WIDTH, MAX_WIDTH) , transform.position.y , Random.RandomRange(-MAX_WIDTH, MAX_WIDTH) );
		//ghost.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 2);

	}

	bool far_away = true;
	bool acelerating = false;

	const int WAY_POINT_STOP_DISTANCE = 2;

	public override NodeResult Execute()
	{
		UpdateDestination();

		nav_mesh_agent.speed = moveSpeed;

		Arrive();
		
		return NodeResult.success;
	}

	private void UpdateDestination()
	{
		if (ghost) /* if there is a ghost follow the ghost (who follows the spline) */
		{
			var ghost_body = ghost.GetComponent<Rigidbody>();
			var direction = bezier.get_point_along_spline(nav_mesh_agent, ghost.transform) - ghost.transform.position;

			ghost_body.velocity = direction.normalized * (nav_mesh_agent.speed + 0.9f);
			
			/*TODO put this line somewhere that executeds even if Patroll is disabled  */ 
			
		}
		else /* simply follow the spline */
		{
			nav_mesh_agent.destination = bezier.get_point_along_spline(nav_mesh_agent, transform);
		}
	}

	private void Arrive()
	{
		Vector3 position = new Vector3(transform.position.x, 0, transform.position.z); Vector3 wP = new Vector3(waypoints[currentWaypointIndex].position.x, 0, waypoints[currentWaypointIndex].position.z);
		if (Vector3.Distance(position, wP) < WAY_POINT_STOP_DISTANCE) /* I'm close , so decelerate */
		{
			if (far_away)
			{
				far_away = false;
				StopAllCoroutines();
				StartCoroutine(Accelerate(0));
			}
		}
		else if (!acelerating) /* I'm far away , so accelerate */
		{
			acelerating = true;
			StopAllCoroutines();
			StartCoroutine(Accelerate(max_speed));
		}
	}

	IEnumerator Accelerate(float value = 0)
	{
		yield return null;
		var t = 0.1f;

		while (Mathf.Abs( moveSpeed - value) > 0.1f) /* While i've not reached the desired speed*/
		{
			/*Lerp towards the desired speed*/

			moveSpeed = Mathf.Lerp(moveSpeed, value, t);
			t += 0.1f;
			yield return null;
		}
		
		yield return new WaitForSeconds(Random.Range( averageWaitTime*0.5f, averageWaitTime * 2f )); /* Wait in place */
		
		// Target next waypoint
		if (! far_away) { new_destination(); } /* I've reached the waypoint so, find a new one */
		far_away = true; acelerating = false;

	}

	private void new_destination()
	{
		currentWaypointIndex = num_generator.GenerateUniqueNum(0, waypoints.Length);

		var perpendicular_vector = bezier.GetPerpendicularWithFixedY( transform.position - waypoints[currentWaypointIndex].position, transform.position.y) * 2;

		bezier.Init(transform.position, waypoints[currentWaypointIndex].position, perpendicular_vector* controlPointStrenght, 0.1f);
	}


}