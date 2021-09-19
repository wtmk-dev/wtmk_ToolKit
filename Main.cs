using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class should be duplicated since everything in it will be customized per game
public class Main : MonoBehaviour
{
    // Game screen views
    [SerializeField]
    private StartScreenView _StartScreenView;
    [SerializeField]
    private GameScreenView _GameScreenView;
    [SerializeField]
    private HelpScreenView _HelpScreenView;
    [SerializeField]
    private CreditsScreenView _CreditScreenView;

    // Game screens
    private StartScreen _StartScreen;
    private GameScreen _GameScreen;
    private HelpScreen _HelpScreen;
    private CreditsScreen _CreditsScreen;

    private IStateDirector[] _Directors = new IStateDirector[1];
    private GameScreenDirector _GameScreenDirector;

    private Dood _Dood = Dood.Instance;

    private void Awake()
    {
        InitGameScreens(BuildGameScreens());
    }

    private void Start()
    {
        Dood.IsLogging = true;
        _GameScreenDirector.IsActive = true;
    }

    private void Update()
    {
        for (int elm = 0; elm < _Directors.Length; elm++)
        {
            _Directors[elm].OnUpdate();
        }
    }

    private IState[] BuildGameScreens()
    {
        _StartScreen        = new StartScreen(_StartScreenView);
        _GameScreen         = new GameScreen(_GameScreenView);
        _HelpScreen         = new HelpScreen(_HelpScreenView);
        _CreditsScreen      = new CreditsScreen(_CreditScreenView);

        _StartScreen.ValidTransitions   = new string[] { _GameScreen.Tag, _HelpScreen.Tag, _CreditsScreen.Tag };
        _GameScreen.ValidTransitions    = new string[] { _StartScreen.Tag, _HelpScreen.Tag, _CreditsScreen.Tag };
        _HelpScreen.ValidTransitions    = new string[] { _StartScreen.Tag, _GameScreen.Tag };
        _CreditsScreen.ValidTransitions = new string[] { _StartScreen.Tag, _GameScreen.Tag };

        IState[] gameStates = new IState[] { _StartScreen, _GameScreen, _HelpScreen, _CreditsScreen };
        
        return gameStates;
    }

    private void InitGameScreens(IState[] gameStates)
    {
        if(gameStates == null)
        {
            Debug.LogError("Error: Can't init game screens.");
        }

        _GameScreenDirector = new GameScreenDirector(gameStates);
        _Directors[0] = _GameScreenDirector;
    }
}
