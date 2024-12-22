using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whistle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {

            transform.position = hit.point; // Snap cursor to terrain
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.05f, transform.position.z);
        }
        
	    // TODO , change this to use new input system
	    if (Input.GetMouseButtonDown(0))
	    {
	    	foreach (var pikmin in PikminManager.instance.units)
	    	{
	    		StateMachine state_machine = pikmin.GetComponent<StateMachine>();
	    		if (state_machine.GetCurrentState() == State.IN_SQUAD)
	    		{
	    			state_machine.OnChildTransitionEvent(State.THROWN);
	    			break;
	    		}
	    	}
	    	
	    }
        
    }
    
    
}
