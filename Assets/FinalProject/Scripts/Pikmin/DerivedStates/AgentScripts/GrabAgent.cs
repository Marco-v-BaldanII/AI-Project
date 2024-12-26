using System.Collections;
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
	private float speed = 5f; // TODO: change this to depend on pik color

	public int separationWeight = 200;
	public int pursuitWeight = 100;

	private const float SEPARATION_DISTANCE = 2f;

	private float objectWeight;

	Vector3 vSeparation = Vector3.zero;

	private const float forceMultiplier = 8;

	private float ditance_to_target;
	private float prev_distance;

	public float time = 0;

	public bool arrived = false;

	// Distinguish between training and gameplay modes
	public bool isTraining = true;

	private const float SEPARATION_WEIGHT = 15f;

	private float lower_limit = -9f;
	private float upper_limit = 9f;

	float limit_timer = 0f;

	public void Awake()
	{
		if (isTraining)
		{
			EpisodeManager.Instance.agents.Add(this);
		}
	}
	
	public bool lazy = false;

	public override void OnEpisodeBegin()
	{
		if (!isTraining) return; // Skip environment reset if not training
		
		
		int lazy_id = Random.Range(0,2);
		if (lazy_id == 0)
		{
			lazy = true;
		}
		else
		{
			lazy = false;
		}
		
		arrived = false;
		if (!targetObject) { targetObject = GameObject.Find("Posy"); }

		myPikmin.rigid.angularVelocity = Vector3.zero;
		myPikmin.rigid.velocity = Vector3.zero;
		this.transform.localPosition = new Vector3(Random.Range(-1f,1f), 0, Random.Range(-1f,1f));

		targetObject.transform.localPosition = new Vector3(Random.Range(lower_limit, upper_limit), 0, Random.Range(lower_limit, upper_limit));

		var machine = this.GetComponent<StateMachine>();
		machine.Start();
		machine.OnChildTransitionEvent(State.GRAB);

		ditance_to_target = Vector3.Distance(this.transform.position, targetObject.transform.position);
		prev_distance = ditance_to_target;

		time = 0;
	}

	public override void CollectObservations(VectorSensor sensor)
	{
		sensor.AddObservation(vSeparation);
		sensor.AddObservation(targetObject.transform.localPosition); // Target position
		sensor.AddObservation(this.transform.localPosition); // Agent position
		sensor.AddObservation(myPikmin.rigid.velocity.x); // Velocity along x-axis
		sensor.AddObservation(myPikmin.rigid.velocity.z); // Velocity along z-axis
		sensor.AddObservation(ditance_to_target);
		
		sensor.AddObservation(time);

		foreach (Pikmin carrot in PikminManager.instance.units)
		{
			//if (carrot != myPikmin) sensor.AddObservation(carrot.transform.position);

			if (carrot != myPikmin)
			{
                Vector3 relativePosition = carrot.transform.position - this.transform.position;
				//sensor.AddObservation(relativePosition); // Relative position to each agent
				//sensor.AddObservation(relativePosition.magnitude);
            }
		}
	}

	public override void OnActionReceived(ActionBuffers actionBuffers)
	{
		

		Vector3 controlSignal = Vector3.zero;
		controlSignal.x = actionBuffers.ContinuousActions[0];
		controlSignal.z = actionBuffers.ContinuousActions[1];

		controlSignal *= forceMultiplier;
		if (arrived || lazy ) { return; }
		else{
			myPikmin.rigid.velocity = new Vector3(controlSignal.x, myPikmin.rigid.velocity.y, controlSignal.z);
		}
	}

	void Update()
	{
		limit_timer += Time.deltaTime;


		int numPikmin = 0;
        vSeparation = Vector3.zero;
        foreach (Pikmin carrot in PikminManager.instance.units)
        {
            if (carrot != myPikmin && carrot != null && Vector3.Distance(myPikmin.transform.position, carrot.transform.position) < SEPARATION_DISTANCE / objectWeight)
            {
                float divisor = Vector3.Distance(transform.position, carrot.transform.position) + 0.1f;
	            vSeparation += (transform.position - carrot.transform.position) / divisor;
	            numPikmin++;
            }
        }

        if (arrived)
		{
            float divisor = 1;
            if (vSeparation != Vector3.zero) { divisor = vSeparation.magnitude; }
	        //AddReward(6f / divisor);
            myPikmin.rigid.velocity = Vector3.zero;

			return;
		}

		if (isTraining)
		{
			time += Time.deltaTime;

			if (time >= 30f)
			{
				HasNOTGrabbedPellet();
			}
			
	
		}


		if (isTraining)
		{
			var punishment = -(vSeparation.magnitude) * time * time;
			//AddReward(punishment);
			print("punishing agent for being close at" + punishment);

			// ive added time^2 to incentivize them to be fast
			//if (ditance_to_target > prev_distance)
			//{
			//	AddReward(-0.1f   *  ( (time * time) *0.6f ) / numPikmin);
			//}
			//else if (ditance_to_target < prev_distance)
			//{
			//	AddReward(0.4f  );
			//}
		}

		prev_distance = ditance_to_target;
	}

	void HandleLimits()
	{
		limit_timer += Time.deltaTime;
		if (limit_timer > 600)
		{
			lower_limit = -20f; upper_limit = 20f;
		}
		else if (limit_timer > 225)
		{
			lower_limit = -12f; upper_limit = 12f;
		}
        else if (limit_timer > 100)
        {
            lower_limit = -9f; upper_limit = 9f;
        }
    }

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Pellet" && !arrived)
		{
			// Grabbing the pellet further away from other pikmins yields more reward
			float divisor = 1;
			if (vSeparation != Vector3.zero){divisor = vSeparation.magnitude;}
			
			var pellet = collision.gameObject.GetComponent<GrabbableObject>();
			pellet.AddPikmin(myPikmin);
			arrived = true;
			time = time == 0 ? 1 : time;
			float reward = (100f/ (divisor* SEPARATION_WEIGHT) ) / time ;
			reward *= 2;
			AddReward(100f/ time);
			//AddReward(reward / EpisodeManager.Instance.RetrieveveWorkingPiks() );
			print("reward is " +reward + "after " +  time + "seconds with " + divisor + " separation " );
			

			if (isTraining && pellet.num_pikmin == PikminManager.instance.units.Count)
			{
				
				
				EpisodeManager.Instance.ResetEpisode(0 );
				pellet.num_pikmin = 0;
				
			}
			else if (!isTraining)
			{
				var machine = this.GetComponent<StateMachine>();
				machine.OnChildTransitionEvent(State.CARRYING);
			}
		}
	}

	private void HasNOTGrabbedPellet()
	{
		if (isTraining )
		{
			EpisodeManager.Instance.ResetEpisode(0);
		}

	}

	public override void Heuristic(in ActionBuffers actionsOut)
	{
		var continuousActionsOut = actionsOut.ContinuousActions;
		continuousActionsOut[0] = Input.GetAxis("Horizontal");
		continuousActionsOut[1] = Input.GetAxis("Vertical");
	}
}
