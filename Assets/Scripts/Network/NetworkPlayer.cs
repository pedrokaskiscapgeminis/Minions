using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;

public class NetworkPlayer : NetworkBehaviour, IPlayerLeft
{
    public TextMeshProUGUI playerNicknameTM;
    public static NetworkPlayer Local { get; set; }

    public Transform playerModel;

    [Networked(OnChanged = nameof(OnNickNameChanged))]

    public NetworkString<_16> nickName { get; set; }

    bool isPublicJoinMessageSent = false;

    public LocalCameraHandler localCameraHandler;
    public GameObject localUI;

    NetworkInGameMessages networkInGameMessages;

    void Awake()
    {
        networkInGameMessages = GetComponent<NetworkInGameMessages>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Local = this;

            Utils.SetRenderLayerInChildren(playerModel, LayerMask.NameToLayer("LocalPlayerModel"));

            RPC_SetNickName(PlayerPrefs.GetString("PlayerNickname"));

            Debug.Log("Spawned local player");

            //Disable main camera
            Camera.main.gameObject.SetActive(false);   
        }
        else
        {
            Camera localCamera = GetComponent<Camera>();
            localCamera.enabled = false;

            AudioListener audioListener = GetComponentInChildren<AudioListener>();
            audioListener.enabled = false;

            localUI.SetActive(false);

            Debug.Log("Spawned remote player");
        }

        Runner.SetPlayerObject(Object.InputAuthority, Object);

        transform.name = $"P_{Object.Id}";

    }

    static void OnNickNameChanged(Changed<NetworkPlayer> changed)
    {
        Debug.Log($"{Time.time} OnHPChanged value {changed.Behaviour.nickName}");

        changed.Behaviour.OnNickNameChanged();
    }

    
    

    public void PlayerLeft(PlayerRef player)
    {
        if (Object.HasStateAuthority)
        {
            if (Runner.TryGetPlayerObject(player, out NetworkObject playerLeftNetworkObject))
            {
                if (playerLeftNetworkObject == Object)
                {
                    Local.GetComponent<NetworkInGameMessages>().SendInGameRPCMessage(playerLeftNetworkObject.GetComponent<NetworkPlayer>().nickName.ToString(), "left");
                }
            }
        }

        if (player == Object.InputAuthority)
        {
            Runner.Despawn(Object);
        }
    }

    private void OnNickNameChanged()
    {
        Debug.Log($"Nickname changed for player to {nickName} for player {gameObject.name}");

        playerNicknameTM.text = nickName.ToString();
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]

    public void RPC_SetNickName(string nickName, RpcInfo info = default)
    {
        Debug.Log($"[RPC] SetNickName{nickName}");
        this.nickName = nickName;

        if (!isPublicJoinMessageSent) 
        {
            networkInGameMessages.SendInGameRPCMessage(nickName, " joined");

            isPublicJoinMessageSent = true;
        }    

    }

}
