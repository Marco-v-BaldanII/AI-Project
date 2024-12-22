using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownState : IState{
	public Pikmin myPikmin;
	
	private const float ANGLE = 45;

	public ThrownState(Pikmin pikmin)
	{
		myPikmin = pikmin;
	}
	
	public override void Enter() 
	{
		Vector3 startPos = PikminManager.instance.transform.position;
		myPikmin.transform.position = startPos;
		//myPikmin.rigid.velocity = CalculateVelocityGivenTime(startPos, PikminManager.instance.GetCursorPosition(), ANGLE,  0.4f ) ;
		//myPikmin.rigid.velocity = CalculateVelocityWithTime(startPos,PikminManager.instance.GetCursorPosition(), 0.5f );
		myPikmin.rigid.velocity =   CalculateVelocity(startPos, PikminManager.instance.GetCursorPosition(), ANGLE);
	}

	public override void Exit()
	{

	}

	public override void Process()
	{

	}

	public override void PhysicsProcess()
	{
		//if(myPikmin.rigid.velocity.y == 0) /* I've landed */
		//{
		//	CallTransition(State.IDLE, this);
		//}
	}
	
	public Transform target; // Assign the target position in the Unity Inspector
	public Transform launchPoint; // Point from where the projectile is launched
	public Rigidbody projectile; // Rigidbody of the projectile
	float gravity = Mathf.Abs(Physics.gravity.y) ; // Gravity value (default Earth gravity)




	// Method to calculate velocity
	public Vector3 CalculateVelocity(Vector3 start, Vector3 end, float angle)
	{
		// Distance in x and y
		Vector3 direction = end - start;
		float distance = direction.magnitude; // Total distance to the target
		float heightDifference = end.y - start.y;

		// Convert angle to radians
		float angleRad = angle * Mathf.Deg2Rad;

		// Calculate velocity using kinematic equations
		float v0Squared = (gravity * distance * distance) / 
		(2 * (distance * Mathf.Tan(angleRad) - heightDifference) * Mathf.Cos(angleRad) * Mathf.Cos(angleRad));

		if (v0Squared <= 0)
		{
			Debug.LogError("Target is unreachable with the given angle.");
			return Vector3.zero; // Return zero if the target is unreachable
		}

		float v0 = Mathf.Sqrt(v0Squared);

		// Decompose the velocity into x, y, and z components
		Vector3 velocity = direction.normalized; // Normalize to get direction
		velocity.y = 0; // Ignore y for now
		velocity *= Mathf.Cos(angleRad) * v0; // Horizontal velocity
		velocity.y = Mathf.Sin(angleRad) * v0; // Vertical velocity

		return velocity;
	}
	
	public Vector3 CalculateVelocityWithTime(Vector3 start, Vector3 end, float travelTime)
	{
		Vector3 displacement = end - start;
		float horizontalDistance = new Vector2(displacement.x, displacement.z).magnitude;

		// Calculate horizontal and vertical velocities
		float horizontalSpeed = horizontalDistance / travelTime;
		float verticalSpeed = (displacement.y + 0.5f * gravity * travelTime * travelTime) / travelTime;

		// Decompose into velocity components
		Vector3 velocity = displacement.normalized; // Normalize for direction
		velocity.y = 0; // Exclude vertical for now
		velocity *= horizontalSpeed; // Horizontal component
		velocity.y = verticalSpeed; // Vertical component

		return velocity;
	}
	
	public override void OnBodyEnter(Collider collider)
	{
		CallTransition(State.IDLE, this);
	}

	//public void FireProjectile(float angle)
	//{
	//	// Calculate velocity
	//	Vector3 velocity = CalculateVelocity(launchPoint.position, target.position, angle);

	//	if (velocity != Vector3.zero)
	//	{
	//		// Instantiate or reset projectile
	//		Rigidbody proj = Instantiate(projectile, launchPoint.position, Quaternion.identity);

	//		// Apply velocity
	//		proj.velocity = velocity;
	//	}
	//}

}