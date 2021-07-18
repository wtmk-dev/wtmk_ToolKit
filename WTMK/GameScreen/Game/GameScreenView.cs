using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameScreenView : MonoBehaviour, IStateView
{
    public virtual void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
