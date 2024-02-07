using UnityEngine;
public static class CheckDuringPlatform
{
    public static bool PlatformIsPC()
    {
        RuntimePlatform platform = Application.platform;
        return platform == RuntimePlatform.WindowsEditor || platform == RuntimePlatform.WindowsPlayer;
    }
}