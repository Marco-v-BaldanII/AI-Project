using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    GRAB,
    IDLE,
    COMBAT,
    CARRYING,
    IN_SQUAD,
    THROWN
}

public interface IState 
{
    void Enter();

    void Exit();

    void Process();

    void PhysicsProcess();
}
