using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    // POSITIONS AND VALUES
    Vector3 keeperPosition = new Vector3(5f, 0f, 15f);
    const float TrackDistance = 20f;
    const float TakeDistance = 10f;
    const float ReturnDistance = 1.5f;
    const float KickDistance = 0.2f;
    const float RetreatDistance = 15f;

    const float TrackSpeed = 10.0f;
    const float TakeSpeed = 7.5f;
    const float ReturnSpeed = 0.75f;
    const float RetreatSpeed = 1.25f;
    //

    public enum State
    {
        Idle,
        Take,
        Track,
        Retreat,
        Return,
        Kick
    }

    private State state = State.Idle;

    private BallStateMachine _ballState;
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
        _ballState = _ball.GetComponent<BallStateMachine>();

        _rb = _ball.GetComponent<Rigidbody>();
    }

    public void Reset()
    {
        transform.position = keeperPosition;
        state = State.Idle;
    }

    public void Transition()
    {
        float goalDistance = Vector3.Distance(transform.position, keeperPosition);
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
            else if (distance <= TakeDistance && !_ballState.IsBallInPossession())
            {
                state = State.Take;
            }
        }
        else if (state == State.Take)
        {
            if (Vector3.Distance(keeperPosition, transform.position) > RetreatDistance)
            {
                state = State.Retreat;
            } else
            {
                if (distance < ReturnDistance)
                {
                    state = State.Return;
                }
                else if (_ballState.IsBallInPossession())
                    state = State.Idle;
            }
        }
        else if (state == State.Return)
        {
            if (goalDistance <= KickDistance)
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
        else if (state == State.Retreat)
        {
            if (goalDistance <= KickDistance)
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
            case State.Retreat:
                transform.position = Vector3.Lerp(transform.position, keeperPosition, RetreatSpeed * Time.deltaTime);
                break;
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

                transform.position -= norm * (Time.deltaTime * TrackDistance);
                transform.position = Vector3.Lerp(transform.position, keeperPosition, ReturnSpeed * Time.deltaTime);
                break;
            case State.Kick:
                if (distance < 3f)
                    _rb.AddForce(new Vector3(30f, 0f, 0f));
                break;
        }
    }
}
