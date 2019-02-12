using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOME
{
    [CreateAssetMenu(fileName = "New Milestons", menuName = "Milestone")]
    public class MileStone : ScriptableObject
    {
        [SerializeField] private string milestoneName;
        [SerializeField] private string milestoneDescription;
        [SerializeField] private Sprite _sprite;
        public Sprite sprite { get { return _sprite; } }
        [SerializeField] private float milestoneDistance;
        public float GoalDistance { get { return milestoneDistance; } }
        private bool _achieved = false;
        public bool Achieved { get { return _achieved; } }

        public void Achieve(int playerIndex)
        {
            _achieved = true;
            UIManager.Instance.BroadCastLowImpact(playerIndex, milestoneDescription, true);
            UIManager.Instance.PlayAchievementSound();
        }

        public void Unachieve()
        {
            _achieved = false;
        }

    }
}

