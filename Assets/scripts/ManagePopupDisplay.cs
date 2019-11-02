using Doozy.Engine.UI;
using UnityEngine;

public class ManagePopupDisplay : MonoBehaviour
{
    public string popupName = "HowToPlay";
    public void ShowPopup()
    {
        UIPopup popup = UIPopup.GetPopup(popupName);
        if (popup != null)
        {
            popup.Show();
        }
        else
        {
            return;
        }
    }
}
