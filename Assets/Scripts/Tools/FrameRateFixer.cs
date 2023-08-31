using UnityEngine;

namespace MasterParking
{
    public static class FrameRateFixer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void FixFrameRate()
        {
            Application.targetFrameRate = 60;
        }
    }
}