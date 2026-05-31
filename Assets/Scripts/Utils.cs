using UnityEngine;

public class Utils
{
    public static float ExpDecayT(float t)
    {
        return 1f - Mathf.Exp(-t * Time.deltaTime);
    }
}
