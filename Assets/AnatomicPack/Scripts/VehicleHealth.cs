using UnityEngine;

public class VehicleHealth : MonoBehaviour
{
    [Header("Energy")]
    [SerializeField] private float maxEnergy = 100f;
    [SerializeField] private float currentEnergy;

    [Header("Score")]
    [SerializeField] private int currentScore = 0;

    [Header("Obstacle Damage")]
    [SerializeField] private float redBloodCellDamage = 5f;
    [SerializeField] private float plateletDamage = 2f;
    

    [Header("White Cell Bonus")]
    [SerializeField] private int whiteCellScore = 10;
    [SerializeField] private float whiteCellEnergy = 5f;

    [Header("Collision Sounds")]
    [SerializeField] private AudioSource collisionAudioSource;
    [SerializeField] private AudioClip redBloodCellSound;
    [SerializeField] private AudioClip plateletSound;
    [SerializeField] private AudioClip whiteCellSound;

    [Header("Protection")]
    [Tooltip("Minimum time between damage events.")]
    [SerializeField] private float damageCooldown = 1f;

    [Tooltip("Protection time when the game starts.")]
    [SerializeField] private float startProtectionDuration = 2f;

    [Header("Lose UI")]
    public GameObject loseCanvas;

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

        if (loseCanvas != null)
            loseCanvas.SetActive(false);
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

        // White blood cells give a bonus and have their own sound.
        if (objectTag == "WhiteBloodCell")
        {
            CollectWhiteBloodCell(other.gameObject);
            return;
        }

        // Prevent repeated damage and sounds during the protection period.
        if (Time.time < nextDamageTime)
            return;

        if (objectTag == "RedBloodCell")
        {
            HitObstacle(
                redBloodCellDamage,
                "Red blood cell",
                redBloodCellSound
            );
        }
        else if (objectTag == "Platelet")
        {
            HitObstacle(
                plateletDamage,
                "Platelet",
                plateletSound
            );
        }
        
    }

    private void HitObstacle(
        float damage,
        string obstacleName,
        AudioClip sound
    )
    {
        nextDamageTime = Time.time + damageCooldown;
        PlaySound(sound);
        TakeDamage(damage, obstacleName);
    }

    private void CollectWhiteBloodCell(GameObject whiteCell)
    {
        currentScore += whiteCellScore;
        AddEnergy(whiteCellEnergy);
        PlaySound(whiteCellSound);

        Debug.Log(
            $"White blood cell collected. " +
            $"Score: {currentScore}, " +
            $"Energy: {currentEnergy}/{maxEnergy}"
        );

        whiteCell.SetActive(false);
    }

    private void PlaySound(AudioClip clip)
    {
        if (collisionAudioSource != null && clip != null)
            collisionAudioSource.PlayOneShot(clip);
    }

    private void TakeDamage(float damage, string obstacleName)
    {
        currentEnergy = Mathf.Max(0f, currentEnergy - damage);

        Debug.Log(
            $"Hit {obstacleName}. Damage: {damage}. " +
            $"Energy: {currentEnergy}/{maxEnergy}"
        );

        if (currentEnergy <= 0f)
            EndGame();
    }

    public void AddEnergy(float amount)
    {
        if (gameOver || amount <= 0f)
            return;

        currentEnergy = Mathf.Min(maxEnergy, currentEnergy + amount);
    }

    private void EndGame()
    {
        if (gameOver)
            return;

        gameOver = true;

        if (loseCanvas != null)
            loseCanvas.SetActive(true);

        VehiclePathController movement =
            GetComponent<VehiclePathController>();

        if (movement != null)
            movement.enabled = false;

        Debug.Log($"Game Over. Final Score: {currentScore}");
    }
}