using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using static Codice.Client.Common.EventTracking.TrackFeatureUseEvent.Features.DesktopGUI.Filters;

public class GrabAgent : Agent
{
	Rigidbody rBody;

	public Vector3 Target = Vector3.zero;
	int closeness = 7;
	int prev_closeness = 7;

	public float forceMultiplier = 500f;
	
	public bool arrived { get; private set; } = false;
	public bool inGrabState = false;

	float time = 2f;

	// Start is called before the first frame update
	void Start()
	{
		
		rBody = GetComponent<Rigidbody>(); 
	}

    protected override void OnEnable()
    {
		base.OnEnable();
        //EpisodeManager.Instance.agents.Add(this);
    }

    public override void OnEpisodeBegin()
	{
		arrived = false;
		if (!rBody){rBody = GetComponent<Rigidbody>(); }

		if (ThrowManager.Instance.isTraining == false) { return; }

		// If the Agent fell off the platform, reset its position and momentum
		//if (this.transform.localPosition.y < 0)
		//{
		//	this.rBody.angularVelocity = Vector3.zero;
		//	this.rBody.velocity = Vector3.zero;
		//	this.transform.localPosition = new Vector3(0, 0.5f, 0); // Reset position
		//}


		//Target.localPosition = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
	}


	public override void CollectObservations(VectorSensor sensor)
	{
		// this is called everyframe to update these values
		

		if (!rBody){rBody = GetComponent<Rigidbody>();}

		sensor.AddObservation(Target); // Target position
		sensor.AddObservation(this.transform.localPosition); // Agent position

		sensor.AddObservation(rBody.velocity.x); // Velocity along x-axis
		sensor.AddObservation(rBody.velocity.z); // Velocity along z-axis

	}

	public bool done = false;

	public override void OnActionReceived(ActionBuffers actionBuffers)
	{
		// Get the actions (force to apply along x and z)
		
	
		
		Vector3 controlSignal = Vector3.zero;
		controlSignal.x = actionBuffers.ContinuousActions[0];
		controlSignal.z = actionBuffers.ContinuousActions[1];
		if (!inGrabState){return;}
		if (arrived) { rBody.velocity = Vector3.zero; return; }

		print("control signal" + controlSignal);
		rBody.AddForce(controlSignal * forceMultiplier);
		
		Target.y = transform.position.y;

		float distanceToTarget = Vector3.Distance(this.transform.position, Target);
		if (distanceToTarget < 2f && ! done)
		{
			done = true;
			closeness = 1;
            transform.DOMove(Target, 0.9f).OnComplete(() =>
            {
                AddReward(500f);
                arrived = true;
            });
            // Reward for reaching the target
   //         AddReward(500f);
			//arrived = true;
			//EndEpisode();

		}
		//else if (distanceToTarget < 0.9f)
		//{
		//	//arrived = true;
		//	closeness = 2;
		//	// Reward for reaching the target
		//	AddReward(10f);

		//}
		//else if (distanceToTarget < 1.2f)
		//{
		//	closeness = 3;
		//	// Reward for reaching the target
		//	AddReward(7f);

		//}
		//else if (distanceToTarget < 1.4f)
		//{
		//	closeness = 3;
		//	// Reward for reaching the target
		//	AddReward(5f);

		//}
		//else if (distanceToTarget < 2f)
		//{
		//	closeness = 3;
        //    // Reward for reaching the target
        //    AddReward(1f);

		//}
		else if (distanceToTarget < 3f)
		{
			closeness = 4;
            // Reward for reaching the target
            AddReward(0.6f);

		}
		if (distanceToTarget < 5f)
		{closeness = 5;

            // Reward for reaching the target
            AddReward(0.3f);

		}
		else if (distanceToTarget < 7f)
		{
			closeness = 6;
            // Reward for reaching the target
            AddReward(0.2f);

		}
		else if (distanceToTarget < 10f)
		{
			closeness = 7;
            // Reward for reaching the target
            AddReward(0.1f);

		}
		
		if ( closeness > prev_closeness)
		{
			AddReward(-30);
			
		}
		prev_closeness = closeness;
	}

	void Update()
	{
		if (arrived)
		{
			rBody.velocity = Vector3.zero;
		}
		else if (inGrabState)
		{
			time -= Time.deltaTime;
			if (time <= 0)
			{
				time = 3;
				transform.DOMove(Target, 0.9f);
			}
		}
	}
	

	public override void Heuristic(in ActionBuffers actionsOut)
	{
		var continuousActionsOut = actionsOut.ContinuousActions;
		continuousActionsOut[0] = Input.GetAxis("Horizontal"); 
		continuousActionsOut[1] = Input.GetAxis("Vertical");
    }

    //public void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.collider.tag == "Pellet")
    //    {
    //        AddReward(-100);
    //    }
    //}
    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.tag == "Pellet")
	//	{
    //        AddReward(-100);
	//	}
    //}

    protected override void OnDisable()
    {
        ThrowManager.Instance.agents.Remove(this);
		base.OnDisable();
    }
}
