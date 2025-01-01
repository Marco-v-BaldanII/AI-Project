using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowManager : MonoBehaviour
{

	//TODO, if you want to be able to carry multiple objects youll have to make this into a regular C#class
	// and make a singleton that manages instances of "carrying objecys (the class you have now)"

	public bool isTraining;
	
	// Static instance of the class
	public static ThrowManager Instance { get; private set; }

	// Flag to determine if the episode should end for all agents
	public bool EpisodeEndFlag { get; private set; } = false;
	
	public GameObject pellet;
		
	public List<GrabAgent> agents;
	
	public int num_pikmin ;
	
	private const float EPISODE_LENGTH = 22f;
	
	float timer = 0f;

	List<RetrievingManager> retrievingMissions;

	private void Awake()
	{

		if (Instance != null && Instance != this)
		{
			Destroy(gameObject); 
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}
	
	private void Start()
	{
		retrievingMissions = new List<RetrievingManager>();
	}

    private void Update()
    {
        foreach(RetrievingManager retriever in retrievingMissions)
		{
			retriever.Process();
		}
    }

    public void AddPikmin(GameObject pellet, GrabAgent pikmin)
	{
		bool found = false;
		
		foreach (RetrievingManager retriver in retrievingMissions)
		{
			if (retriver.pellet == pellet)
			{
				retriver.AddAgent(pikmin);
				found = true;
				break;
			}
		}
		
		if (! found) /* Create a new retrieving operation and add pikmin to it */
		{
			RetrievingManager retriver = new RetrievingManager();
			retriver.pellet = pellet;
            retriver.AddAgent(pikmin);
            retrievingMissions.Add(retriver);
		}
		
	}

}