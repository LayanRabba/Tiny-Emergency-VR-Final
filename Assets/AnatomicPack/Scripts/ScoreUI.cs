using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    public VehicleHealth vehicleHealth;
    public TMP_Text scoreText;

    private void Update()
    {
        if (vehicleHealth == null || scoreText == null)
            return;

        scoreText.text = vehicleHealth.CurrentScore.ToString();
    }
}