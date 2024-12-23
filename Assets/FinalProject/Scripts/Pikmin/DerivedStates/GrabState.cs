using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class GrabState :  Agent 
{
	public Pikmin myPikmin;
	
	public GameObject targetObject;
	private Transform transform;
	private Rigidbody rigid;
	
	private const float GRAB_DISTANCE = 0.4f;
	private  float speed = 5f; // TODO: change this to depend on pik color
	
	public int separationWeight = 200;
	public int pursuitWeight = 100;
	
	private const float SEPARATION_DISTANCE = 4f;
	
	private float objectWeight;
	
	Vector3 vSeparation = Vector3.zero;
	
	private const float forceMultiplier = 10f;
	
	
	public override void OnEpisodeBegin()
	{
		// If the Agent fell off the platform, reset its position and momentum

		myPikmin.rigid.angularVelocity = Vector3.zero;
		myPikmin.rigid.velocity = Vector3.zero;
		//this.transform.localPosition = new Vector3(0, 0.5f, 0); // Reset position
		var machine = this.GetComponent<StateMachine>();
		machine.OnChildTransitionEvent(StateType.GRAB);

		//Target.localPosition = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
	}

	
	public override void CollectObservations(VectorSensor sensor)
	{
		// Add something to measure proximity to other pikmins
		sensor.AddObservation(vSeparation);

		sensor.AddObservation(targetObject.transform.localPosition); // Target position
		sensor.AddObservation(this.transform.localPosition); // Agent position

		sensor.AddObservation(myPikmin.rigid.velocity.x); // Velocity along x-axis
		sensor.AddObservation(myPikmin.rigid.velocity.z); // Velocity along z-axis
	}
	
	public override void OnActionReceived(ActionBuffers actionBuffers)
	{
		// Get the actions (force to apply along x and z)
		Vector3 controlSignal = Vector3.zero;
		controlSignal.x = actionBuffers.ContinuousActions[0];
		controlSignal.z = actionBuffers.ContinuousActions[1];

		myPikmin.rigid.AddForce(controlSignal * forceMultiplier);
	}

	void Update()
	{
		vSeparation = Vector3.zero;
		foreach (Pikmin carrot in PikminManager.instance.units) /* Sumatorio */
		{
			//TODO, adjust distance thresholf to be dependent on amount of pikmin carrying
			if (carrot != myPikmin && carrot != null && Vector3.Distance(myPikmin.transform.position, carrot.transform.position) < SEPARATION_DISTANCE / objectWeight    )
			{

				
				float divisor = Vector3.Distance(transform.position, carrot.transform.position) + 0.1f;
														vSeparation += (transform.position - carrot.transform.position) / divisor;

			}
		}
		// subtract reward if close to other pikmin 
		AddReward(- vSeparation.magnitude / 1f);
	}


	
	private void OnCollisionEnter(Collision collision)
	{
		// grant reward when grabbing object
		if (collision.gameObject.tag == "Pellet"){
			
			AddReward(1f);
			EndEpisode();
		}
	}
	
	private void HasNOTGrabbedPellet()
	{
		AddReward(-1f);
		EndEpisode();
	}
	
	public  void CallTransition(StateType new_state_type, IState prev_){}
}
