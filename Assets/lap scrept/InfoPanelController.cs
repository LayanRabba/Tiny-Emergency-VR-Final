using UnityEngine;

public class InfoPanelController : MonoBehaviour
{
    [Header("Info Panel")]
    public GameObject infoPanel;

    public void ToggleInfo()
    {
        if (infoPanel == null)
            return;

        infoPanel.SetActive(!infoPanel.activeSelf);
    }

    public void ShowInfo()
    {
        if (infoPanel == null)
            return;

        infoPanel.SetActive(true);
    }

    public void HideInfo()
    {
        if (infoPanel == null)
            return;

        infoPanel.SetActive(false);
    }
}