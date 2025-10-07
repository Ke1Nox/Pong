using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Collections.Generic;

public class ReadySystem : MonoBehaviourPunCallbacks
{
    public static ReadySystem Instance;

    [SerializeField] private int minPlayersReady = 2;
    [SerializeField] private TextMeshProUGUI logText;

    private HashSet<int> readyPlayers = new HashSet<int>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (logText != null)
        {
           
            logText.text = "Presiona ENTER para estar listo.\n";
        }
    }

    void Update()
    {
        //apretar enter para iniciar
        if (PhotonNetwork.InRoom && Input.GetKeyDown(KeyCode.Return))
        {
            if (!readyPlayers.Contains(PhotonNetwork.LocalPlayer.ActorNumber))
            {
                photonView.RPC(nameof(RPC_SetPlayerReady), RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.ActorNumber, PhotonNetwork.NickName);
            }
        }
    }


    //arrancar partida
    [PunRPC]
    void RPC_SetPlayerReady(int actorNumber, string nickname)
    {
        readyPlayers.Add(actorNumber);
        int totalPlayers = PhotonNetwork.CurrentRoom.PlayerCount;

        ShowLog($"{nickname} esta listo ({readyPlayers.Count}/{totalPlayers})");

        if (PhotonNetwork.IsMasterClient && readyPlayers.Count >= minPlayersReady)
        {
            photonView.RPC(nameof(RPC_StartGame), RpcTarget.AllBuffered);
        }
        else
        {
            int faltan = Mathf.Max(0, minPlayersReady - readyPlayers.Count);
            ShowLog($"Faltan {faltan} jugador(es) mas para iniciar.");
        }
    }

    //arranca el juego 
    [PunRPC]
    void RPC_StartGame()
    {
        ShowLog("La partida dio inicio!!!!!");   
    }
    //identacion de texto 
    private void ShowLog(string message)
    {
        Debug.Log(message);

        if (logText != null)
        {
            logText.text += "\n" + message;
        }
    }
}
