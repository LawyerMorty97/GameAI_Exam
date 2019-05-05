using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityScript : MonoBehaviour
{
    Vector3 keeperPosition = new Vector3(55f, 0f, 15f);

    private float WalkSpeed = 15f;
    private float TurnSpeed = 240f;

    private Rigidbody rb;
    private LineRenderer _lines;

    private GameManager _game;
    private GameObject _ball;
    private BallStateMachine _ballMachine;
    private Rigidbody _brb;
    
    void Start()
    {
        print(transform.position);
        rb = gameObject.AddComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        _game = GameManager.GetInstance();

        _lines = gameObject.AddComponent<LineRenderer>();
        _lines.positionCount = 2;

        _ball = GameObject.FindGameObjectWithTag("Ball");
        _ballMachine = _ball.GetComponent<BallStateMachine>();
        _brb = _ball.GetComponent<Rigidbody>();
    }

    public void Reset()
    {
        transform.position = keeperPosition;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 270f, 0f));
    }

    void Update()
    {
        _lines.SetPosition(0, transform.position);
        _lines.SetPosition(1, (transform.position + (transform.forward * 4f)));

        if (_game.GetState() != GameManager.GameState.Running)
            return;

        float speed = WalkSpeed * Time.deltaTime;
        float rotSpeed = TurnSpeed * Time.deltaTime;
        float x = Input.GetAxis("Horizontal") * rotSpeed;
        float z = Input.GetAxis("Vertical") * speed;

        transform.Translate(0f, 0f, z);
        transform.Rotate(0f, x, 0f);

        if (_ballMachine.IsBallInPossession() && Input.GetButtonDown("Jump"))
        {
            Vector3 normalizedDirection = (_ball.transform.position - transform.position).normalized;
            _brb.AddForce(normalizedDirection * 250f);
        }
    }
}
