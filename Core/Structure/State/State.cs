using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WTMK.Core
{
    public class State<T> : IState<T>
    {
        public virtual T Tag { get; protected set; }
        public virtual T NextState { get; protected set; }
        public virtual IList<T> ValidTransitions { get; protected set; }
        public virtual IStateView View { get; }
        public virtual bool Transition { get { return _Ready; } }   

        public virtual void OnEnter() 
        {
            Debug.LogWarning($"On_Enter {Tag}");
        }

        public virtual void OnExit()
        {
            _Ready = false;
        }

        public virtual bool OnUpdate() 
        { 
            return _Ready; 
        }

        public virtual void OnFixedUpdate()
        {

        }

        protected bool _Ready;
        protected IStateView _View;

        public State(T tag)
        {
            Tag = tag;
        }

        public State(T tag, IList<T> validTransitions)
        {
            Tag = tag;
            ValidTransitions = validTransitions;
        }

        protected void Transation(T state)
        {
            NextState = state;
            _Ready = true;
        }
    }
}
