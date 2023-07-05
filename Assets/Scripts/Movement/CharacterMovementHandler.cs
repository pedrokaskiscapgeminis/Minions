using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;


public class CharacterMovementHandler : NetworkBehaviour
{
    //Other components
    NetworkCharacterControllerPrototypeCustom networkCharacterControllerPrototypeCustom;
    Camera localCamera;
    NetworkInGameMessages networkInGameMessages;
    NetworkPlayer networkPlayer;

    private void Awake()
    {
        networkCharacterControllerPrototypeCustom = GetComponent<NetworkCharacterControllerPrototypeCustom>();
        localCamera = GetComponentInChildren<Camera>();
        networkInGameMessages = GetComponent<NetworkInGameMessages>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void FixedUpdateNetwork()
    {

        if (GetInput(out NetworkInputData networkInputData))
        {
            //rotate the transform according to the clinet aim vector
            transform.forward = networkInputData.aimForwardVector;

            //Cancel out rotation on X axis 
            Quaternion rotation = transform.rotation;
            rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.y, rotation.eulerAngles.z);

            //Move
            Vector3 moveDirection = transform.forward * networkInputData.movementInput.y + transform.right * networkInputData.movementInput.x;
            moveDirection.Normalize();

            networkCharacterControllerPrototypeCustom.Move(moveDirection);

            //Jump
            //if (networkInputData.isJumpPressed)
            //{
             //   Debug.Log("jump ici");
             //   networkCharacterControllerPrototypeCustom.Jump();
            //}
        }
    }

    void CheckFallRespawn()
    {
        if (transform.position.y < -12)
        {
            if (Object.HasStateAuthority)
            {
                Debug.Log($"{Time.time} Respawn due to fall outside of map at position {transform.position}");

                networkInGameMessages.SendInGameRPCMessage(networkPlayer.nickName.ToString(), "fell off the world");

                transform.position = Utils.GetRandomSpawnPoint();
            }
        }
    }
}
