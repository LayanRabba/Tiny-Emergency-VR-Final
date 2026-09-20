using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [Header("Player Health")]
    public VehicleHealth vehicleHealth;

    [Header("UI")]
    public Image bloodFill;

    private void Update()
    {
        if (vehicleHealth == null || bloodFill == null)
            return;

        float healthPercent =
            vehicleHealth.CurrentEnergy /
            vehicleHealth.MaxEnergy;

        bloodFill.fillAmount = healthPercent;
    }
}