using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Paused,
        Keeping,
        Scoring
    }
    public static GameManager instance = null;

    private GameObject computer;
    private GameObject player;

    public GameObject Ball
    {
        get;
        private set;
    }

    private GameObject scorer_;
    private StateMachine _machine;
    private BallStateMachine _ballMachine;

    public GameObject GetScorer()
    {
        return scorer_;
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        Ball = GameObject.FindGameObjectWithTag("Ball");

        _machine = FindObjectOfType<StateMachine>();
        computer = GameObject.FindGameObjectWithTag("Computer");
        player = GameObject.FindGameObjectWithTag("Player");
        scorer_ = player;

        _ballMachine = Ball.GetComponent<BallStateMachine>();
    }

    // Update is called once per frame
    void Update()
    {
        _ballMachine.Transition();
        _ballMachine.Action();
        //_machine.Transition();
        //_machine.Action();
    }
}
