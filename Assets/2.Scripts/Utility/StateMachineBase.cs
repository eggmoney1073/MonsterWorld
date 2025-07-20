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

    protected void SetState(Dictionary<T, StateFunction> storage, T state, StateFunction updateFunc)
    {
        if (storage.ContainsKey(state))
            storage.Remove(state);

        storage.Add(state, updateFunc);
    }

    protected void SetStateUpadte(T state, StateFunction updateFunc)
    {
        if (_stateUpdates.ContainsKey(state))
            _stateUpdates.Remove(state);

        _stateUpdates.Add(state, updateFunc);
    }

    protected void SetStateEnter(T state, StateFunction updateFunc)
    {
        if (_stateEnter.ContainsKey(state))
            _stateEnter.Remove(state);

        _stateEnter.Add(state, updateFunc);
    }

    protected void SetStateExit(T state, StateFunction updateFunc)
    {
        if (_stateExit.ContainsKey(state))
            _stateExit.Remove(state);

        _stateExit.Add(state, updateFunc);
    }

    protected virtual void Update()
    {
        if (_stateUpdates.ContainsKey(_currentState))
            _stateUpdates[_currentState]();
    }
}
