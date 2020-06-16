using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StateType : MonoBehaviour
{
    //初始
    public const int DEFULT = 0;
    //待机
    public const int IDLE = 1;
    //移动
    public const int MOVE = 2;
    //跑开
    public const int RUNAWAY = 3;
    //攻击
    public const int ATTACK = 4;
}
public delegate void CallBack();

public class StateManager : BaseManager<StateManager> {
    public IState _currentState;
    public int _currentStateType;
    protected Dictionary<int,IState> _statePool;
    protected override void Awake()
    {
        base.Awake();
        _statePool = new Dictionary<int, IState>();
        ChangeState(StateType.IDLE);

    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (_currentStateType + 1>4)
            {
                _currentStateType = 0;
            }
            ChangeState(_currentStateType+1);
        }
    }
    public virtual void ChangeState(int stateType, CallBack callBack=null)
    {
        if (_currentState != null)
        {
            if (_currentStateType == stateType)
            {
                _currentState.Reset();
                return;
            }
            else
            {
                _currentState.Exit();
            }
        }
        if (_statePool.ContainsKey(stateType))
        {
            _currentState = _statePool[stateType];
        }
        else
        {
            _currentState = CreateState(stateType);
            _statePool[stateType] = _currentState;
        }
        _currentStateType = stateType;
        Debug.Log(_currentStateType);
        _currentState.Enter();
        _currentState.Excute();
        callBack?.Invoke();
    }

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
