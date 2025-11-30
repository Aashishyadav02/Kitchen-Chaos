using UnityEngine;

public class StoveBurnWarningUI : MonoBehaviour
{
   [SerializeField] private StoveCounter stoveCounter;

   private void Start()
   {
      stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
      Hide();
   }

   private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
   {
      float showBurnProgressAmount = .5f;
      bool show = stoveCounter.IsFried() && e.progressNormlized >= showBurnProgressAmount;

      if (show)
      {
         Show();
      }
      else
      {
         Hide();
      }
   }

   private void Show()
   {
      gameObject.SetActive(true);
   }

   private void Hide()
   {
      gameObject.SetActive(false);
   }
}
