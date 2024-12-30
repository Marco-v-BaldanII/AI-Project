using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whistle : MonoBehaviour
{
	int layerMask; 
	
	public float whistleIncrementSpeed = 1;
	
	public Sprite aimSprite;
	public Sprite whistleSprite;
	
	private SpriteRenderer renderer;
	private Collider collider;
	
    // Start is called before the first frame update
    void Start()
    {
	    layerMask =  ~LayerMask.GetMask("Pikmin", "UI");
	    renderer = GetComponent<SpriteRenderer>();
	    collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	    if ( Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
        {

		    
        			    transform.position = hit.point; // Snap cursor to terrain
			    transform.position = new Vector3(transform.position.x, transform.position.y + 0.05f, transform.position.z);
		    
        }
        
	    // TODO , change this to use new input system
	    if (Input.GetMouseButtonDown(0))
	    {
	    	Throw();
	    	
	    }
	    if (Input.GetMouseButton(1))
	    {
	    	collider.enabled = true;
	    	float speed = whistleIncrementSpeed * Time.deltaTime;
	    	transform.localScale = new Vector3(transform.localScale.x + speed, transform.localScale.y + speed, transform.localScale.z + speed);
	    	renderer.sprite = whistleSprite;
	    }
	    else
	    {
	    	collider.enabled = false;
	    	renderer.sprite = aimSprite;
	    	transform.localScale = new Vector3(transform.localScale.x -0.1f, transform.localScale.y -0.1f, transform.localScale.z - 0.1f);
	    }
	    
	    transform.localScale = new Vector3(Mathf.Clamp(transform.localScale.x, 0.7f, 5), Mathf.Clamp(transform.localScale.x, 0.7f, 5), Mathf.Clamp(transform.localScale.z, 0.7f, 5));
        
    }
    
	public void Throw()
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
