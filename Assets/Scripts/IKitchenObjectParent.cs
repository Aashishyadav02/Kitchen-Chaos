using UnityEngine;

public interface IKitchenObjectParent
{
    public Transform GetKitchenObjectFollwTransform();
    
    public void SetKitchenObject(KitchenObject kitchenObject);
    
    public KitchenObject GetKitchenObject();

    public void ClearKitchenObject();
    
    public bool HasKitchenObject();


}
