using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]private float moveSpeed;
    [SerializeField]private GameInput gameInput;
    private bool isWalking;
    private void Update()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        //Debug.Log(inputVector);
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        isWalking = (moveDir != Vector3.zero);
        transform.position += (moveDir * moveSpeed * Time.deltaTime);
        float rotationSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir,Time.deltaTime * rotationSpeed);
    }

    public bool IsWalking()
    {
        return isWalking;
    }
}
