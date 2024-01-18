using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace WTMK.Core
{
    public class Screen : MonoBehaviour
    {
        [SerializeField]
        protected Canvas _UI;
        [SerializeField]
        protected GameObject _Stage;

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

        protected GameData _GameData;
    }
}

