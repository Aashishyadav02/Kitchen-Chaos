using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class PlateIconSingleUI : MonoBehaviour
{
  [SerializeField] private Image image;
  public void SetKitchenObjectSO(KitchenObjectSO kitchenObjectSO)
  {
    image.sprite = kitchenObjectSO.sprite;
  }
  
}
