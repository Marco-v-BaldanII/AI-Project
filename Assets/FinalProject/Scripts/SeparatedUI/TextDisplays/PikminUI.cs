using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PikminUI : MonoBehaviour
{
    private TextMeshProUGUI label;
    // Start is called before the first frame update
    void Awake()
    {
        label = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        label.text = "x" + PikminManager.instance.pikminInSquad.ToString();
    }
}
