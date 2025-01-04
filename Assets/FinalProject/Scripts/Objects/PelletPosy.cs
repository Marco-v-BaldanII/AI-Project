using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelletPosy : MonoBehaviour
{
    public GameObject pellet;
    public GameObject pelletCanvas;

    DefeatableEntity flower;
 

    private void PopPosy()
    {
        pellet.transform.parent = this.transform.parent;
        pellet.AddComponent<Rigidbody>();
        pelletCanvas.SetActive(true);

        this.gameObject.transform.DOScale(new Vector3(.01f, .01f, .01f), 0.8f);

    }

    private void OnDestroy()
    {
        // unsubscribe
        if (flower) { flower.OnDefeated -= PopPosy; }
    }

    private void Start()
    {

        flower = GetComponent<DefeatableEntity>();
        if (flower) { flower.OnDefeated += PopPosy; }

        scaleUp();
    }

    public void scaleUp()
    {
        transform.DOScaleY(1.08f, 1.4f).OnComplete(scaleDown);
    }

    public void scaleDown()
    {
        transform.DOScaleY(0.92f, 1.4f).OnComplete(scaleUp);
    }
}
