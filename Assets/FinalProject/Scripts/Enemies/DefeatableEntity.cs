using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefeatableEntity : MonoBehaviour
{
	
	public Image img;
	private  float hp;
	public float maxHP;
	public float timeToDestroy;
	private Animator animator = null;
	public Component[] components_to_disable;
	private GrabbableObject grab_object;

	public bool destroyObject = false;
	
	private void Awake()
	{
		hp = maxHP;
		grab_object = GetComponent<GrabbableObject>();
		if (!grab_object) { grab_object = GetComponentInChildren<GrabbableObject>(); }
		animator = GetComponent<Animator>();
	}

	public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PikminAttack")
        {
	        hp--;
	        if (animator) animator?.SetFloat("hp", hp);
	        print("Enemy hurt, hp is now " + hp);
	        
	        img.fillAmount = (1f / maxHP) * hp;
	        
	        if (hp <= 0)
	        {
		        grab_object.enabled = true;
		        
		        foreach (Component component in components_to_disable)
		        {
		        	Destroy(component);
		        }
		        
		        Collider[] colliders = GetComponentsInChildren<Collider>();
		        
		        foreach (Collider col in colliders)
		        {
		         if(col.gameObject != this.gameObject)	{Destroy(col);}
		        }
		        
		        // Change tag so can be grabbed
		        grab_object.gameObject.tag = "Pellet";

				if (!destroyObject) { Destroy(this, timeToDestroy); }
				else { Destroy(this.gameObject, timeToDestroy); }
	        }
            
        }
    }
    
    

}
