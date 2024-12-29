using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bulborb : MonoBehaviour
{
	
	public Image img;
	private  float hp;
	public float maxHP;
	
	private void Awake()
	{
		hp = maxHP;
	}

	public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PikminAttack")
        {
	        hp -= 1;
	        
	        img.fillAmount = (1f / maxHP) * hp;
	        
	        if (hp <= 0)
	        {
	        	Destroy(this.gameObject);
	        }
            
        }
    }
    
    

}
