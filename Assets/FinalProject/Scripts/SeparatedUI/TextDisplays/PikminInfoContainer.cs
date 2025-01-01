using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PikminInfoContainer : MonoBehaviour
{

    public PikColor color;
    public int index;

	public bool showText;


	public TextMeshProUGUI label;
	// Start is called before the first frame update
	void OnEnable()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (label == null) { label = GetComponentInChildren<TextMeshProUGUI>(); }
		if (showText && label != null)
		{
			label.enabled = true;
			label.text = "x" + (PikminManager.instance.GetTypeAmount(color).amount)  .ToString();
		}
	}
}
