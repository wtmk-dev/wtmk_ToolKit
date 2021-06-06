using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreenView : MonoBehaviour, IStateView
{
    [SerializeField]
    private Button _Start;
    private StartScreenEvent _Event = new StartScreenEvent();

    public Button bStart { get; private set; }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    private EventManager _EventManager = EventManager.Instance;

    void Awake()
    {
        bStart = _Start;
        _Start.onClick.AddListener(() => { _EventManager.FireEvent(_Event.NewGame); });
    }
}
