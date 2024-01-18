using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using WTMK.Core.Data;

namespace WTMK.Core
{
    public class Screen : MonoBehaviour, IState<GameScreen>
    {
        [SerializeField]
        protected Canvas _UI;
        [SerializeField]
        protected GameObject _Stage;

        public GameScreen Tag { get; protected set; }
        public GameScreen NextState { get; protected set; }
        public IList<GameScreen> ValidTransitions { get; protected set; }
        public IStateView View { get; protected set; }
        public bool Transition { get; protected set; }


        public virtual void DoUpdate()
        {

        }

        public virtual void Init()
        {

        }

        public virtual void Hide()
        {
            Toggle(false);
        }

        public virtual void Show()
        {
            Toggle(true);
        }

        public virtual void OnEnter()
        {
         
        }

        public virtual void OnExit()
        {
         
        }

        public virtual bool OnUpdate()
        {            
            return false;
        }

        protected void Toggle(bool isActive)
        {
            _UI.gameObject.SetActive(isActive);
            _Stage.SetActive(isActive);
        }

        public virtual void StartDelayFunctionCall(Action callBack, float time = 1f)
        {
            StartCoroutine(DelayBeforeCallBack(time, callBack));
        }

        private IEnumerator DelayBeforeCallBack(float waitTime, Action callBack)
        {
            yield return new WaitForSeconds(waitTime);
            callBack();
        }
    }
}

