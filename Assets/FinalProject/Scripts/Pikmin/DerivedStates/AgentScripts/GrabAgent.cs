using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class GrabAgent : Agent
{
	Rigidbody rBody;

	public Vector3 Target = Vector3.zero;
	int closeness = 7;
	int prev_closeness = 7;

	public float forceMultiplier = 500f;
	
	public bool arrived { get; private set; } = false;



	// Start is called before the first frame update
	void Start()
	{
		
		rBody = GetComponent<Rigidbody>(); 
	}

    protected override void OnEnable()
    {
		base.OnEnable();
        EpisodeManager.Instance.agents.Add(this);
    }

    public override void OnEpisodeBegin()
	{
		arrived = false;
		if (!rBody){rBody = GetComponent<Rigidbody>(); }

		if (EpisodeManager.Instance.isTraining == false) { return; }

		// If the Agent fell off the platform, reset its position and momentum
		if (this.transform.localPosition.y < 0)
		{
			this.rBody.angularVelocity = Vector3.zero;
			this.rBody.velocity = Vector3.zero;
			this.transform.localPosition = new Vector3(0, 0.5f, 0); // Reset position
		}


		//Target.localPosition = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
	}


	public override void CollectObservations(VectorSensor sensor)
	{
		Target = Vector3.zero;


		sensor.AddObservation(Target); // Target position
		sensor.AddObservation(this.transform.localPosition); // Agent position

		sensor.AddObservation(rBody.velocity.x); // Velocity along x-axis
		sensor.AddObservation(rBody.velocity.z); // Velocity along z-axis
	}

	public override void OnActionReceived(ActionBuffers actionBuffers)
	{
		// Get the actions (force to apply along x and z)
		Vector3 controlSignal = Vector3.zero;
		controlSignal.x = actionBuffers.ContinuousActions[0];
		controlSignal.z = actionBuffers.ContinuousActions[1];
		if (arrived) { rBody.velocity = Vector3.zero; return; }

		print("control signal" + controlSignal);
		rBody.AddForce(controlSignal * forceMultiplier);

		float distanceToTarget = Vector3.Distance(this.transform.position, Target);
		if (distanceToTarget < 0.3f)
		{
			closeness = 1;
			// Reward for reaching the target
			AddReward(1000f);
			arrived = true;
			//EndEpisode();

		}
		else if (distanceToTarget < 1f)
		{
			closeness = 2;
			// Reward for reaching the target
			AddReward(10f);

		}
		else if (distanceToTarget < 2f)
		{
			closeness = 3;
            // Reward for reaching the target
            AddReward(1f);

		}
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
			AddReward(-50);
			
		}
		prev_closeness = closeness;
	}

	void Update()
	{
		if (arrived)
		{
			rBody.velocity = Vector3.zero;
		}
	}

	public override void Heuristic(in ActionBuffers actionsOut)
	{
		var continuousActionsOut = actionsOut.ContinuousActions;
		continuousActionsOut[0] = Input.GetAxis("Horizontal"); 
		continuousActionsOut[1] = Input.GetAxis("Vertical"); 
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Pellet")
		{
            AddReward(-100);
		}
    }

    protected override void OnDisable()
    {
        EpisodeManager.Instance.agents.Remove(this);
		base.OnDisable();
    }
}
