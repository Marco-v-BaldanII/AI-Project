using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabState : IState{
	public Pikmin myPikmin;
	public Vector3 velocity;
    
	private GameObject targetObject;
	private Transform transform;
	private Rigidbody rigid;
	GrabAgent agent;
	private const float GRAB_DISTANCE = 0.4f;
	private  float speed = 5f; // TODO: change this to depend on pik color
    
	public GrabState(Pikmin pikmin)
	{
		myPikmin = pikmin;
	}

    bool isCarrying = false;
    public override void Enter() 
    {
	    agent = myPikmin.GetComponent<GrabAgent>();
	    agent.enabled = true;
		agent.inGrabState = true;
	    ThrowManager.Instance.agents.Add(agent);
		isCarrying = false;
	    // from the pikmin assing to the state and the agent the target object
	    //if ( myPikmin.targetObject != null) {
	    //	targetObject = myPikmin.targetObject; 
	    //	agent.Target = targetObject;
	    //}
	    

    }
    public override void PhysicsProcess()
	{
		if (agent.arrived)
		{
			GrabObject();
		}
    	
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

		//if ( Vector3.Distance(transform.position , targetObject.transform.position) < GRAB_DISTANCE)
		//{
		//	GrabObject();
		//	//state_machine.ChangeState(state_machine.Hide);
		//}

		if (myPikmin.grabbing)
		{

		}

		Debug.Log(Vector3.Distance(transform.position, targetObject.transform.position));

	}
	
	private void GrabObject()
	{
		if (myPikmin.targetObject){
		
		GrabbableObject grab_object  = myPikmin.targetObject.GetComponent<GrabbableObject>(); 
		if ( grab_object != null)
		{
            isCarrying = true;
            CallTransition(State.CARRYING, this);
			grab_object.AddPikmin(myPikmin);
		}
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
		agent.inGrabState = false;
        if (!isCarrying)
		{
			GrabAgent agent = myPikmin.GetComponent<GrabAgent>();
			if (agent) agent.enabled = false;
		}
	}
	
	public override void OnAreaStay(Collider collision)
	{
		if (collision.tag == "Captain" && collision.gameObject != PikminManager.instance.gameObject) /* touched by whistle or captain = rejoin group */
		{
			CallTransition(State.IN_SQUAD, this);
		}
	}

}
