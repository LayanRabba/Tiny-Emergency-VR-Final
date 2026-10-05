using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CapsuleDoorController : MonoBehaviour
{
    [Header("Capsule")]
    public Animator capsuleAnimator;

    [Header("Player")]
    public Transform player;

    [Header("Detection Zones")]
    public BoxCollider outerZone;
    public BoxCollider insideZone;

    [Header("Front Wall")]
    public BoxCollider frontCollider;

    [Header("Animation")]
    public string animationStateName = "Take 001";
    public float animationDuration = 1.5f;

    [Header("Door Stability")]
    public float zoneStabilityTime = 0.25f;

    [Header("Sound")]
    public AudioSource machineAudio;

    [Header("Scene Transition")]
    public string veinSceneName = "InsideVesselsScene";
    public float transitionDelay = 5f;

    // 0 = Closed
    // 1 = Open
    private float animationProgress = 0f;

    // الحالة الحالية التي أمرنا الباب بها
    private bool doorShouldBeOpen = false;

    // تستخدم لمنع الاهتزاز على حدود المناطق
    private bool pendingState = false;
    private float pendingTimer = 0f;

    // حتى ننتقل للمشهد مرة واحدة فقط
    private bool transitionStarted = false;

    void Start()
    {
        capsuleAnimator.speed = 0f;

        animationProgress = 0f;

        capsuleAnimator.Play(
            animationStateName,
            0,
            animationProgress
        );

        capsuleAnimator.Update(0f);

        // الباب مغلق بالبداية
        doorShouldBeOpen = false;
        pendingState = false;

        // الجدار الأمامي مغلق
        frontCollider.enabled = true;

        if (machineAudio != null)
        {
            machineAudio.Stop();
            machineAudio.loop = false;
        }
    }

    void Update()
    {
        bool playerNear = IsInsideZone(
            outerZone,
            player.position
        );

        bool playerInside = IsInsideZone(
            insideZone,
            player.position
        );

        /*
         * الباب:
         *
         * قريب وخارج الكبسولة = مفتوح
         * داخل الكبسولة = مغلق
         * بعيد عن الكبسولة = مغلق
         */
        bool desiredOpen = playerNear && !playerInside;

        HandleDoorState(desiredOpen);

        // تحريك الأنميشن
        float targetProgress = doorShouldBeOpen ? 1f : 0f;

        animationProgress = Mathf.MoveTowards(
            animationProgress,
            targetProgress,
            Time.deltaTime / animationDuration
        );

        capsuleAnimator.Play(
            animationStateName,
            0,
            animationProgress
        );

        capsuleAnimator.Update(0f);

        /*
         * أثناء فتح الباب نسمح بالمرور.
         * عند الدخول أو الابتعاد نعيد الجدار.
         */
        frontCollider.enabled = !doorShouldBeOpen;

        // عند الدخول للكبسولة يبدأ عداد الانتقال مرة واحدة
        if (playerInside && !transitionStarted)
        {
            transitionStarted = true;
            StartCoroutine(LoadVeinScene());
        }
    }

    private void HandleDoorState(bool desiredOpen)
    {
        // إذا الحالة المطلوبة هي نفس الحالة الحالية، لا نفعل شيء
        if (desiredOpen == doorShouldBeOpen)
        {
            pendingTimer = 0f;
            pendingState = desiredOpen;
            return;
        }

        /*
         * إذا تغيرت الحالة المطلوبة،
         * ننتظر قليلًا للتأكد أن اللاعب فعلًا دخل/خرج
         * وليس فقط يهتز على حدود الـCollider.
         */
        if (pendingState != desiredOpen)
        {
            pendingState = desiredOpen;
            pendingTimer = 0f;
        }

        pendingTimer += Time.deltaTime;

        if (pendingTimer >= zoneStabilityTime)
        {
            SetDoorState(desiredOpen);
            pendingTimer = 0f;
        }
    }

    private void SetDoorState(bool open)
    {
        // إذا الحالة أصلًا نفسها، لا نشغل الصوت مرة ثانية
        if (doorShouldBeOpen == open)
            return;

        doorShouldBeOpen = open;

        // صوت واحد فقط عند الفتح أو الإغلاق
        if (machineAudio != null)
        {
            machineAudio.Stop();
            machineAudio.Play();
        }
    }

    IEnumerator LoadVeinScene()
    {
        yield return new WaitForSeconds(transitionDelay);

        SceneManager.LoadScene(veinSceneName);
    }

    private bool IsInsideZone(Collider zone, Vector3 point)
    {
        if (zone == null)
            return false;

        Vector3 closestPoint = zone.ClosestPoint(point);

        return (closestPoint - point).sqrMagnitude < 0.0001f;
    }
}