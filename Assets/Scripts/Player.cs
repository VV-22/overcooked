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
        Vector3 originalMoveDir = moveDir;
        float playerRadius = 0.65f;
        float playerHeight = 2f;
        float moveDistance = moveSpeed * Time.deltaTime;
        bool canMove = !(Physics.CapsuleCast(transform.position,transform.position + Vector3.up * playerHeight,playerRadius,moveDir,moveDistance));

        if(!canMove)
        {
            //attempt only X dir
            Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f).normalized;
            canMove = !(Physics.CapsuleCast(transform.position,transform.position + Vector3.up * playerHeight,playerRadius,moveDirX,moveDistance));
            if(canMove)
            {
                //can move in X dir
                moveDir = moveDirX;
            }
            else
            {
                //attempt only Z dir
                Vector3 moveDirZ = new Vector3(0f, 0f, moveDir.z).normalized;
                canMove = !(Physics.CapsuleCast(transform.position,transform.position + Vector3.up * playerHeight,playerRadius,moveDirZ,moveDistance));
                if(canMove)
                {
                    //can move in Z dir
                    moveDir = moveDirZ;
                }
                else
                {
                    //cannot move in any direction
                }
            }
        }
        if(canMove)
            transform.position += (moveDir * moveDistance);
        isWalking = (originalMoveDir != Vector3.zero);
        float rotationSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, originalMoveDir,Time.deltaTime * rotationSpeed);
    }

    public bool IsWalking()
    {
        return isWalking;
    }
}
