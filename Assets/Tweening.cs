using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweening : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DOMove(new Vector3(1, 2, 3), 1);

        transform.DOMoveY(4f,1f).SetLoops(-1, LoopType.Yoyo);

    }

    public void BounceDown()
    {
        //transform.DoTween(transform.position + )
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
