using System.Collections;
using System.Collections.Generic;

public class Pool : IPool
{
    public int QueueCount { get { return _Queue.Count; } }
    public IPoolable GetPoolable() 
    {
        return _Queue.Dequeue();
    }

    public void PlaceInQueue(IPoolable t)
    {
        _Queue.Enqueue(t);
    }

    private IPoolable[] _Pool;
    private Queue<IPoolable> _Queue;

    public Pool(IPoolable[] pool)
    {
        _Queue = new Queue<IPoolable>();
        _Pool = pool;

        for (int i = 0; i < _Pool.Length; i++)
        {
            _Pool[i].OnReturnRequest += PlaceInQueue;
            _Queue.Enqueue(_Pool[i]);
        }
    }

    ~Pool()
    {
        _Queue.Clear();
        for (int i = 0; i < _Pool.Length; i++)
        {
            _Pool[i].OnReturnRequest -= PlaceInQueue;
        }
    }
}
