using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;

    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSo;
    }
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;
    private List<KitchenObjectSO> kitchenObjectSoList;

    private void Awake()
    {
        kitchenObjectSoList = new List<KitchenObjectSO>();
    }
    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
       if(!validKitchenObjectSOList.Contains(kitchenObjectSO))
       {
           //not a valid ingredient
           return false;
       }
       if (kitchenObjectSoList.Contains(kitchenObjectSO)) 
       {
            //Already has this type
            return false;
       }
       else
       {
           kitchenObjectSoList.Add(kitchenObjectSO);
           OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs 
               { kitchenObjectSo = kitchenObjectSO});
           return true;
       }
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
       return kitchenObjectSoList; 
    }
}
