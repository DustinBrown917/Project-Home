using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HOME
{
    public class MilestoneGraphic : MonoBehaviour
    {
        [SerializeField] private Image outline;
        [SerializeField] private Image plaque;
        [SerializeField] private Image actualGraphic;
        [SerializeField] private Color unachievedColour;
        [SerializeField] private Color achievedColour;
        private bool achieved = false;
        private Vector3 startPosition;
        private Vector3 homePosition;

        private Coroutine cr_Achievement = null;
        private Coroutine cr_MovePos = null;
        private Coroutine cr_waitTimer = null;

        private void Start()
        {
            homePosition = transform.position;
            Unachieve();
        }

        public void SetImage(Sprite sprite)
        {
            actualGraphic.sprite = sprite;
        }

        public void SetStartPosition(Vector3 pos)
        {
            startPosition = pos;
        }

        public void Achieve()
        {
            if(achieved) { return; }
            achieved = true;
            outline.color = achievedColour;
            plaque.color = plaque.color = new Color(plaque.color.r, plaque.color.g, plaque.color.b, 1.0f);
            actualGraphic.enabled = true;
            CoroutineManager.BeginCoroutine(WaitThenMove(), ref cr_waitTimer, this);
        }

        public void Unachieve()
        {
            outline.color = unachievedColour;
            plaque.color = new Color(plaque.color.r, plaque.color.g, plaque.color.b, unachievedColour.a);
            actualGraphic.enabled = false;
        }

        private IEnumerator WaitThenMove()
        {
            transform.position = startPosition;
            outline.transform.localScale = new Vector3(3, 3, 3);
            yield return new WaitForSeconds(1.0f);
            CoroutineManager.BeginCoroutine(CREffects.ZoomToScale(outline.transform, outline.transform.localScale, Vector3.one, 0.2f), ref cr_Achievement, this);
            CoroutineManager.BeginCoroutine(CREffects.ZoomToPosition(transform, transform.position, homePosition, 0.2f), ref cr_MovePos, this);
        }
    }


}

