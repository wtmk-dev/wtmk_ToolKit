using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreenView : MonoBehaviour, IStateView
{
    [SerializeField]
    protected Button _Start;
    protected StartScreenEvent _Event = new StartScreenEvent();

    public Button bStart { get; private set; }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    private EventManager _EventManager = EventManager.Instance;

    void Awake()
    {
        bStart = _Start;
        //_Start.onClick.AddListener(TransitionStartScreen);
    }

    private void TransitionStartScreen()
    {
        /*
        _sButton.transform.DOScale(0f, 1.7f).SetEase(Ease.OutBounce);
        _Title.transform.DOLocalMoveY(1000f, 1.7f).SetEase(Ease.Linear);

        _Background.DOFade(0f, 2.7f).SetEase(Ease.InOutElastic).OnComplete(() =>
        {
            _EventManager.FireEvent(_Event.NewGame);
        }); ;
        */
    }
}
