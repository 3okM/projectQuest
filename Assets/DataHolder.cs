using UnityEngine;

public class DataHolder :MonoBehaviour
{
    private static float volume;

public static float Get()
{
        Debug.Log("Get!" + volume);
        return volume;
}
public static void Set(float value)
{
        volume = value;
}
}