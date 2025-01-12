
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
		myPikmin.source.clip = myPikmin.throwSfx;
		myPikmin.source?.Play();
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
        Quaternion targetRotation = Quaternion.LookRotation(myPikmin.rigid.velocity.normalized, Vector3.up);
        myPikmin.transform.rotation = Quaternion.Slerp(myPikmin.transform.rotation, targetRotation, Time.deltaTime * 10f);
    }
    public override void PhysicsProcess()
    {

	}
	
	public UnityEngine.Transform target; // Assign the target position in the Unity Inspector
	public UnityEngine.Transform launchPoint; // Point from where the projectile is launched
	public Rigidbody projectile; // Rigidbody of the projectile
	float gravity = Mathf.Abs(Physics.gravity.y) ; // Gravity value (default Earth gravity)




	// Method to calculate velocity
	public Vector3 CalculateVelocity(Vector3 start, Vector3 end, float angle)
	{

		Vector3 direction = end - start;
		float distance = direction.magnitude; // Total distance to the target
		float heightDifference = end.y - start.y;

		float angleRad = angle * Mathf.Deg2Rad;

		float v0Squared = (gravity * distance * distance) / 
		(2 * (distance * Mathf.Tan(angleRad) - heightDifference) * Mathf.Cos(angleRad) * Mathf.Cos(angleRad));

		if (v0Squared <= 0)
		{
			Debug.LogError("Target is unreachable with the given angle.");
			return Vector3.zero; // Return zero if the target is unreachable
		}

		float v0 = Mathf.Sqrt(v0Squared);

		// Decompose the velocity into x, y, and z components
		Vector3 velocity = direction.normalized; 
		velocity.y = 0; 
		velocity *= Mathf.Cos(angleRad) * v0; 
		velocity.y = Mathf.Sin(angleRad) * v0; 

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