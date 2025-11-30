using System;
using Unity.Collections;
using UnityEngine;

public class PlateIconsUI : MonoBehaviour
{
   [SerializeField] private PlateKitchenObject plateKitchenObject;
   [SerializeField] private Transform iconsTemplate;

   private void Awake()
   {
      iconsTemplate.gameObject.SetActive(false);
   }

   private void Start()
   {
      plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
   }

   private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
   {
      UpdateVisuals();
   }

   private void UpdateVisuals()
   {
      foreach (Transform child in transform)
      {
         if (child==iconsTemplate)
         {
            continue;
         }
         Destroy(child.gameObject);
      }
      foreach (KitchenObjectSO kitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList() )
      {
         Transform iconTransform = Instantiate(iconsTemplate,transform);
         iconTransform.gameObject.SetActive(true);
         iconTransform.GetComponent<PlateIconSingleUI>().SetKitchenObjectSO(kitchenObjectSO);
      }
   }
}
