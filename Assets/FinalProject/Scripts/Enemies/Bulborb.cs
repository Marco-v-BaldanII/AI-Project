using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bulborb : MonoBehaviour
{
	
	public Image img;
	private  float hp;
	public float maxHP;
	private Animator animator;
	public Component[] components_to_disable;
	private GrabbableObject grab_object;
	
	private void Awake()
	{
		hp = maxHP;
		grab_object = GetComponent<GrabbableObject>();
		animator = GetComponent<Animator>();
	}

	public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PikminAttack")
        {
	        hp--;
	        animator.SetFloat("hp", hp);
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
		        this.gameObject.tag = "Pellet";
		        
		        Destroy(this);
	        }
            
        }
    }
    
    

}
