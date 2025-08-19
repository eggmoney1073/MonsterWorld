using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineBase<T> : MonoBehaviour
{
    protected delegate void StateFunction();

    protected Dictionary<T, StateFunction> _stateEnter;
    protected Dictionary<T, StateFunction> _stateUpdates;
    protected Dictionary<T, StateFunction> _stateExit;

    public T _currentState;
    protected T _prevState;

    protected void InitStateMachine()
    {
        _stateEnter = new Dictionary<T, StateFunction>();
        _stateUpdates = new Dictionary<T, StateFunction>();
        _stateExit = new Dictionary<T, StateFunction>();
    }

    protected virtual void ChangeState(T state)
    {
        _prevState = _currentState;
        _currentState = state;

        if (_stateExit.ContainsKey(_prevState))
            _stateExit[_prevState]();

        if (_stateEnter.ContainsKey(_currentState))
            _stateEnter[_currentState]();
    }

    protected void SetState(Dictionary<T, StateFunction> stateStorage, T state, StateFunction stateFunc)
    {
        if (stateStorage.ContainsKey(state))
            stateStorage.Remove(state);

        stateStorage.Add(state, stateFunc);
    }

    protected virtual void Update()
    {
        if (_stateUpdates.ContainsKey(_currentState))
            _stateUpdates[_currentState]();
    }
}
