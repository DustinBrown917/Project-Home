using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOME
{
    public static class GameOptions
    {
        private static bool dancepadMode = true;
        private static float difficulty = 1.0f;

        public static bool GetDancepadMode()
        {
            return dancepadMode;
        }

        public static void SetDancepadMode(bool mode)
        {
            dancepadMode = mode;
        }

        public static float GetDifficulty()
        {
            return difficulty;
        }

        public static void SetDifficulty(float diff)
        {
            difficulty = diff;
        }
    }
}

