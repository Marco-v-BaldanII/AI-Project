using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpisodeManager : MonoBehaviour
{

		// Static instance of the class
		public static EpisodeManager Instance { get; private set; }

		// Flag to determine if the episode should end for all agents
	public bool EpisodeEndFlag { get; private set; } = false;
		
	public List<GrabAgent> agents;

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
	
	
	public void ResetEpisode(float points)
	{
		for (int i = 0; i < agents.Count; ++i)
		{
		
			agents[i].AddReward(points);
			agents[i].EndEpisode();
		}
	}
	
	

}
