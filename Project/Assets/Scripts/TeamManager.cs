using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public GameObject RedTeam;
    public GameObject BlueTeam;

    private bool _assigned = false;

    // Start is called before the first frame update
    void Start()
    {
        if (RedTeam == null || BlueTeam == null)
        {
            Debug.LogError("Teams not fully assigned!");
            return;
        }

        List<GameObject> players = new List<GameObject>();

        for (int i = 0; i < RedTeam.transform.childCount; i++)
        {
            GameObject player = RedTeam.transform.GetChild(i).gameObject;
            players.Add(player);
        }

        for (int i = 0; i < BlueTeam.transform.childCount; i++)
        {
            GameObject player = BlueTeam.transform.GetChild(i).gameObject;
            players.Add(player);
        }

        Debug.Log("There are " + players.Count.ToString() + " players total.");

        GameObject ball = GameManager.instance.Ball;
        Vector3 posBall = ball.transform.position;

        foreach(GameObject player in players)
        {
            //Vector3 position = player.transform.position;

            //Vector3 mul = Vector3.Scale(position, posBall);
            player.transform.LookAt(ball.transform);

            //player.AddComponent<VelocityScript>();
        }

        _assigned = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_assigned)
            return;
    }
}
