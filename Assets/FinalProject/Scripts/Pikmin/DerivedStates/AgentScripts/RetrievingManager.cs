using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetrievingManager 
{
    public bool isTraining;

    // Flag to determine if the episode should end for all agents
    public bool EpisodeEndFlag { get; private set; } = false;

    public GameObject pellet;

    public List<GrabAgent> agents;

    public int num_pikmin;

    private const float EPISODE_LENGTH = 22f;

    float timer = 0f;

    public RetrievingManager()
    {
        agents = new List<GrabAgent>();
    }

	public void AddAgent(GrabAgent pik)
	{
		agents.Add(pik);
		StateMachine machine = pik.GetComponent<StateMachine>();

        machine.OnTransition += RemoveAgent;
	}

    public void RemoveAgent(Pikmin pik, State new_state)
    {
        if (new_state == State.COMBAT || new_state == State.IN_SQUAD)
        {
            var agent = pik.GetComponent<GrabAgent>();
            agents.Remove(agent);
            StateMachine machine = pik.GetComponent<StateMachine>();

            // careful if agent is destroyed
            machine.OnTransition -= RemoveAgent;
        }
    }

    private void Start()
    {
        if (!isTraining) { return; }

        ResetEpisode();
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

    public void ResetEpisode(bool reset_agents = true)
    {
        if (!isTraining) { return; }
        //agents.Clear();

        //Vector2 randomOffset = new Vector2(Random.Range(-7, 7), Random.Range(-7, 7));
        ////if(randomOffset.x <2 && randomOffset.x < -2) {randomOffset.x = 5f;}
        ////if(randomOffset.y <2 && randomOffset.y < -2) {randomOffset.y = 5f;}
        //PikminManager.instance.transform.position = new Vector3(-13 + randomOffset.x, 0, -7.5f + randomOffset.y);

        //foreach (Pikmin pik in PikminManager.instance.units)
        //{

        //    StateMachine machine = pik.GetComponent<StateMachine>();
        //    machine.OnChildTransitionEvent(State.IN_SQUAD);
        //    GrabAgent agent = pik.GetComponent<GrabAgent>();
        //    if (agent != null && reset_agents)
        //    {
        //        agent.EndEpisode();
        //        agents.Add(agent);
        //    }
        //}

        //if (isTraining)
        //{
        //    Vector2 randomOffset2 = new Vector2(Random.Range(-7, 7), Random.Range(-7, 7));
        //    pellet.transform.position = new Vector3(-13 + randomOffset2.x, 0, -7.5f + randomOffset2.y);
        //}

        //// reset if captain and pellet are too close
        //while (Vector3.Distance(PikminManager.instance.transform.position, pellet.transform.position) < 2f)
        //{
        //    Vector2 randomOffset2 = new Vector2(Random.Range(-7, 7), Random.Range(-7, 7));
        //    pellet.transform.position = new Vector3(-13 + randomOffset2.x, 0, -7.5f + randomOffset2.y);
        //}


        //StartCoroutine("ThrowPikmin");

    }

    //private IEnumerator ThrowPikmin()
    //{
    //    yield return new WaitForSeconds(4);

    //    foreach (Pikmin pik in PikminManager.instance.units)
    //    {
    //        PikminManager.instance.ThrowAt(pellet.transform.position);
    //        yield return new WaitForSeconds(Random.Range(0.05f, 0.3f));
    //    }


    //}

    public void Process()
    {
        float multiplier = 0f;
        if (pellet != null)
        {
            Renderer col = pellet.GetComponentInChildren<Renderer>();
            multiplier = col.bounds.size.x + col.bounds.size.z;
            multiplier *= 0.25f;
        }


        float radius = 1f * multiplier;

        for (int i = 0; i < agents.Count; ++i)
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

        if (!isTraining) { return; }

        timer += Time.deltaTime;

        int num_arrived = 0;
        foreach (GrabAgent agent in agents)
        {
            if (agent.arrived)
            {
                num_arrived++;
            }
        }
        if (num_arrived == agents.Count || timer > EPISODE_LENGTH)
        {
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
        return Random.Range(1, 10);
    }
}
