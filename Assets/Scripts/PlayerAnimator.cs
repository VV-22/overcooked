using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";
    private Animator playerAnimator;
    [SerializeField]private Player player;
    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
    }

    
    private void Update()
    {
        playerAnimator.SetBool(IS_WALKING,player.IsWalking());
    }
    
}
