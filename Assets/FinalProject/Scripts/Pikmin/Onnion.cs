using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Onnion : MonoBehaviour
{
	public void Produce(uint amount)
	{
		

			PikminManager.instance.Spawn((int)amount, transform.position);
		
		
	}
}
