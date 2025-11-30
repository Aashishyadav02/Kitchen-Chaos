using System;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
   
   
   
   public event EventHandler OnPlateSpawned;
   public event EventHandler OnPlateRemoved;
   
   [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
   private float spawnPlateTimer;
   private float spwanPlateTimerMax=4f;
   private int plateSpawnedAmount;
   private int plateSpawnedAmountMax=4;

   private void Update()
   {
      spawnPlateTimer += Time.deltaTime;
      if (spawnPlateTimer > spwanPlateTimerMax)
      {
         spawnPlateTimer = 0;
         if (KitchenGameManager.Instance.IsGamePlaying() && plateSpawnedAmount < plateSpawnedAmountMax)
         {
            plateSpawnedAmount++;
            OnPlateSpawned?.Invoke(this, EventArgs.Empty);
         }
      }
   }

   public override void Interact(Player player)
   {
      if (!player.HasKitchenObject())
      {
         //Player Is empty handed
         if (plateSpawnedAmount >0)
         { //there is atleast one plate here
            plateSpawnedAmount--;
            KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
            
            OnPlateRemoved?.Invoke(this, EventArgs.Empty);
         }
      }
   }
}
