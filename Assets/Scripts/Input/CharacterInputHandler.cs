using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputHandler : MonoBehaviour
{
    Vector2 moveInputVector = Vector2.zero;
    Vector2 viewInputVector = Vector2.zero;
    bool isJumpButtonPressed = false;

    LocalCameraHandler localCameraHandler;


    private void Awake()
    {
        localCameraHandler = GetComponentInChildren<LocalCameraHandler>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        viewInputVector.x = Input.GetAxis("Mouse X");
        viewInputVector.y = Input.GetAxis("Mouse Y") * -1;

        moveInputVector.x = Input.GetAxis("Horizontal");
        moveInputVector.y = Input.GetAxis("Vertical");

        //if (Input.GetButtonDown("Jump"))
        //{

        //    isJumpButtonPressed = true;
        //}

        localCameraHandler.SetViewInputVector(viewInputVector);
        
    }

    /// <summary>
    ///  qzijhbfpqidj///
    /// </summary>
    /// <returns></returns>

    public NetworkInputData GetNetworkInput()
    {

        NetworkInputData networkInputData = new NetworkInputData();

        networkInputData.aimForwardVector = localCameraHandler.transform.forward;

        networkInputData.movementInput = moveInputVector;

        networkInputData.isJumpPressed = isJumpButtonPressed;

        //Reset variables now we have read their states
        isJumpButtonPressed = false;
        
        return networkInputData;
    }

}
