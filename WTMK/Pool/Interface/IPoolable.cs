using System.Collections;
using System.Collections.Generic;
using System;

public interface IPoolable 
{
    public event Action<IPoolable> OnReturnRequest;
}
