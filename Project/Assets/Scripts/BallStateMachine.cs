using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallStateMachine : MonoBehaviour
{
    Vector3 OriginalBallPosition = new Vector3(30f, 0.5f, 15f);

    private enum State
    {
        Idle,
        Follow
    }

    private State _activeState = State.Idle;
    private Rigidbody _rb;
    private GameObject _scorer;

    public bool IsBallInPossession()
    {
        return (_activeState == State.Follow);
    }

    // Start is called before the first frame update
    void Start()
    {
        _scorer = GameManager.instance.GetScorer();
        _rb = GetComponent<Rigidbody>();
    }

    public void Reset()
    {
        _rb.rotation = Quaternion.Euler(Vector3.zero);
        _rb.velocity = Vector3.zero;
        transform.position = OriginalBallPosition;
        _activeState = State.Idle;
    }

    public void Transition()
    {
        float distance = Vector3.Distance(transform.position, _scorer.transform.position);
        Vector3 velocity = _rb.velocity;

        if (_activeState == State.Idle)
        {
            if (distance < 1.5f)
            {
                _activeState = State.Follow;
            }
        } else if (_activeState == State.Follow)
        {
            if (Vector3.Distance(velocity, Vector3.zero) > 1f)
                _activeState = State.Idle;
            /*if (distance > 2.75f)
                _activeState = State.Idle;*/
        }
    }

    public void Action()
    {
        Vector3 normalizedDirection = (transform.position - _scorer.transform.position).normalized;
        normalizedDirection.y = 0.5f;

        if (_activeState == State.Follow)
        {
            _rb.rotation = Quaternion.Euler(Vector3.zero);
            _rb.velocity = Vector3.zero;

            /*
            Vector3 frontPos = transform.position - (scorer.transform.forward * -4f);
            float angle = Vector3.Angle(transform.position, scorer.transform.position);
            float front = Mathf.Deg2Rad * (angle + 180f);

            float nX = transform.position.x + (5 * Mathf.Cos(front));
            float nZ = transform.position.z - (5 * Mathf.Sin(front));

            transform.position = new Vector3(nX, 0.5f, nZ);*/

            //_rb.AddForce(frontPos);

            //transform.position = frontPos;

            //Vector3 ballPos = scorer.transform.position + (normalizedDirection * 2f);

            //_rb.MovePosition(scorer.transform.position + (normalizedDirection * 3f));

            transform.position = (_scorer.transform.position + (_scorer.transform.forward * 2f));

            //_rb.AddForce(normalizedDirection * 20f);
        }
    }
}
