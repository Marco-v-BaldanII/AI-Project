using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;


public class BezierPathCreator 
{
	private Vector3 startPoint;
	private Vector3 endPoint;
	private Vector3 control_point;

	private float t = 0.05f;
	private float closeThreshold = 2f;

	private Vector3 bezier;
	List<Vector3> points;
	

	public BezierPathCreator(float closenessThreshold)
	{
		this.closeThreshold = closenessThreshold;
	}

	CancellationTokenSource tokenSource;

	bool init = false;

	public async void Init(Vector3 startpoint, Vector3 endPoint, Vector3 control_point , float t)
	{
		this.startPoint = startpoint;
		this.endPoint = endPoint;
		this.control_point = control_point;
		this.t = t;

		points = new List<Vector3>();
		init = true;

		tokenSource = new CancellationTokenSource();
		
		index = 0;
		
		await Interpolate(tokenSource.Token);
	}

	private async Task Interpolate(CancellationToken token)
	{
		var t_increment = t;

		while (t != 1){

			//Debug.DrawLine(startPoint, control_point, Color.green);
			//Debug.DrawLine(control_point, endPoint, Color.green);

			var l1 = Vector3.Lerp(startPoint, control_point, t);
			var l2 = Vector3.Lerp(control_point, endPoint, t);

			bezier = Vector3.Lerp(l1, l2, t);
			points.Add(bezier);

			for (int index = 0; index < points.Count; index++)
			{
			 	if (index > 1)
			 	{
			 		//Debug.DrawLine(points[index - 1], points[index], Color.cyan);
			 	}
			}

			t += t_increment;
			await Task.Yield();
			
			token.ThrowIfCancellationRequested(); 
		}
	}

	int index = 0;
	public Vector3 get_point_along_spline(UnityEngine.AI.NavMeshAgent agent, Transform body )
	{

		if (points == null || points.Count == 0)
		{
			return agent.transform.position;
		}

		Vector3 closest_point;

		UnityEngine.AI.NavMeshHit hit;
		UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath() ;

		// while the current point in the spline is outside navigation area, advance to next point
		while ( ! UnityEngine.AI.NavMesh.CalculatePath(agent.transform.position, points[index], UnityEngine.AI.NavMesh.AllAreas, path ) )
		{
			index++;
			index = Mathf.Clamp(index, 0, points.Count - 1);
			if (index == points.Count - 1) { break; }

			//Debug.Log("finding new_point");
		}
		var dist = Vector3.Distance(body.position, points[index]);

        if ( Vector3.Distance( body.position, points[index]) < closeThreshold) /* When close to the point, target the next */
		{
			index++;
			index = Mathf.Clamp(index, 0, points.Count - 1);
		}

		return points[index]; /* Return the current point to move to*/

	}

	public Vector3 GetPerpendicularWithFixedY(Vector3 v, float fixedY)
	{
		float zPrime = 1f;

		if (v.y == 0)
		{
			//  edge case, if y is zero, swap x and z components
			return new Vector3(0, fixedY, v.x);
		}
		else
		{
			float xPrime = -(v.z * zPrime) / v.y; // dot product equals 0 -> orthonormal vector
			return new Vector3(xPrime, fixedY, zPrime);
		}
	}
}
