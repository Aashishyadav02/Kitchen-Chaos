using UnityEngine;


[CreateAssetMenu()]
public class CuttingRecipeSo : ScriptableObject
{
   public KitchenObjectSO input;
   public KitchenObjectSO output;
   public int cuttingProgressMax;
}
