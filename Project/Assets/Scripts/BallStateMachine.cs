using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallStateMachine : MonoBehaviour
{
    private enum State
    {
        Idle,
        Follow
    }

    private State _activeState = State.Idle;
    private Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Transition()
    {
        GameObject scorer = GameManager.instance.GetScorer();

        float distance = Vector3.Distance(transform.position, scorer.transform.position);

        if (_activeState == State.Idle)
        {
            if (distance < 1.5f)
            {
                _activeState = State.Follow;
            }
        } else if (_activeState == State.Follow)
        {
            if (distance > 1.5f)
                _activeState = State.Idle;
        }
    }

    public void Action()
    {
        GameObject scorer = GameManager.instance.GetScorer();
        Vector3 normalizedDirection = (transform.position - scorer.transform.position).normalized;
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
            _rb.AddForce(normalizedDirection * 100f);
        }
    }
}
