using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState : IState {
   
    public BaseState( )
    {
        
    }

    public virtual void Enter()
    {
    }

    public virtual void Excute()
    {

    }

    public virtual void Exit()
    {
    }

    public virtual void Reset()
    {
        Debug.Log("重置");
    }

}
