using UnityEngine;

public class BadTransformFinder : MonoBehaviour
{
    void Update()
    {
        Transform[] all = FindObjectsByType<Transform>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );

        foreach (Transform t in all)
        {
            if (IsBad(t.position) ||
                IsBad(t.localPosition) ||
                IsBad(t.localScale) ||
                IsBad(t.rotation))
            {
                Debug.LogError(
                    "BAD TRANSFORM FOUND >>> " +
                    GetFullPath(t) +
                    "\nWorld Position: " + t.position +
                    "\nLocal Position: " + t.localPosition +
                    "\nLocal Scale: " + t.localScale +
                    "\nRotation: " + t.rotation,
                    t.gameObject
                );

                enabled = false;
                return;
            }
        }
    }

    bool IsBad(Vector3 v)
    {
        return !IsFinite(v.x) ||
               !IsFinite(v.y) ||
               !IsFinite(v.z) ||
               Mathf.Abs(v.x) > 10000 ||
               Mathf.Abs(v.y) > 10000 ||
               Mathf.Abs(v.z) > 10000;
    }

    bool IsBad(Quaternion q)
    {
        return !IsFinite(q.x) ||
               !IsFinite(q.y) ||
               !IsFinite(q.z) ||
               !IsFinite(q.w);
    }

    bool IsFinite(float value)
    {
        return !float.IsNaN(value) && !float.IsInfinity(value);
    }

    string GetFullPath(Transform t)
    {
        string path = t.name;

        while (t.parent != null)
        {
            t = t.parent;
            path = t.name + "/" + path;
        }

        return path;
    }
}