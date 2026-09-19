using UnityEngine;
using TMPro;
using System.Collections;

public class EducationalInfoSequence : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text educationText;

    [Header("Timing")]
    public float displayTime = 25f;

    private string[] messages =
    {
        // 1
        "<color=#66E0FF><b>MISSION: INSIDE THE BLOODSTREAM</b></color>\n\n" +
        "<color=#F2F2F2>Avoid red blood cells and collect white blood cells.\n" +
        "Reach the blockage as quickly as possible.\n" +
        "Use the laser to destroy the blockage and restore blood flow.</color>",

        // 2
        "<color=#66E0FF><b>WHITE BLOOD CELLS</b></color>\n\n" +
        "<color=#F2F2F2>White blood cells are part of the immune system.\n" +
        "They help defend the body against infections and harmful microorganisms.</color>\n\n" +
        "<color=#FFD966><b>GAME TIP: Collect them for +5 points.</b></color>",

        // 3
        "<color=#66E0FF><b>RED BLOOD CELLS</b></color>\n\n" +
        "<color=#F2F2F2>Red blood cells carry oxygen from the lungs to tissues throughout the body.\n" +
        "They are essential for keeping organs supplied with oxygen.</color>\n\n" +
        "<color=#FFD966><b>GAME TIP: Hitting one costs -10 points.</b></color>",

        // 4
        "<color=#66E0FF><b>PLATELETS</b></color>\n\n" +
        "<color=#F2F2F2>Platelets help stop bleeding when a blood vessel is injured.\n" +
        "They gather around damaged areas and help form blood clots.</color>",

        // 5
        "<color=#66E0FF><b>BLOOD FLOW</b></color>\n\n" +
        "<color=#F2F2F2>Blood continuously moves through blood vessels.\n" +
        "This circulation carries oxygen and nutrients to tissues throughout the body.</color>",

        // 6
        "<color=#66E0FF><b>OXYGEN TRANSPORT</b></color>\n\n" +
        "<color=#F2F2F2>Most oxygen in the blood is carried by red blood cells.\n" +
        "Healthy blood flow allows oxygen to reach organs and tissues efficiently.</color>",

        // 7
        "<color=#66E0FF><b>THE IMMUNE SYSTEM</b></color>\n\n" +
        "<color=#F2F2F2>The bloodstream transports immune cells throughout the body.\n" +
        "White blood cells help identify and respond to harmful microorganisms.</color>",

        // 8
        "<color=#66E0FF><b>BLOOD CLOTTING</b></color>\n\n" +
        "<color=#F2F2F2>Blood clotting is normally a protective process that helps stop bleeding.\n" +
        "However, a clot forming in the wrong place can obstruct blood flow.</color>",

        // 9
        "<color=#66E0FF><b>BLOCKED BLOOD FLOW</b></color>\n\n" +
        "<color=#F2F2F2>A blockage reduces the amount of blood that can pass through a vessel.\n" +
        "This can reduce the oxygen delivered to nearby tissues.</color>",

        // 10
        "<color=#66E0FF><b>WHY THE BRAIN NEEDS BLOOD</b></color>\n\n" +
        "<color=#F2F2F2>Brain cells require a continuous supply of oxygen and nutrients.\n" +
        "A serious interruption of blood flow can quickly damage brain tissue.</color>",

        // 11
        "<color=#66E0FF><b>ISCHEMIC STROKE</b></color>\n\n" +
        "<color=#F2F2F2>An ischemic stroke occurs when a blood vessel supplying the brain becomes blocked.\n" +
        "Restoring blood flow as quickly as possible is extremely important.</color>",

        // 12
        "<color=#66E0FF><b>TREATING THE BLOCKAGE</b></color>\n\n" +
        "<color=#F2F2F2>The final goal is to remove the blockage and restore blood flow.\n" +
        "When you reach the blockage, use the laser to destroy the targets on it.\n" +
        "Clearing the blockage allows blood to flow normally again.</color>\n\n" +
        "<color=#FFD966><b>GAME TIP: Use the laser to clear the blockage and save the patient.</b></color>"
    };

    void Start()
    {
        StartCoroutine(ShowInformation());
    }

    IEnumerator ShowInformation()
    {
        for (int i = 0; i < messages.Length; i++)
        {
            educationText.text = messages[i];

            // آخر معلومة تبقى ظاهرة
            if (i == messages.Length - 1)
                yield break;

            yield return new WaitForSeconds(displayTime);
        }
    }
}