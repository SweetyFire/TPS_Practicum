using UnityEngine;

public static class AudioSourceExt
{
    public static bool IsReversePitch(this AudioSource source)
    {
        return source.pitch < 0f;
    }

    public static float GetClipRemainingTime(this AudioSource source)
    {
        float remainingTime = (source.clip.length - source.time) / source.pitch;
        return source.IsReversePitch() ? (source.clip.length + remainingTime) : remainingTime;
    }

    public static float GetClipDuration(this AudioSource source)
    {
        return source.clip.length / source.pitch;
    }

    public static bool IsPlaying(this AudioSource source)
    {
        return source.isPlaying || source.GetClipRemainingTime() <= 0f;
    }

    public static float GetClipDuration(this AudioSource source, AudioClip clip)
    {
        return clip.length / source.pitch;
    }
}
