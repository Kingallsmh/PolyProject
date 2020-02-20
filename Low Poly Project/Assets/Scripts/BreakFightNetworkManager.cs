using Mirror;
using Mirror.Discovery;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakFightNetworkManager : NetworkManager
{
    public static BreakFightNetworkManager Instance;
    public Transform topSpawn;
    public Transform bottomSpawn;
    public GameObject ballPrefab;
    GameObject ball;

    public override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        Transform start = numPlayers == 0 ? bottomSpawn : topSpawn;
        //Rotate second player's camera
        GameObject player = Instantiate(playerPrefab, start.position, start.rotation);        
        NetworkServer.AddPlayerForConnection(conn, player);
        //player.GetComponent<BreakPlayer>().RpcInitPlayer();

        //When room has all players
        if (numPlayers == 2)
        {
            StartSession();
        }
    }

    //When a player leaves the session
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        //Reset/shutdown the session
        base.OnServerDisconnect(conn);
        if (NetworkServer.active)
        {
            StartCoroutine(WaitToStopHost());
        }
    }    

    IEnumerator WaitToStopHost()
    {
        yield return new WaitForSeconds(1);
        StopHost();
    }

    void StartSession()
    {
        GetComponent<NetworkDiscovery>().StopDiscovery();
        ball = Instantiate(ballPrefab);
        ball.GetComponent<BreakBall>().InitBall();
        NetworkServer.Spawn(ball);
        BrickManager.Instance.SetBrickDimensions(new Vector3(4, 1, 4));
        BrickManager.Instance.GeneratePlayerBricks(0, Color.green);
        BrickManager.Instance.GeneratePlayerBricks(1, Color.yellow);
    }

    
    void StopSession()
    {
        if (ball != null)
            NetworkServer.Destroy(ball);
    }

    //public override void OnServerAddPlayer(NetworkConnection conn)
    //{
    //    // add player at correct spawn position
    //    Transform start = numPlayers == 0 ? leftRacketSpawn : rightRacketSpawn;
    //    GameObject player = Instantiate(playerPrefab, start.position, start.rotation);
    //    NetworkServer.AddPlayerForConnection(conn, player);

    //    // spawn ball if two players
    //    if (numPlayers == 2)
    //    {
    //        ball = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "Ball"));
    //        NetworkServer.Spawn(ball);
    //    }
    //}

    //public override void OnServerDisconnect(NetworkConnection conn)
    //{
    //    // destroy ball
    //    if (ball != null)
    //        NetworkServer.Destroy(ball);

    //    // call base functionality (actually destroys the player)
    //    base.OnServerDisconnect(conn);
    //}
}
