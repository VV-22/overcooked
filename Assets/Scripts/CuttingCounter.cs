using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField]private KitchenObjectSO cutKitchenObjectSO;
    public override void Interact(Player player)
    {
       if(!HasKitchenObject())
        {
            //counter is empty
            if(player.HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                //player not carrying anything
            }
        }
        else
        {
            //there is a kitchenObject
            if(player.HasKitchenObject())
            {
                //player is carrying something
            }
            else
            {
                //player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if(HasKitchenObject())
        {
            //Kitchen object present. have to cut now
            GetKitchenObject().DestroySelf();
            KitchenObject.spawnKitchenObject(cutKitchenObjectSO, this);
        }
    }
}
