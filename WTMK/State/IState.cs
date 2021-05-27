using System.Collections.Generic;

public interface IState 
{
    string Tag { get; }
    IState NextState { get; }
    IList<IState> ValidTransitions { get; set; }
    IStateView View { get; }
    void OnEnter();
    void OnExit();
    bool OnUpdate();
}
