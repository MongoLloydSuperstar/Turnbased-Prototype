using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform pivot;
    
    [SerializeField] float targetDistance = 10.0f;
    private float currentDistance;
    
    [SerializeField] float cameraTiltVertical = 0.0f;
    [SerializeField] float cameraTiltHorizontal = 0.0f;
    private float cameraRoll = 0.0f;

    [SerializeField] float moveSpeed = 1.0f;
    [SerializeField] float distanceSpeed = 1.0f;
    [SerializeField] float cameraDistanceStep = 1.0f;


    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }


    private void Start()
    {
        pivot = transform.parent;
        transform.position = CameraStartPosition(targetDistance, pivot);
    }

    private void LateUpdate()
    {
        ChangeDistance();
        transform.position = CameraPositionLerp(targetDistance, pivot);
        transform.rotation = RotationCamera();        
    }


    private Vector3 CameraStartPosition(float distance, Transform lookAt)
    {
        transform.LookAt(lookAt);

        Vector3 direction = Vector3.back * distance;
        Quaternion rotation = transform.rotation;
        
        return lookAt.position + rotation * direction;
    }

    private Quaternion RotationCamera()
    {
        Vector3 finalRotation = transform.rotation.eulerAngles;

        finalRotation += new Vector3(cameraTiltVertical, cameraTiltHorizontal, cameraRoll);
        return Quaternion.Euler(finalRotation);
    }

    
    private void ChangeDistance()
    {
        float mouseScrollDelta = Input.GetAxis("Mouse ScrollWheel");

        if (mouseScrollDelta > 0f) {
            targetDistance -= cameraDistanceStep;
        }
        if (mouseScrollDelta < 0f) {
            targetDistance += cameraDistanceStep;
        }
    }

    private Vector3 CameraPositionLerp(float distance, Transform lookAt)
    {
        transform.LookAt(lookAt);

        Vector3 currentPosition = transform.position;
        Vector3 direction = Vector3.back * distance;
        Quaternion rotation = transform.rotation;

        Vector3 targetPosition = lookAt.position + rotation * direction;
        
        return Vector3.Lerp(currentPosition, targetPosition, distanceSpeed * Time.deltaTime);
    }

}
