using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private enum State
    {
        Idle,
        ChaseBall,
        TrackBall, // Keeper State
    }

    private State state = State.Idle;

    public GameObject ball;

    // Start is called before the first frame update
    void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
    }

    public void Transition()
    {
        Vector3 heading = ball.transform.position - transform.position;
        float ballDist = heading.magnitude;

        if (tag != "Keeper")
        {
            if (state == State.Idle)
            {
                if (ballDist > 2.0f)
                {
                    state = State.ChaseBall;
                }
            }
            else if (state == State.ChaseBall)
            {
                if (ballDist < 2.0f)
                {
                    state = State.Idle;
                }
            }
        } else
        {
            if (state == State.Idle)
            {
                if (ballDist < 2f)
                {
                    state = State.TrackBall;
                }
            }
            else if (state == State.TrackBall)
            {
                if (ballDist < 2.0f)
                {
                    state = State.Idle;
                }
            }
        }
    }

    public void Action()
    {
        Vector3 directionNormalized = (transform.position - ball.transform.position).normalized;

        switch(state)
        {
            case State.ChaseBall:
                //transform.position -= directionNormalized * (Time.deltaTime * 5f);
                break;
            case State.TrackBall:
                Vector3 keeper_position = new Vector3(transform.position.x, transform.position.y, transform.position.z + directionNormalized.z);
                transform.position = keeper_position * (Time.deltaTime * 5f);
                break;
        }
    }
}
