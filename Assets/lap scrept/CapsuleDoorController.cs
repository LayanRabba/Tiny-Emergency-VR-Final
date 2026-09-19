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

    [Header("Sound")]
    public AudioSource machineAudio;

    [Header("Scene Transition")]
    public string veinSceneName = "InsideVesselsScene";
    public float transitionDelay = 5f;

    // 0 = Closed
    // 1 = Open
    private float animationProgress = 0f;

    // لمنع تكرار صوت الفتح/الإغلاق كل Frame
    private bool previousShouldOpen = false;

    // حتى يبدأ الانتقال مرة واحدة فقط
    private bool transitionStarted = false;

    void Start()
    {
        // نوقف التشغيل التلقائي للـ Animator
        capsuleAnimator.speed = 0f;

        // تبدأ الكبسولة مغلقة
        animationProgress = 0f;

        capsuleAnimator.Play(
            animationStateName,
            0,
            animationProgress
        );

        // إظهار أول Frame
        capsuleAnimator.Update(0f);

        // المدخل مسكر بالبداية
        frontCollider.enabled = true;

        // الحالة الابتدائية مغلقة
        previousShouldOpen = false;

        // نتأكد أن الصوت لا يبدأ وحده
        if (machineAudio != null)
        {
            machineAudio.Stop();
        }
    }

    void Update()
    {
        // هل اللاعب قريب من الكبسولة؟
        bool playerNear = IsInsideZone(
            outerZone,
            player.position
        );

        // هل اللاعب داخل الكبسولة؟
        bool playerInside = IsInsideZone(
            insideZone,
            player.position
        );

        // تفتح فقط إذا كان اللاعب قريبًا ولكن ليس داخلها
        bool shouldOpen = playerNear && !playerInside;

        // إذا تغيرت حالة الكبسولة:
        // مغلقة -> فتح
        // أو مفتوحة -> إغلاق
        if (shouldOpen != previousShouldOpen)
        {
            if (machineAudio != null)
            {
                machineAudio.Stop();
                machineAudio.Play();
            }

            previousShouldOpen = shouldOpen;
        }

        // 1 = مفتوحة
        // 0 = مغلقة
        float targetProgress = shouldOpen ? 1f : 0f;

        animationProgress = Mathf.MoveTowards(
            animationProgress,
            targetProgress,
            Time.deltaTime / animationDuration
        );

        // نحرك الأنميشن يدويًا
        capsuleAnimator.Play(
            animationStateName,
            0,
            animationProgress
        );

        capsuleAnimator.Update(0f);

        // مفتوحة = يستطيع المرور
        // مغلقة/تسكر = الجدار الأمامي يمنع المرور
        frontCollider.enabled = !shouldOpen;

        // أول ما يدخل اللاعب داخل الكبسولة
        // يبدأ عداد 5 ثواني مرة واحدة فقط
        if (playerInside && !transitionStarted)
        {
            transitionStarted = true;
            StartCoroutine(LoadVeinScene());
        }
    }

    IEnumerator LoadVeinScene()
    {
        // ننتظر 5 ثواني
        yield return new WaitForSeconds(transitionDelay);

        // ننتقل إلى مشهد الوريد
        SceneManager.LoadScene(veinSceneName);
    }

    private bool IsInsideZone(Collider zone, Vector3 point)
    {
        Vector3 closestPoint = zone.ClosestPoint(point);

        return (closestPoint - point).sqrMagnitude < 0.0001f;
    }
}