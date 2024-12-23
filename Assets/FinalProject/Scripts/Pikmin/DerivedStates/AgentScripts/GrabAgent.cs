﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class GrabAgent : Agent
{
	public Pikmin myPikmin;
	
	public GameObject targetObject;

	private Rigidbody rigid;
	
	private const float GRAB_DISTANCE = 0.4f;
	private  float speed = 5f; // TODO: change this to depend on pik color
	
	public int separationWeight = 200;
	public int pursuitWeight = 100;
	
	private const float SEPARATION_DISTANCE = 2f;
	
	private float objectWeight;
	
	Vector3 vSeparation = Vector3.zero;
	
	private const float forceMultiplier = 8;
	
	private float ditance_to_target;
	private float prev_distance;

	float time = 0;
	
	bool arrived = false;

    public void Awake()
    {
        EpisodeManager.Instance.agents.Add(this);
    }
    public override void OnEpisodeBegin()
	{
		arrived = false;
		if (!targetObject){targetObject = GameObject.Find("Posy") ;}
		

		myPikmin.rigid.angularVelocity = Vector3.zero;
		myPikmin.rigid.velocity = Vector3.zero;
		this.transform.position = new Vector3(0, PikminManager.instance.transform.position.y , 0);
        //this.transform.localPosition = new Vector3(Random.value * 4 - 4, 0.5f, Random.value * 4 - 4); // Reset position

        targetObject.transform.localPosition = new  Vector3(Random.value * 8 - 4, targetObject.transform.localPosition.y, Random.value * 8 - 4);

        var machine = this.GetComponent<StateMachine>();
		machine.Start();
		machine.OnChildTransitionEvent(State.GRAB);
		
		ditance_to_target = Vector3.Distance(this.transform.position, targetObject.transform.position);
		prev_distance = ditance_to_target;
		//Target.localPosition = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
		time = 0;
	}

	
	public override void CollectObservations(VectorSensor sensor)
	{
		// Add something to measure proximity to other pikmins
		sensor.AddObservation(vSeparation);

		sensor.AddObservation(targetObject.transform.localPosition); // Target position
		sensor.AddObservation(this.transform.localPosition); // Agent position

		sensor.AddObservation(myPikmin.rigid.velocity.x); // Velocity along x-axis
		sensor.AddObservation(myPikmin.rigid.velocity.z); // Velocity along z-axis
		
		sensor.AddObservation(ditance_to_target);
		
		
		foreach (Pikmin carrot in PikminManager.instance.units)
		{
			if (carrot != myPikmin) sensor.AddObservation(carrot.transform.position);
		}
		
	}
	
	public override void OnActionReceived(ActionBuffers actionBuffers)
	{
		// Get the actions (force to apply along x and z)
		Vector3 controlSignal = Vector3.zero;
		controlSignal.x = actionBuffers.ContinuousActions[0];
		controlSignal.z = actionBuffers.ContinuousActions[1];

		Debug.Log("pikmin");
		print("pikmin speed " + controlSignal);
		controlSignal *= forceMultiplier;
		myPikmin.rigid.velocity = new Vector3(controlSignal.x, myPikmin.rigid.velocity.y, controlSignal.z);
	}

	void Update()
	{
		if(arrived){myPikmin.rigid.velocity = Vector3.zero; return;}
		
		time += Time.deltaTime;

		if(time >= 8f)
		{
			HasNOTGrabbedPellet();
		}

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
		print("substracting for being close" + vSeparation.magnitude / 1f);
		AddReward(- ( vSeparation.magnitude / 1f )  * 5f ) ;
		
		if(ditance_to_target > prev_distance)
		{
			AddReward(-0.1f);
		}
		else if (ditance_to_target < prev_distance)
        {
			AddReward(0.4f);
		}

		prev_distance = ditance_to_target;
		
	}


	
	private void OnCollisionEnter(Collision collision)
	{
		// grant reward when grabbing object
		if (collision.gameObject.tag == "Pellet" && arrived == false){

			var pellet = collision.gameObject.GetComponent <GrabbableObject > ();
			pellet.AddPikmin(myPikmin);
			arrived = true;
			
			if (pellet.num_pikmin == PikminManager.instance.units.Count)
			{

				EpisodeManager.Instance.ResetEpisode(200f);
				pellet.num_pikmin = 0;
				//AddReward(1000f);
				//EndEpisode();
			}
		}
	}
	
	private void HasNOTGrabbedPellet()
	{

		EpisodeManager.Instance.ResetEpisode(-100f);
		//AddReward(-100f);
		//EndEpisode();
	}
	
	// For manual control
	public override void Heuristic(in ActionBuffers actionsOut)
	{
		var continuousActionsOut = actionsOut.ContinuousActions;
		continuousActionsOut[0] = Input.GetAxis("Horizontal"); 
		continuousActionsOut[1] = Input.GetAxis("Vertical"); 
	}
	

}
