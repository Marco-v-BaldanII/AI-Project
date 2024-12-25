using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabState : IState{
	public Pikmin myPikmin;
	public Vector3 velocity;
    
	private GameObject targetObject;
	private Transform transform;
	private Rigidbody rigid;
	
	private const float GRAB_DISTANCE = 0.4f;
	private  float speed = 5f; // TODO: change this to depend on pik color
    
	public GrabState(Pikmin pikmin)
	{
		myPikmin = pikmin;
	}
	
    public override void Enter() 
    {
	    GrabAgent agent = myPikmin.GetComponent<GrabAgent>();
	    agent.enabled = true;
	    // from the pikmin assing to the state and the agent the target object
	    if ( myPikmin.targetObject) targetObject = myPikmin.targetObject; agent.targetObject = targetObject;
	    

    }
    public override void PhysicsProcess()
    {
	    //Pursuit();
    }
    
	private void Pursuit()
	{
		Vector3 direction = targetObject.transform.position - transform.position;

		Vector3 desired_velocity = Vector3.Normalize( direction) * speed;

		Vector3 steeringForce = desired_velocity - rigid.velocity;

		rigid.velocity += steeringForce * 50 * Time.deltaTime; /* Create smooth turn towards next destination */
		rigid.velocity = Vector3.ClampMagnitude(rigid.velocity, 20);

		Quaternion targetRotation = Quaternion.LookRotation(rigid.velocity, Vector3.up);

		transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 50 * Time.deltaTime);

		if ( Vector3.Distance(transform.position , targetObject.transform.position) < GRAB_DISTANCE)
		{
			GrabObject();
			//state_machine.ChangeState(state_machine.Hide);
		}

		Debug.Log(Vector3.Distance(transform.position, targetObject.transform.position));

	}
	
	private void GrabObject()
	{
		GrabbableObject grab_object  = targetObject.GetComponent<GrabbableObject>(); 
		if ( grab_object != null)
		{
			CallTransition(State.CARRYING, this);
			grab_object.AddPikmin(myPikmin);
		}
	}

    public override void OnBodyStay(Collider collison)
    {
        //if (collison.tag == "Pellet")
        //{
        //    GrabObject();
        //}
    }

	public override void Exit()
	{
		GrabAgent agent = myPikmin.GetComponent<GrabAgent>();
		if (agent) agent.enabled = false;
	}

}
