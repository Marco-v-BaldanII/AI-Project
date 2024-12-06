using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update

    public BehaviorExecutor executor;

    void Start()
    {
        //AAA();
       // Invoke("AAA", 0.5f);
    }

    void AAA()
    {
        executor.blackboard.boolParams[0] = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
