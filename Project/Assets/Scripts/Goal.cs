using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public GameManager.Team teamPost;
    private GameManager _game;
    
    void Start()
    {
        _game = GameManager.instance;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Ball")
        {
            _game.AddScore(teamPost);
        }
    }

    void Update()
    {

    }
}
