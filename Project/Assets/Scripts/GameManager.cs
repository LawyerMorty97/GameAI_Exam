using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum Team
    {
        Red,
        Blue
    };

    public struct Teammate
    {
        public GameObject player;
        public Team team;
    };

    public static GameManager instance = null;

    public GameObject Ball
    {
        get;
        private set;
    }

    public GameObject RedTeam;
    public GameObject BlueTeam;

    private List<Teammate> players;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        Ball = GameObject.FindGameObjectWithTag("Ball");
    }

    // Get a player's teammates
    private List<Teammate> GetTeamMembers(Teammate player)
    {
        List<Teammate> mates = new List<Teammate>();

        foreach (Teammate mate in players)
        {
            if (mate.team != player.team)
                mates.Add(mate);
        }

        mates.Remove(player);

        return mates;
    }

    // Start is called before the first frame update
    void Start()
    {
        players = new List<Teammate>();

        for (int i = 0; i < RedTeam.transform.childCount; i++)
        {
            GameObject player = RedTeam.transform.GetChild(i).gameObject;

            Teammate mate = new Teammate
            {
                player = player,
                team = Team.Red
            };

            players.Add(mate);
        }

        for (int i = 0; i < BlueTeam.transform.childCount; i++)
        {
            GameObject player = BlueTeam.transform.GetChild(i).gameObject;

            Teammate mate = new Teammate
            {
                player = player,
                team = Team.Blue
            };

            players.Add(mate);
        }

        foreach (Teammate mate in players)
        {
            Rigidbody rb = mate.player.AddComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        Debug.Log("There are " + players.Count.ToString() + " players total.");
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Teammate mate in players)
        {
            mate.player.transform.LookAt(Ball.transform);
            float mag = Vector3.Distance(mate.player.transform.position, Ball.transform.position);
            float percent = (mag / 25f) * 1;
            Color distColor = Color.Lerp(Color.red, Color.black, percent);

            Vector3 normed = (mate.player.transform.position + Ball.transform.position).normalized;
            mate.player.transform.position = Ball.transform.position + (normed * 5f);

            Debug.DrawLine(mate.player.transform.position, Ball.transform.position, distColor, 0);
        }
    }
}
