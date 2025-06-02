using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool<T> where T : Component
{
    Queue<T> _pool;
    int _count;
    Func<T> _createFunc;

    public int _Count {  get { return _pool.Count; } }

    public GameObjectPool() { }

    public GameObjectPool(int count, Func<T> createFunc)
    {
        _count = count;
        _createFunc = createFunc;
        _pool = new Queue<T>(count);
        SetQueue();
    }

    public T Get()
    {
        if (_pool.Count > 0)
        {
            return _pool.Dequeue();
        }
        else
        {
            return _createFunc();
        }
    }

    public void Set(T obj)
    {
        _pool.Enqueue(obj);
    }

    public T New()
    {
        return _createFunc();
    }

    void SetQueue()
    {
        for (int i = 0; i < _count; i++)
        {
            _pool.Enqueue(_createFunc());
        }
    }
}
