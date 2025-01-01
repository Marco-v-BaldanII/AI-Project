using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class WeightIcon : MonoBehaviour
{
	public TextMeshProUGUI text;
	private GrabbableObject grab_object;
	
    // Start is called before the first frame update
	void Awake()
    {
	    text = GetComponent<TextMeshProUGUI>();
	    grab_object = GetComponentInParent<GrabbableObject>();
	    
    }

    // Update is called once per frame
    void Update()
    {
		if (text )text.text = grab_object.num_pikmin.ToString() + "\n" + "_\n\n\n" + grab_object.weight.ToString();
    }
}
