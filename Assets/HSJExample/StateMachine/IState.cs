using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState{
    void Enter();
    void Excute();
    void Reset();
    void Exit();
}
