using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalCameraHandler : MonoBehaviour
{
    public Transform cameraAnchorPoint;

    Vector2 viewInput;

    //other components
    NetworkCharacterControllerPrototypeCustom networkCharacterControllerPrototypeCustom;
    Camera localCamera;

    float cameraRotationX = 0;
    float cameraRotationY = 0;

    private void Awake()
    {
        localCamera = GetComponent<Camera>();
        networkCharacterControllerPrototypeCustom = GetComponentInParent<NetworkCharacterControllerPrototypeCustom>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //detach camera
        if (localCamera.enabled)
        {
            localCamera.transform.parent = null;
        }
    }

    void LateUpdate()
    {
        if (cameraAnchorPoint == null)
        {
            return;
        }

        if (!localCamera.enabled)
        {
            return;
        }

        //Move the cam to the player pos
        localCamera.transform.position = cameraAnchorPoint.position;

        //Calculate Rotation
        cameraRotationX += viewInput.y * Time.deltaTime * networkCharacterControllerPrototypeCustom.viewUpDownRotationSpeed;
        cameraRotationX = Mathf.Clamp(cameraRotationX, -90, 90);

        cameraRotationY += viewInput.x * Time.deltaTime * networkCharacterControllerPrototypeCustom.rotationSpeed;

        localCamera.transform.rotation = Quaternion.Euler(cameraRotationX, cameraRotationY, 0);
    }

    public void SetViewInputVector(Vector2 viewInput)
    {
        this.viewInput = viewInput;
    }
}
