using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    const int MaxScore = 5;
    public enum Team
    {
        Red,
        Blue
    }

    public enum GameState
    {
        Running,
        Paused,
        Intermission,    // Intermission is used when a score occurs
        Drawing,        // Game State to draw pathfinding
    }

    public static GameManager instance = null;

    public GameObject Ball
    {
        get;
        private set;
    }

    public int RedScore
    {
        get;
        private set;
    }

    public int BlueScore
    {
        get;
        private set;
    }

    private Grid _grid;
    private GameObject _scorer;
    private StateMachine _machine;
    private BallStateMachine _ballMachine;
    private VelocityScript _playerMachine;
    private PathingManager _pather;
    private UIManager _ui;

    private GameState _gameState;

    public GameState GetState()
    {
        return _gameState;
    }

    public GameObject GetScorer()
    {
        return _scorer;
    }

    public void AddScore(Team team)
    {
        switch(team)
        {
            case Team.Blue:
                BlueScore += 1;
                break;
            case Team.Red:
                RedScore += 1;
                break;
        }

        StartCoroutine(ResetGame());
    }

    public Team GetWinner()
    {
        Team winner;

        if (RedScore > BlueScore)
            winner = Team.Red;
        else
            winner = Team.Blue;

        return winner;
    }

    private IEnumerator ResetGame()
    {
        Debug.Log("Score is " + RedScore + " - " + BlueScore);
        _gameState = GameState.Intermission;

        bool isWin = false;
        if (RedScore == MaxScore || BlueScore == MaxScore)
        {
            EventManager.TriggerEvent("OnWin");
            isWin = true;
        }
        else
        {
            EventManager.TriggerEvent("OnGoal");
        }

        yield return new WaitForSeconds(5f);
        _ballMachine.Reset();
        _playerMachine.Reset();
        _machine.Reset();
        if (isWin)
        {
            RedScore = 0;
            BlueScore = 0;
            _gameState = GameState.Drawing;
        }
        yield return new WaitForSeconds(1f);
        _gameState = GameState.Running;
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        Ball = GameObject.FindGameObjectWithTag("Ball");

        _machine = FindObjectOfType<StateMachine>();
        _scorer = GameObject.FindGameObjectWithTag("Player");

        _ballMachine = Ball.GetComponent<BallStateMachine>();
        _gameState = GameState.Drawing;

        _playerMachine = _scorer.GetComponent<VelocityScript>();

        _pather = GetComponent<PathingManager>();
        _grid = Grid.GetInstance();
        //_pather.StartDrawing();
    }

    public void DrawingComplete()
    {
        _gameState = GameState.Running;
        _grid.HideGrid(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameState == GameState.Running)
        {
            _ballMachine.Transition();
            _ballMachine.Action();

            _machine.Transition();
            _machine.Action();
        }
    }
}
