using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateFactory{

    public static BaseState CreateState(int state)
    {
        BaseState product = null;
        switch (state)
        {
            case StateType.IDLE:
                {
                    product = new IdleState();
                    break;
                }
            case StateType.MOVE:
                {
                    product = new MoveState();
                    break;
                }
            case StateType.ATTACK:
                {
                    product = new AttackState();
                    break;
                }
            case StateType.RUNAWAY:
                {
                    product = new RunState();
                    break;
                }

            default:
                {
                    Debug.LogWarning("not exist state:" + state);
                    break;
                }
        }
        return product;
    }
}
