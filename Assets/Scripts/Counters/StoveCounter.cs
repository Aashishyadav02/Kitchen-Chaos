using System;
using System.Collections;
using UnityEngine;

public class StoveCounter : BaseCounter,IHasProgress
{
   public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
   public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

   public class OnStateChangedEventArgs : EventArgs
   {
      public State state;
   }
   public enum State
   {
      Idle,
      Frying,
      Fried,
      Burned,
   }
   
   [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
   [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

   private State state;
   private float fryingTimer;
   private FryingRecipeSO fryingRecipeSO;
   private float burningTimer;
   private BurningRecipeSO burningRecipeSO;

   private void Start()
   {
      state = State.Idle;
   }

   private void Update()
   {
      if (HasKitchenObject())
      {
         switch (state)
         {
            case State.Idle:

               break;
            case State.Frying:
               fryingTimer += Time.deltaTime;
               
               OnProgressChanged?.Invoke(this,new IHasProgress.OnProgressChangedEventArgs
               {
                  progressNormlized = fryingTimer/fryingRecipeSO.fryingTimerMax
               });
               
               if (fryingTimer > fryingRecipeSO.fryingTimerMax)
               {
                  //fried
                  GetKitchenObject().DestroySelf();
                  KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);

                  state = State.Fried;
                  burningTimer = 0f;
                  burningRecipeSO = GetBurniningRecipeSoWithInput(GetKitchenObject().GetKitchenObjectSO());
                  
                  OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                     { state = state });
               }

               break;
            case State.Fried:
               burningTimer += Time.deltaTime;
               
               OnProgressChanged?.Invoke(this,new IHasProgress.OnProgressChangedEventArgs
               {
                  progressNormlized =burningTimer/burningRecipeSO.burningTimerMax
               });

               if (burningTimer > burningRecipeSO.burningTimerMax)
               {
                  //fried
                  GetKitchenObject().DestroySelf();
                  KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                  state = State.Burned;
                  
                  OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                     { state = state });
                  OnProgressChanged?.Invoke(this,new IHasProgress.OnProgressChangedEventArgs
                  {
                     progressNormlized = 0f
                  });
               }

               break;
            case State.Burned:

               break;
         }
      }
   }
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
               //player is carrying something and can be Fried
               player.GetKitchenObject().SetKitchenObjectParent(this);
               
               fryingRecipeSO = GetFryingRecipeSoWithInput(GetKitchenObject().GetKitchenObjectSO());
               state = State.Frying;
               fryingTimer = 0f;
               OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                  { state = state });
               OnProgressChanged?.Invoke(this,new IHasProgress.OnProgressChangedEventArgs
               {
                  progressNormlized = fryingTimer/fryingRecipeSO.fryingTimerMax
               });
            }
         }
         else
         {
            //player do not carrying Anything
         }
      }
      else
      {
         //there is kitchen object there
         if (player.HasKitchenObject())
         {
            //player is carrying something
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {//player is holding a plate
               if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
               {
                  GetKitchenObject().DestroySelf();
                  
                  state = State.Idle;
                  OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                     { state = state });
                  OnProgressChanged?.Invoke(this,new IHasProgress.OnProgressChangedEventArgs
                  {
                     progressNormlized = 0f
                  });
               }
            }
            
         }
         else
         {
            //player is not carrying anything
            GetKitchenObject().SetKitchenObjectParent(player);
            state = State.Idle;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
               { state = state });
            OnProgressChanged?.Invoke(this,new IHasProgress.OnProgressChangedEventArgs
            {
               progressNormlized = 0f
            });
         }
      }
   }
   private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
   {
      FryingRecipeSO fryingRecipeSo=GetFryingRecipeSoWithInput(inputKitchenObjectSO);
      return fryingRecipeSo != null;
   }

   private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
   {
      FryingRecipeSO fryingRecipeSo=GetFryingRecipeSoWithInput(inputKitchenObjectSO);
      if (fryingRecipeSo != null)
      {
         return fryingRecipeSo.output;
      }
      else
      {
         return null;
      }
   }

   private FryingRecipeSO GetFryingRecipeSoWithInput(KitchenObjectSO inputKitchenObjectSO)
   {
      foreach (FryingRecipeSO fryingRecipeSo in fryingRecipeSOArray)
      {
         if (fryingRecipeSo.input==inputKitchenObjectSO)
         {
            return fryingRecipeSo;
         }
      }
      return null;
   }
   private BurningRecipeSO GetBurniningRecipeSoWithInput(KitchenObjectSO inputKitchenObjectSO)
   {
      foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
      {
         if (burningRecipeSO.input==inputKitchenObjectSO)
         {
            return burningRecipeSO;
         }
      }
      return null;
   }

   public bool IsFried()
   {
      return state == State.Fried;
   }
}
