using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurtPart : MonoBehaviour
{
	public DefeatableEntity enemy;
	
	public void OnTriggerEnter(Collider other)
	{
		// The enemies specific parts (eyes / legs) should notify the parent enemy component
		enemy.OnTriggerEnter(other);
	}
    
}
