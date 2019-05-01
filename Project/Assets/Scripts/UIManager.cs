using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public enum UI_Element
    {
        StateViewer
    }

    public UI_Element textElement;
    private bool _active;
    private Text _text;

    private StateMachine _stateMachine;

    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<Text>();
        if (_text != null)
            _active = true;

        _stateMachine = FindObjectOfType<StateMachine>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(textElement)
        {
            case UI_Element.StateViewer:
                _text.text = "Current State: " + _stateMachine.GetState().ToString();
                break;
        }
    }
}
