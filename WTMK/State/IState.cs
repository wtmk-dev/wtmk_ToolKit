using System.Collections.Generic;

public interface IState 
{
    string Tag { get; }
    string NextState { get; }
    IList<string> ValidTransitions { get; set; }
    IStateView View { get; }
    void OnEnter();
    void OnExit();
    bool OnUpdate();
}
