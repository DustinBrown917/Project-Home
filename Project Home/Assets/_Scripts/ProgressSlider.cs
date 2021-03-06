﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HOME
{
    public class ProgressSlider : MonoBehaviour
    {
        private Slider slider;

        [SerializeField] private MilestoneGraphic[] milestoneGraphics;
        [SerializeField] private Transform imageZoomStartPosition;

        private float awardInterval;


        private void Awake()
        {
            slider = GetComponent<Slider>();
            slider.onValueChanged.AddListener(HandleSliderChanged);
        }

        private void Start()
        {
            for(int h = 0; h < PlayerManager.PlayerCount; h++)
            {
                Player p = PlayerManager.GetPlayer(h);
                for (int i = 0; i < p.MileStones.Length; i++)
                {
                    milestoneGraphics[i].SetImage(p.MileStones[i].sprite);
                    milestoneGraphics[i].SetStartPosition(imageZoomStartPosition.position);
                }
                awardInterval = slider.maxValue / milestoneGraphics.Length;
            }
        }

        private void HandleSliderChanged(float value)
        {
            int currentIndex = (int)(value / awardInterval);
            if(currentIndex - 1 >= 0 && currentIndex - 1 < milestoneGraphics.Length){
                milestoneGraphics[currentIndex - 1].Achieve();
            }
        }
    }
}

