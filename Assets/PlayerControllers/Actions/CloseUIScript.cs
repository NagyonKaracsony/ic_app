using UnityEngine;
public class CloseUI : MonoBehaviour
{
    public void ClosePanel()
    {
        Destroy(gameObject.transform.parent.gameObject); // Destroy the UI panel
    }
}