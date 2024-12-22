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
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
	    if (num_pikmin >= weight && is_moving)
	    {
	    	Vector3 direction = goal.transform.position - transform.position; 
		    direction.Normalize();
		    transform.position +=  Time.deltaTime * direction * (speed * ( (float) num_pikmin/ (float) weight ) );
	    }
	    else{is_moving = false;}
    }
    
    
	// This should probably also become a state machine
    
	public void AddPikmin(Pikmin pikmin)
	{
		pikmin.transform.parent = transform;
		num_pikmin++;
		
		if(num_pikmin >= weight)
		{
			is_moving = true;
		}
	}
}
