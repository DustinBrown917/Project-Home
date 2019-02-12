using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOME
{
    public class OptionsMessenger : MonoBehaviour
    {
        public void SetDancepadMode(bool dancepadMode)
        {
            GameOptions.SetDancepadMode(dancepadMode);
        }
    }
}

