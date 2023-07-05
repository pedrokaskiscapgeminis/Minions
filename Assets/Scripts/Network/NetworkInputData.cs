using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Fusion;

public struct NetworkInputData : INetworkInput
{
    public Vector2 movementInput;
    public Vector3 aimForwardVector;
    public NetworkBool isJumpPressed;
}
