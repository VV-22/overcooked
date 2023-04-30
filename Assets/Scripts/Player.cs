using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{

    public static Player Instance{get; private set;}

    [SerializeField]private float moveSpeed;
    [SerializeField]private GameInput gameInput;
    [SerializeField]private LayerMask countersLayerMask;
    [SerializeField]private Transform kitchenObjectHoldPoint;

    public event EventHandler OnPickedSomething;

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }
    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

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
       gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if(!KitchenGameManager.Instance.IsGamePlaying())
            return;
        if(selectedCounter != null)
            selectedCounter.InteractAlternate(this);
    }

    public bool IsWalking()
    {
        return isWalking;
    }


    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if(!KitchenGameManager.Instance.IsGamePlaying())
            return;
        if(selectedCounter != null)
            selectedCounter.Interact(this);
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
            canMove = moveDir.x != 0 && !(Physics.CapsuleCast(transform.position,transform.position + Vector3.up * playerHeight,playerRadius,moveDirX,moveDistance));
            if(canMove)
            {
                //can move in X dir
                moveDir = moveDirX;
            }
            else
            {
                //attempt only Z dir
                Vector3 moveDirZ = new Vector3(0f, 0f, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !(Physics.CapsuleCast(transform.position,transform.position + Vector3.up * playerHeight,playerRadius,moveDirZ,moveDistance));
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
        isWalking = (moveDir != Vector3.zero);
        float rotationSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir,Time.deltaTime * rotationSpeed);
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
            if(raycastHit.transform.TryGetComponent<BaseCounter>(out BaseCounter baseCounter))
            {
                //Has a baseCounter Component
                //baseCounter.Interact();
                if(baseCounter != selectedCounter)
                {
                    setSelectedCounter(baseCounter);
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

    private void setSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this,new OnSelectedCounterChangedEventArgs{selectedCounter = selectedCounter});

    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if(kitchenObject!= null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
