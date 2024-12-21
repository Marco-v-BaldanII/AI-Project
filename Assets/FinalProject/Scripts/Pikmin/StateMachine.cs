using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{

    [SerializeField] State[] stateTypes;

    private IState[] states;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var state in stateTypes)
        {
            switch (state)
            {

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
