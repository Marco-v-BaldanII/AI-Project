using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface FSM_State
{
    void Move();


    void OnEnter();

    void OnExit();

}
