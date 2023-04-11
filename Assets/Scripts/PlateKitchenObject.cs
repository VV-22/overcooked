using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;

    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }
    private List<KitchenObjectSO> kitchenObjectSOList;
    [SerializeField]private List<KitchenObjectSO> validKitchenObjectSOList;

    private void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }
    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if(!validKitchenObjectSOList.Contains(kitchenObjectSO))
        {
            //not a valid ingredient
            return false;
        }
        if(!kitchenObjectSOList.Contains(kitchenObjectSO))
        {
            //valid ingredient not present in the current plate
            kitchenObjectSOList.Add(kitchenObjectSO);
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs{kitchenObjectSO = kitchenObjectSO});
            return true;
        }
        else
        {
            //valid ingredient but already present in the plate
            return false;
        }
    }
}
