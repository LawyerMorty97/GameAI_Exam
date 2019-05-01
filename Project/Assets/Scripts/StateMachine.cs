using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    // POSITIONS AND VALUES
    Vector3 keeperPosition = new Vector3(5f, 0f, 15f);
    const float TrackDistance = 20f;
    const float TakeDistance = 10f;
    const float ReturnDistance = 1f;
    const float KickDistance = 0.1f;

    const float TrackSpeed = 15.0f;
    const float TakeSpeed = 7.5f;
    const float ReturnSpeed = 1.5f;
    //

    public enum State
    {
        Idle,
        Take,
        Track,
        Return,
        Kick
    }

    private State state = State.Idle;

    private GameObject _ball;
    private Rigidbody _rb;

    public State GetState()
    {
        return state;
    }

    // Start is called before the first frame update
    void Start()
    {
        _ball = GameObject.FindGameObjectWithTag("Ball");
        _rb = _ball.GetComponent<Rigidbody>();
    }

    public void Transition()
    {
        float distance = Vector3.Distance(transform.position, _ball.transform.position);

        if (state == State.Idle)
        {
            //if (distance <= 20f && state != State.Take)
            if (distance <= TrackDistance)
            {
                state = State.Track;
            }
            else if (distance <= TakeDistance)
            {
                state = State.Take;
            }
        }
        else if (state == State.Track)
        {
            if (distance > TrackDistance)
            {
                state = State.Idle;
            }
            else if (distance <= TakeDistance)
            {
                state = State.Take;
            }
        }
        else if (state == State.Take)
        {
            if (distance < ReturnDistance)
            {
                state = State.Return;
            }
        }
        else if (state == State.Return)
        {
            float distance_accuracy = Vector3.Distance(transform.position, keeperPosition);
            if (distance_accuracy <= KickDistance)
            {
                state = State.Kick;
            }
        }
        else if (state == State.Kick)
        {
            if (distance >= TakeDistance)
            {
                state = State.Idle;
            }
        }
    }

    public void Action()
    {
        float distance = Vector3.Distance(transform.position, _ball.transform.position);
        Vector3 norm = (transform.position - _ball.transform.position).normalized;

        switch(state)
        {
            case State.Return:
                _rb.velocity = Vector3.zero;
                _rb.rotation = Quaternion.Euler(Vector3.zero);
                _ball.transform.position = transform.position + new Vector3(1f, 0f, 0f);

                transform.position = Vector3.Lerp(transform.position, keeperPosition, ReturnSpeed * Time.deltaTime);
                break;
            case State.Take:
                transform.position -= norm * (Time.deltaTime * TakeSpeed);
                break;
            case State.Track:
                norm.x = 0f;
                transform.position -= norm * (Time.deltaTime * TrackSpeed);
                break;
            case State.Kick:
                if (distance < 3f)
                    _rb.AddForce(new Vector3(10f, 0f, 0f));
                break;
        }
    }
}
