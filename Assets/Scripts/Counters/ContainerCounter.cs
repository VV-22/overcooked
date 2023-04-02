using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{

    public event EventHandler onPlayerGrabbedObject;
    [SerializeField]private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        if(!player.HasKitchenObject())
        {
            KitchenObject.spawnKitchenObject(kitchenObjectSO, player);
            
            onPlayerGrabbedObject?.Invoke(this,EventArgs.Empty);
        }
    }
}
