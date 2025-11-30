using System;
using UnityEngine;

public class CuttingCounter : BaseCounter,IHasProgress
{
   public static event EventHandler OnAnyCut;

    new public static void ResetStaticData()
   {
      OnAnyCut = null;
   }
   public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
   
   public event EventHandler OnCut;
   
   [SerializeField] private CuttingRecipeSo[] cutKitchenObjectSoArray;
   private int cuttingProgress;
   public override void Interact(Player player)
   {
      if (!HasKitchenObject())
      {
         //there is no kitchen object 
         if (player.HasKitchenObject())
         {
            //player is carrying something
            if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
            {
               //player is carrying something and can be cut
               player.GetKitchenObject().SetKitchenObjectParent(this);
               cuttingProgress = 0;
               
               CuttingRecipeSo cuttingRecipeSo=GetCuttingRecipeSoWithInput(GetKitchenObject().GetKitchenObjectSO());
               
               OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs() 
                  { progressNormlized = (float)cuttingProgress /cuttingRecipeSo.cuttingProgressMax });
            }
         }
         else
         {
            //player do not carrying Anything
         }
      }
      else
      {
         //there is kitchen object here
         if (player.HasKitchenObject())
         {
            //player is carrying something
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            { 
               //player is holding A plate
               if(plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()));
               GetKitchenObject().DestroySelf();
            }
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
      if (HasKitchenObject()&& HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
      {
         //there is kitchen object here And it can be cut
         cuttingProgress++;
         OnCut?.Invoke(this, EventArgs.Empty);
         Debug.Log(OnAnyCut.GetInvocationList().Length);
         OnAnyCut?.Invoke(this, EventArgs.Empty);
         CuttingRecipeSo cuttingRecipeSo=GetCuttingRecipeSoWithInput(GetKitchenObject().GetKitchenObjectSO());
         
         OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs() 
            { progressNormlized = (float)cuttingProgress /cuttingRecipeSo.cuttingProgressMax });
         if (cuttingProgress >= cuttingRecipeSo.cuttingProgressMax)
         {
            KitchenObjectSO outputKitchenObjectSo = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
            GetKitchenObject().DestroySelf();
            KitchenObject.SpawnKitchenObject(outputKitchenObjectSo, this);
         }
      }
   }

   private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
   {
     CuttingRecipeSo cuttingRecipeSo=GetCuttingRecipeSoWithInput(inputKitchenObjectSO);
     return cuttingRecipeSo != null;
   }

   private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
   {
      CuttingRecipeSo cuttingRecipeSo=GetCuttingRecipeSoWithInput(inputKitchenObjectSO);
      if (cuttingRecipeSo != null)
      {
         return cuttingRecipeSo.output;
      }
      else
      {
         return null;
      }
   }

   private CuttingRecipeSo GetCuttingRecipeSoWithInput(KitchenObjectSO inputKitchenObjectSO)
   {
      foreach (CuttingRecipeSo cuttingRecipeSo in cutKitchenObjectSoArray)
      {
         if (cuttingRecipeSo.input==inputKitchenObjectSO)
         {
            return cuttingRecipeSo;
         }
      }
      return null;
   }
}
