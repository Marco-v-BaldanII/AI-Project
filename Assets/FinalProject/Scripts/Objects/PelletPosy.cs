using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelletPosy : MonoBehaviour
{
    public GameObject pellet;
    public GameObject pelletCanvas;
    private void OnDisable()
    {
        pellet.transform.parent = this.transform.parent;
        pellet.AddComponent<Rigidbody>();
        pelletCanvas.SetActive(true);

        this.gameObject.transform.DOScale(new Vector3(.01f, .01f, .01f), 0.8f);



    }
}
