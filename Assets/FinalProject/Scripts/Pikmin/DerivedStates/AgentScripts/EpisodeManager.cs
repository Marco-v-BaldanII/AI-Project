﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpisodeManager : MonoBehaviour
{


	public bool isTraining;
	
		// Static instance of the class
		public static EpisodeManager Instance { get; private set; }

		// Flag to determine if the episode should end for all agents
	public bool EpisodeEndFlag { get; private set; } = false;
	
	public GameObject pellet;
		
	public List<GrabAgent> agents;
	
	public int num_pikmin ;
	
	private const float EPISODE_LENGTH = 22f;
	
	float timer = 0f;
	float radius = 2f;

		// Awake is called when the script instance is being loaded
		private void Awake()
		{
			// Ensure there's only one instance of EpisodeManager
			if (Instance != null && Instance != this)
			{
				    Destroy(gameObject); // Destroy any duplicate instances
			}
			else
			{
			 	Instance = this;
			 	DontDestroyOnLoad(gameObject); // Make this persist across scenes
			}
		}
	
	private void Start()
	{
		ResetEpisode();
		PikminManager.instance.Spawn(num_pikmin);

		foreach (Pikmin pik in PikminManager.instance.units)
		{
			GrabAgent agent = pik.GetComponent<GrabAgent>();
			if(agent != null)
			{
				agents.Add(agent);
			}
		}
	}
	
	public void ResetEpisode()
	{
		//for (int i = 0; i < agents.Count; ++i)
		//{
		//	if (agents[i].lazy == false){
		//	 	agents[i].AddReward(points);
		//	}
		//	agents[i].EndEpisode();

		//}
		
		//num_pikmin = RandomAgentAmount();
		
		//PikminManager.instance.DestroyAll();
		//PikminManager.instance.Spawn(num_pikmin);
		//agents.Clear();
		
		//foreach (Pikmin pik in PikminManager.instance.units)
		//{
		//	GrabAgent agent = pik.GetComponent<GrabAgent>();
		//	if(agent != null)
		//	{
		//		agents.Add(agent);
		//	}
		//}
		radius = Random.Range(1f, 4f);
		
		foreach (Pikmin pik in PikminManager.instance.units)
		{
			pik.transform.position = Vector3.zero;
			GrabAgent agent = pik.GetComponent<GrabAgent>();
				if(agent != null)
				{
					agent.EndEpisode();
				}
			
		}
		if (isTraining)
		{
			pellet.transform.position = new Vector3(Random.Range(-8f,8f), 0 , Random.Range(-8f,8f));
			//pellet.transform.localScale = new Vector3(Random.Range(3f,7f), Random.Range(3f, 7f), Random.Range(3f,7f) ));
			
			float scale = Random.Range(3f,6f);
			if(scale > 5.2f)
			{
				radius = 4f;
			}
		}
		
	}
	
	private void Update()
	{

		
		for(int i = 0; i < num_pikmin; ++i)
		{
			// Calculate the angle for this Pikmin
			float angle = 2 * Mathf.PI * i / num_pikmin;

			// Calculate the position based on polar coordinates
			float x = Mathf.Cos(angle) * radius;
			float z = Mathf.Sin(angle) * radius;

			// Assign the Pikmin's position
			Vector3 targetPosition = new Vector3(x, 0f, z);

			// Set this Pikmin's target position 
			GrabAgent agent = agents[i]; // position around the pellet
			agent.Target = pellet.transform.position + targetPosition;

			// Tells the pikmin how big the object is
			Collider col = pellet.GetComponent<Collider>();
			if (col != null) { agent.bounds = col.bounds.size; }
			
			Debug.DrawLine(new Vector3(agent.Target.x, 0, agent.Target.z), new Vector3(agent.Target.x, 10, agent.Target.z));
		}
		
		timer += Time.deltaTime;
		
		int num_arrived = 0;
		foreach (GrabAgent agent in agents)
		{
			if(agent.arrived)
			{
				num_arrived++;
			}
		}
		if (num_arrived == agents.Count || timer > EPISODE_LENGTH)
		{
			if(num_arrived == agents.Count){print("success");}
			ResetEpisode();
			timer = 0f;
		}
		//timer += Time.deltaTime;
		
		//if ( timer > EPISODE_LENGTH)
		//{
		//	ResetEpisode();
		//	timer = 0f;
		//}
		
	}
	
	public int RetrieveveWorkingPiks()
	{
		int num = 0;
		//for (int i = 0; i < agents.Count; ++i)
		//{
		//	if (agents[i].lazy == false){
		//		num++;
		//	}
		//}
		
		return num;
	}
	
	public int RandomAgentAmount()
	{
		return Random.Range(1,10);
	}

}
