using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpisodeManager : MonoBehaviour
{

	//TODO, if you want to be able to carry multiple objects youll have to make this into a regular C#class
	// and make a singleton that manages instances of "carrying objecys (the class you have now)"

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
		if(!isTraining){return;}
		
		//ResetEpisode();
		//PikminManager.instance.Spawn(num_pikmin);

		//foreach (Pikmin pik in PikminManager.instance.units)
		//{
		//	GrabAgent agent = pik.GetComponent<GrabAgent>();
		//	if(agent != null)
		//	{
		//		agents.Add(agent);
		//	}
		//}
	}
	
	public void ResetEpisode()
	{
		if (!isTraining){return;}
	
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
		}
		
	}
	
	private void Update()
	{
		float radius = 2f;
		
		for(int i = 0; i < agents.Count; ++i)
		{
			// Calculate the angle for this Pikmin
			float angle = 2 * Mathf.PI * i / agents.Count;

			// Calculate the position based on polar coordinates
			float x = Mathf.Cos(angle) * radius;
			float z = Mathf.Sin(angle) * radius;

			// Assign the Pikmin's position
			Vector3 targetPosition = new Vector3(x, 0f, z);

			// Set this Pikmin's target position 
			GrabAgent agent = agents[i]; // position around the pellet
			if (pellet) agent.Target = pellet.transform.position + targetPosition;
			
			Debug.DrawLine(new Vector3(agent.Target.x, 0, agent.Target.z), new Vector3(agent.Target.x, 10, agent.Target.z));
		}
		
		if (!isTraining){return;}
		
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
