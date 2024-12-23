using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeprecatedGrabState : IState{
	public Pikmin myPikmin;
	public Vector3 velocity;
    
	private GameObject targetObject;
	private Transform transform;
	private Rigidbody rigid;
	
	private const float GRAB_DISTANCE = 0.4f;
	private  float speed = 5f; // TODO: change this to depend on pik color
	
	public int separationWeight = 200;
	public int pursuitWeight = 100;
	
	private const float SEPARATION_DISTANCE = 4f;
	
	private float objectWeight;
    
	public DeprecatedGrabState(Pikmin pikmin)
	{
		myPikmin = pikmin;
	}
	
	public void IState.Enter() 
    {
	    var direction= myPikmin.targetObject.transform.position - myPikmin.transform.position;
	    direction.Normalize();
	    velocity = direction * 5;
	    
	    targetObject = myPikmin.targetObject;
	    transform = myPikmin.transform;
	    rigid = myPikmin.GetComponent<Rigidbody>();
	    
	    GrabbableObject grab_obj = targetObject.GetComponent<GrabbableObject>();
	    objectWeight = (float) grab_obj.weight;
	    

    }
    public void IState.PhysicsProcess()
    {
	    Pursuit();
    }
    
	private void Pursuit()
	{
		Vector3 direction = targetObject.transform.position - transform.position;

		Vector3 desired_velocity = Vector3.Normalize( direction) * speed;

		Vector3 steeringForce = desired_velocity - rigid.velocity;



		Quaternion targetRotation = Quaternion.LookRotation(rigid.velocity, Vector3.up);

		transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 50 * Time.deltaTime);

		if ( Vector3.Distance(transform.position , targetObject.transform.position) < GRAB_DISTANCE)
		{
			GrabObject();
			//state_machine.ChangeState(state_machine.Hide);
		}

		Debug.Log(Vector3.Distance(transform.position, targetObject.transform.position));
		
		
		//---------Separation from other pikmin----------//
		Vector3 vSeparation = Vector3.zero;
		foreach (Pikmin carrot in PikminManager.instance.units) /* Sumatorio */
		{
			//TODO, adjust distance thresholf to be dependent on amount of pikmin carrying
			if (carrot != myPikmin && carrot != null && Vector3.Distance(myPikmin.transform.position, carrot.transform.position) < SEPARATION_DISTANCE / objectWeight    )
			{


				float divisor = Vector3.Distance(transform.position, carrot.transform.position) + 0.1f;
				vSeparation += (transform.position - carrot.transform.position) / divisor;

			}
		}

		vSeparation /= PikminManager.instance.numPikmin;
		
		//rigid.velocity += vSeparation * separationWeight;
		
		
		float forwardProjection = Vector3.Dot(transform.forward, vSeparation);
		vSeparation = (-transform.right * forwardProjection) * separationWeight;
		rigid.velocity += vSeparation;
		
		rigid.velocity += steeringForce * pursuitWeight * Time.deltaTime; /* Create smooth turn towards next destination */
		rigid.velocity = Vector3.ClampMagnitude(rigid.velocity, 20);
		
		Debug.DrawLine(myPikmin.transform.position, vSeparation, Color.red);
		Debug.DrawLine(myPikmin.transform.position, steeringForce, Color.yellow);

	}
	
	private void GrabObject()
	{
		GrabbableObject grab_object  = targetObject.GetComponent<GrabbableObject>(); 
		if ( grab_object != null)
		{
			CallTransition(StateType.CARRYING, this);
			grab_object.AddPikmin(myPikmin);
		}
	}

    public override void OnBodyStay(Collider collison)
    {
        if (collison.tag == "Pellet")
        {
            GrabObject();
        }
    }
    
	public void IState.Process()
	{
		
	}
	
	public void IState.Exit()
	{
		
	}



    
	public  void IState.OnAreaEnter(Collider collision){}
	public  void IState.OnAreaStay(Collider collision){}
	public  void IState.OnAreaExit(Collider collision){}
	
	public  void IState.OnBodyEnter(Collider collison){}
	public  void IState.OnBodyStay(Collider collison) {}

}
