using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void CallBack();

public class StateMachine : MonoBehaviour {
    public static StateMachine Instance;
    //protected PlayerController _player;
    public IState _currentState;
    public int _currentStateType;
    protected Dictionary<int,IState> _statePool;
    private void Awake()
    {
        Instance = this;
        //_player = gameObject.GetComponent<PlayerController>();
        _statePool = new Dictionary<int, IState>();
    }
    public virtual void ChangeState(int stateType, CallBack callBack)
    {
        //if (_player == null)
        //{
        //    Debug.LogWarning("Can't find owner for this StateMachine");return;
        //}
        if (_currentStateType == stateType)
        {
            if (_currentState != null)
            {
                _currentState.Reset();
                return;
            }
        }
        if (_currentState!=null)
        {
            _currentState.Exit();
        }
        if (_statePool.ContainsKey(stateType))
        {
            _currentState = _statePool[stateType];
        }
        else
        {
            _currentState = StateFactory.CreateState(stateType);
            _statePool[stateType] = _currentState;
        }
        _currentStateType = stateType;
        Debug.Log(_currentStateType);   
        if (callBack != null)
        {
            callBack();
        }
        //_currentState.Enter();
        //_currentState.Excute();
    }
    //protected void Update()
    //{
    //    if (_currentState != null)
    //    {
    //        _currentState.excute();
    //    }
    //}
}
