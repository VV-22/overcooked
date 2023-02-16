using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public static Player Instance{get; private set;}

    [SerializeField]private float moveSpeed;
    [SerializeField]private GameInput gameInput;
    [SerializeField]private LayerMask countersLayerMask;

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public ClearCounter selectedCounter;
    }
    private bool isWalking;
    private Vector3 lastInteractDir;
    private ClearCounter selectedCounter;

    private void Awake()
    {
        if(Instance !=null)
            Debug.LogError("There is more than one player instance");
        Instance = this;
    }
    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    private void Start()
    {
       gameInput.OnInteractAction += GameInput_OnInteractAction;
    }
    public bool IsWalking()
    {
        return isWalking;
    }


    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if(selectedCounter != null)
            selectedCounter.Interact();
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
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
    
    
    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        if(moveDir!= Vector3.zero)
            lastInteractDir = moveDir;
        float interactDistance = 1.5f;
        
        if(Physics.Raycast(transform.position,lastInteractDir,out RaycastHit raycastHit, interactDistance,countersLayerMask))
        {
            if(raycastHit.transform.TryGetComponent<ClearCounter>(out ClearCounter clearCounter))
            {
                //Has a clearCounter Component
                //clearCounter.Interact();
                if(clearCounter != selectedCounter)
                {
                    setSelectedCounter(clearCounter);
                }

            }
            else
            {
                setSelectedCounter(null);
            }
        }
        else
        {
            setSelectedCounter(null);
        }
    }

    private void setSelectedCounter(ClearCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this,new OnSelectedCounterChangedEventArgs{selectedCounter = selectedCounter});

    }
}
