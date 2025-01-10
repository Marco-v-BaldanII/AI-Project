using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class  GrabbableObject : MonoBehaviour
{
	
	public uint num_pikmin = 0;
	public uint weight = 1;
	
	public float speed = 5f;
	
	public Transform goal;
	
	private bool is_moving = false;
	
	public float GrabRadius = 3f;

	HashSet<Pikmin> pikmins;

	NavMeshAgent agent;
	
    // Start is called before the first frame update
    void Start()
    {
	    pikmins = new	HashSet<Pikmin>();
		agent = GetComponent<NavMeshAgent>();
		if (goal == null) { goal = GameObject.Find("Onion").transform; }
    }

    // Update is called once per frame
    void Update()
    {
		//TODO Change this to navmesh or something
	    if (num_pikmin >= weight && is_moving && ThrowManager.Instance.isTraining == false)
	    {

		    if (agent){

			agent.destination = goal.transform.position;
		    }
		    else
		    {
		    	
		    
	           Vector3 direction = goal.transform.position - transform.position;
			   direction.y = 0;
		       direction.Normalize();
			   transform.position +=  Time.deltaTime * direction * (speed * ( (float) num_pikmin/ (float) weight ) );
		    }
	    }
	    else{is_moving = false;}
	    
	    //if (ThrowManager.Instance.isTraining){
		//    weight = (uint)EpisodeManager.Instance.RetrieveveWorkingPiks();
	    //}
    }
    
    
	// This should probably also become a state machine
    
	public void AddPikmin(Pikmin pikmin)
	{
		if (ThrowManager.Instance != null && ! ThrowManager.Instance.isTraining) {
			if (pikmins == null) { pikmins = new HashSet<Pikmin>(); }
			pikmin.transform.parent = transform;
			pikmins.Add(pikmin);
		}
		num_pikmin++;
		
		StateMachine machine = pikmin.GetComponent<StateMachine>();
		machine.OnTransition += RemovePikmin;
		
		if(num_pikmin >= weight)
		{
			is_moving = true;
		}
		
		
		pikmin.transform.DORotate( Quaternion.LookRotation(this.transform.position - pikmin.transform.position).eulerAngles, 0.3f );
		
		if(Vector3.Distance(pikmin.transform.position, this.transform.position) > GrabRadius)
		{
			//while (Vector3.Distance(pikmin.transform.position, this.transform.position) > GrabRadius)
			//{
			//	pikmin.transform.position += (this.transform.position - pikmin.transform.position).normalized;

   //         }

		}
	}
	
	void RemovePikmin(Pikmin pik, State newState)
	{
		if (newState == State.IN_SQUAD)
		{

            StateMachine machine = pik.GetComponent<StateMachine>();
            machine.OnTransition -= RemovePikmin;

            num_pikmin--;
			if(pikmins.Contains(pik))
			{
				pikmins.Remove(pik);
			}
			if(num_pikmin >= weight)
			{
				is_moving = true;
			}
			else{is_moving = false;}
		
		}
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Onnion")
		{
	        FreePikmin();
			
	        Onnion onnion = other.gameObject.GetComponent<Onnion>();
			
			
			
	        transform.DOMoveY(10, 0.8f, false).OnComplete(() =>  Free(onnion));
			transform.DOScale(0.001f, 0.8f);
			// Free pikmin after tweening has finished
		}
    }
	
	private void FreePikmin()
	{
        foreach (Pikmin p in pikmins)
        {
			if (p)
			{
				p.transform.parent = PikminManager.instance.floor.transform;

				StateMachine machine = p.GetComponent<StateMachine>();
				machine.OnChildTransitionEvent(State.IDLE);
			}
        }
        pikmins.Clear();

	}

	private  void Free(Onnion onion)
	{
		onion?.Produce(weight); // Call the onnion to produce the necessary piks
        Destroy(gameObject);
    }
}
