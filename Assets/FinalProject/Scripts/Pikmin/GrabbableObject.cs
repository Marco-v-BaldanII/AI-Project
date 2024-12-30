using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  GrabbableObject : MonoBehaviour
{
	
	public uint num_pikmin = 0;
	public uint weight = 1;
	
	public float speed = 5f;
	
	public Transform goal;
	
	private bool is_moving = false;

	HashSet<Pikmin> pikmins;
	
    // Start is called before the first frame update
    void Start()
    {
	    pikmins = new	HashSet<Pikmin>();
    }

    // Update is called once per frame
    void Update()
    {
		//TODO Change this to navmesh or something
	    if (num_pikmin >= weight && is_moving && EpisodeManager.Instance.isTraining == false)
	    {
	    	Vector3 direction = goal.transform.position - transform.position;
			direction.y = 0;
		    direction.Normalize();
		    transform.position +=  Time.deltaTime * direction * (speed * ( (float) num_pikmin/ (float) weight ) );
	    }
	    else{is_moving = false;}
	    
	    if (EpisodeManager.Instance.isTraining){
		    weight = (uint)EpisodeManager.Instance.RetrieveveWorkingPiks();
	    }
    }
    
    
	// This should probably also become a state machine
    
	public void AddPikmin(Pikmin pikmin)
	{
		if (EpisodeManager.Instance != null && ! EpisodeManager.Instance.isTraining) {
			pikmin.transform.parent = transform;
			pikmins.Add(pikmin);
		}
		num_pikmin++;
		
		if(num_pikmin >= weight)
		{
			is_moving = true;
		}
		
		
		pikmin.transform.DORotate( Quaternion.LookRotation(this.transform.position - pikmin.transform.position).eulerAngles, 0.3f );
		
		
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
            p.transform.parent = PikminManager.instance.floor.transform;

            StateMachine machine = p.GetComponent<StateMachine>();
            machine.OnChildTransitionEvent(State.IDLE);
        }
        pikmins.Clear();

	}

	private  void Free(Onnion onion)
	{
		onion?.Produce(weight); // Call the onnion to produce the necessary piks
        Destroy(gameObject);
    }
}
