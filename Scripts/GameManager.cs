using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class GameManager : MonoBehaviourPun
{
    [Header("Players")]
    public string playerPrefabPath;
    public Transform[] spawnPoints;
    public float respawnTime;
    public PlayerController[] players;

    private int playersInGame;

    //instance
    public static GameManager instance;

    private void Awake()
    {
        //make proper singleton
        if (instance != null && instance != this)
            gameObject.SetActive(false);
        else
        {
            instance = this;
        }
    }

    [PunRPC]
    void ImInGame()
    {
        playersInGame++;
        if (playersInGame == PhotonNetwork.PlayerList.Length)
            SpawnPlayer();
    }

    private void Start()
    {
        photonView.RPC("ImInGame", RpcTarget.AllBuffered);
        players = new PlayerController[PhotonNetwork.PlayerList.Length];
    }

    void SpawnPlayer()
    {
        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefabPath, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);

        //initialize player
        playerObj.GetComponent<PhotonView>().RPC("Initialize", RpcTarget.All, PhotonNetwork.LocalPlayer);
    }
}
