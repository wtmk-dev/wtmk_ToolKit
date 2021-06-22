using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField]
    private StartScreenView _ScreenView;
    [SerializeField]
    private GameScreenView _GameScreenView;
    [SerializeField]
    private HelpScreenView _HelpScreenView;
    [SerializeField]
    private CreditsScreenView _CreditsView;

    private IStateDirector[] _Directors = new IStateDirector[1];
    private GameScreenDirector _GameStateDirector;

    private Dood _Dood = Dood.Instance;

    void Awake()
    {
        BuildGameScreens();
    }

    void Start()
    {
        Dood.IsLogging = true;
        _GameStateDirector.IsActive = true;
    }

    void Update()
    {
        for(int elm = 0; elm < _Directors.Length; elm++)
        {
            _Directors[elm].OnUpdate();
        }
    }

    protected void BuildGameScreens()
    {
        StartScreen startScreen        = new StartScreen(_ScreenView);
        GameScreen gameScreen          = new GameScreen(_GameScreenView);
        HelpScreen helpScreen          = new HelpScreen(_HelpScreenView);
        CreditsScreen creditsScreen    = new CreditsScreen(_CreditsView);

        startScreen.ValidTransitions   = new string[] { gameScreen.Tag, helpScreen.Tag, creditsScreen.Tag };
        gameScreen.ValidTransitions    = new string[] { startScreen.Tag, helpScreen.Tag, creditsScreen.Tag };
        helpScreen.ValidTransitions    = new string[] { startScreen.Tag, gameScreen.Tag };
        creditsScreen.ValidTransitions = new string[] { startScreen.Tag, gameScreen.Tag };

        var gamestates                 = new IState[] { startScreen , gameScreen , helpScreen, creditsScreen };

        _GameStateDirector = new GameScreenDirector(gamestates);

        _Directors[0] = _GameStateDirector;
    }
}
