using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public enum UI_Element
    {
        StateViewer,
        ScoreRed,
        ScoreBlue,
        GoalBanner
    }

    public UI_Element textElement;
    private bool _active;
    private Text _text;

    private GameManager _game;
    private StateMachine _stateMachine;

    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<Text>();
        if (_text != null)
            _active = true;

        _stateMachine = FindObjectOfType<StateMachine>();
        _game = GameManager.instance;

        EventManager.StartListening("OnGoal", RunGoal);
        EventManager.StartListening("OnWin", RunWin);
    }

    void RunGoal()
    {
        if (textElement == UI_Element.GoalBanner)
            StartCoroutine(OnGoal());
    }

    void RunWin()
    {
        if (textElement == UI_Element.GoalBanner)
            StartCoroutine(OnWin());
    }

    IEnumerator OnGoal()
    {
        _text.text = "Goal!";
        yield return new WaitForSeconds(1f);
        _text.text = "5";
        yield return new WaitForSeconds(1f);
        _text.text = "4";
        yield return new WaitForSeconds(1f);
        _text.text = "3";
        yield return new WaitForSeconds(1f);
        _text.text = "2";
        yield return new WaitForSeconds(1f);
        _text.text = "1";
        yield return new WaitForSeconds(1f);
        _text.text = "Play!";
        yield return new WaitForSeconds(2f);
        _text.text = "";
    }

    IEnumerator OnWin()
    {
        _text.text = _game.GetWinner().ToString() + " Won The Game!";
        yield return new WaitForSeconds(2f);
        _text.text = "3";
        yield return new WaitForSeconds(1f);
        _text.text = "2";
        yield return new WaitForSeconds(1f);
        _text.text = "1";
        yield return new WaitForSeconds(2f);
        _text.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (!_active)
            return;

        switch(textElement)
        {
            case UI_Element.StateViewer:
                _text.text = "Current State: " + _stateMachine.GetState().ToString();
                break;
            case UI_Element.ScoreBlue:
                _text.text = _game.BlueScore.ToString();
                break;
            case UI_Element.ScoreRed:
                _text.text = _game.RedScore.ToString();
                break;
        }
    }
}
