using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    [SerializeField]private CuttingRecipeSO[] cuttingRecipeSOArray;
    private int cuttingProgress;

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public event EventHandler OnCut;
    public override void Interact(Player player)
    {
       if(!HasKitchenObject())
        {
            //counter is empty
            if(player.HasKitchenObject())
            {
                if(HasRecipeForInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    //player carrying something that can be cut
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;
                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    OnProgressChanged?.Invoke(this,new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = cuttingProgress/(float)cuttingRecipeSO.cuttingProgressMax
                    });
                }
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
        if(HasKitchenObject() && HasRecipeForInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            //Kitchen object that can be cut is present. have to cut now
            cuttingProgress ++;
            OnCut?.Invoke(this, EventArgs.Empty);
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
            OnProgressChanged?.Invoke(this,new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = cuttingProgress/(float)cuttingRecipeSO.cuttingProgressMax
            });
            if(cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
                GetKitchenObject().DestroySelf();
                KitchenObject.spawnKitchenObject(outputKitchenObjectSO, this);
            }
        }
    }
    private bool HasRecipeForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        if(cuttingRecipeSO != null)
            return cuttingRecipeSO.output;
        else
            return null;
        
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach(CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if(cuttingRecipeSO.input == inputKitchenObjectSO )
            return cuttingRecipeSO;
        }
        return null;
    }
}
