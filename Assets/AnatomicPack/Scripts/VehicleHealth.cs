using UnityEngine;

public class VehicleHealth : MonoBehaviour
{
    [Header("Energy")]
    [SerializeField] private float maxEnergy = 100f;
    [SerializeField] private float currentEnergy;

    [Header("Score")]
    [SerializeField] private int currentScore = 0;

    [Header("Obstacle Damage")]
    [SerializeField] private float redBloodCellDamage = 10f;
    [SerializeField] private float virusDamage = 20f;

    [Header("White Cell Bonus")]
    [SerializeField] private int whiteCellScore = 10;
    [SerializeField] private float whiteCellEnergy = 5f;

    [Header("Protection")]
    [Tooltip("Minimum time between damage events.")]
    [SerializeField] private float damageCooldown = 1f;

    [Tooltip("Protection time when the game starts.")]
    [SerializeField] private float startProtectionDuration = 2f;

    private float nextDamageTime;
    private bool gameOver;

    public float CurrentEnergy => currentEnergy;
    public float MaxEnergy => maxEnergy;
    public int CurrentScore => currentScore;
    public bool IsGameOver => gameOver;

    private void Awake()
    {
        currentEnergy = maxEnergy;
        currentScore = 0;
        gameOver = false;
    }

    private void Start()
    {
        nextDamageTime = Time.time + startProtectionDuration;

        Debug.Log(
            $"Game started. Energy: {currentEnergy}/{maxEnergy}, " +
            $"Score: {currentScore}"
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameOver)
            return;

        string objectTag = other.gameObject.tag;

        // White blood cells are bonus items.
        if (objectTag == "WhiteBloodCell")
        {
            CollectWhiteBloodCell(other.gameObject);
            return;
        }

        // Platelets are visual elements only and are ignored.
        if (objectTag == "Platelet")
            return;

        // Prevent receiving many damage events at the same time.
        if (Time.time < nextDamageTime)
            return;

        if (objectTag == "RedBloodCell")
        {
            nextDamageTime = Time.time + damageCooldown;
            TakeDamage(redBloodCellDamage, "Red blood cell");
        }
        else if (objectTag == "Virus")
        {
            nextDamageTime = Time.time + damageCooldown;
            TakeDamage(virusDamage, "Virus");
        }
    }

    private void CollectWhiteBloodCell(GameObject whiteCell)
    {
        currentScore += whiteCellScore;
        AddEnergy(whiteCellEnergy);

        Debug.Log(
            $"White blood cell collected. " +
            $"Score: {currentScore}, " +
            $"Energy: {currentEnergy}/{maxEnergy}"
        );

        // Remove the collected white cell from the scene.
        whiteCell.SetActive(false);
    }

    private void TakeDamage(float damage, string obstacleName)
    {
        currentEnergy = Mathf.Max(0f, currentEnergy - damage);

        Debug.Log(
            $"Hit {obstacleName}. Damage: {damage}. " +
            $"Energy: {currentEnergy}/{maxEnergy}"
        );

        if (currentEnergy <= 0f)
        {
            EndGame();
        }
    }

    public void AddEnergy(float amount)
    {
        if (gameOver || amount <= 0f)
            return;

        currentEnergy = Mathf.Min(
            maxEnergy,
            currentEnergy + amount
        );
    }

    private void EndGame()
    {
        if (gameOver)
            return;

        gameOver = true;

        VehiclePathController movement =
            GetComponent<VehiclePathController>();

        if (movement != null)
        {
            movement.enabled = false;
        }

        Debug.Log(
            $"Game Over. Final Score: {currentScore}"
        );
    }
}