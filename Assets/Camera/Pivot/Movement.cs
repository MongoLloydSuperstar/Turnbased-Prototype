using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class Movement : MonoBehaviour
{
    //Components
    private Animator animator;

    
    public float initialAngleY;

    [SerializeField] float rotationAmount = 90.0f;
    private int rotationLeftID, rotationRightID;
    private float FLOAT_TOLERANCE = 70f;

    //Using SmoothDampening
    [SerializeField] float smoothTime = 1.0f;
    [SerializeField] float maxSpeed = 1.0f;
    private float startAngleY;
    private float targetAngleY;

    //Using Slerp
    float moveIncrementSpeed = 1.0f;
    private Quaternion startRotation;
    private float moveIncrement;


    private void Start()
    {
        animator = GetComponent<Animator>();

        rotationLeftID = Animator.StringToHash("RotationLeft");
        rotationRightID = Animator.StringToHash("RotationRight");

        startRotation = transform.rotation;
        startAngleY = initialAngleY;
        targetAngleY = initialAngleY;
    }

    private void Update()
    {
        Move();
        RotateSmoothDampening();
    }


    private void RotateAnimation()
    {
        bool restState = animator.GetCurrentAnimatorStateInfo(0).IsTag("Resting");
        Transform pivotTransform = GetComponentInParent<Transform>();

        if (Input.GetKeyDown(KeyCode.Q) && restState) {
            animator.SetTrigger(rotationLeftID);

        }
        if (Input.GetKeyDown(KeyCode.E) && restState) {
            animator.SetTrigger(rotationRightID);
        }
    }

    private void RotateSlerp()
    {
        Quaternion currentRotation = transform.rotation;
        float currentRotationY = currentRotation.eulerAngles.y;
        float currentRotationX = currentRotation.eulerAngles.x;


        // Keeps the eulerAngles between 0 and 360
        if (currentRotationY >= 360) currentRotationY -= 360;
        else if (currentRotationY < 0) currentRotationY += 360;

        if (targetAngleY >= 360) targetAngleY -= 360;
        else if (targetAngleY < 0) targetAngleY += 360;


        if (Input.GetKeyDown(KeyCode.Q) && Mathf.Abs(currentRotationY - targetAngleY) <= FLOAT_TOLERANCE) {
            startRotation = transform.rotation;
            targetAngleY = (targetAngleY + rotationAmount);
            moveIncrement = 0.0f;
        }
        if (Input.GetKeyDown(KeyCode.E) && Mathf.Abs(currentRotationY - targetAngleY) <= FLOAT_TOLERANCE) {
            startRotation = transform.rotation;
            targetAngleY = (targetAngleY - rotationAmount);
            moveIncrement = 0.0f;
        }

        Vector3 targetVector3 = (Vector3.right * currentRotationX) + (Vector3.up * targetAngleY);
        Quaternion targetRotation = Quaternion.Euler(targetVector3);

        moveIncrement += moveIncrementSpeed * Time.deltaTime;

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, moveIncrement);
    }

    private void RotateSmoothDampening()
    {
        float currentRotationX = transform.rotation.eulerAngles.x;
        float currentRotationY = transform.rotation.eulerAngles.y;
        float currentRotationZ = transform.rotation.eulerAngles.z;

        if (Input.GetKeyDown(KeyCode.Q) && TestTolerence(currentRotationY, targetAngleY)) {
            startAngleY = transform.rotation.eulerAngles.y;
            targetAngleY = WrapDegrees(targetAngleY + rotationAmount);

            moveIncrement = 0.0f;
        }
        if (Input.GetKeyDown(KeyCode.E) && TestTolerence(currentRotationY, targetAngleY)) {
            startAngleY = transform.rotation.eulerAngles.y;
            targetAngleY = WrapDegrees(targetAngleY - rotationAmount);

            moveIncrement = 0.0f;
        }

        // If the statement is true it might not reach the target
        if (smoothTime * maxSpeed < rotationAmount + FLOAT_TOLERANCE) {
            maxSpeed = rotationAmount / smoothTime + FLOAT_TOLERANCE;
        }

        float velocity = 0.0f;

        float finalAngleY = Mathf.SmoothDampAngle(startAngleY, targetAngleY, ref velocity, smoothTime, maxSpeed, moveIncrement);
        moveIncrement += Time.deltaTime * 50.0f;

        transform.rotation = Quaternion.Euler(currentRotationX, finalAngleY, currentRotationZ);
    }


    private void RoundRotation()
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;
        float rotationY = currentRotation.y;
        float roundingFactor = 5f;

        rotationY = Mathf.Round(rotationY / roundingFactor) * roundingFactor;

        currentRotation.Set(currentRotation.x, rotationY, currentRotation.z);

        transform.rotation = Quaternion.Euler(currentRotation);
    }

    private void Move()
    {
        CameraController cameraController = GetComponentInChildren<CameraController>();

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float speed = cameraController.MoveSpeed;

        Vector3 forwardMovement = transform.forward * verticalInput;
        Vector3 sideMovement = transform.right * horizontalInput;

        transform.position += (forwardMovement + sideMovement) * speed;

    }


    private float WrapDegrees(float degrees)
    {
        if (degrees >= 360) degrees -= 360;
        else if (degrees < 0) degrees += 360;

        return degrees;
    }

    private bool TestTolerence(float a, float b)
    {
        float delta = Mathf.Abs(a - b);

        if (delta <= FLOAT_TOLERANCE && delta >= 0) return true;

        else if (delta >= -FLOAT_TOLERANCE && delta <= 0) return true;

        return false;
    }
}
