using System.Collections.Generic;
using System.Linq;
using _Game_Assets.Scripts.Definitions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Game_Assets.Scripts.Screen_Handlers
{
    public class HealthScreen : BaseScreen
    {
        [SerializeField] private GridLayoutGroup heartsLayoutGroup;
        private List<Image> hearts;

        [SerializeField] private UnityEvent heartPoppedUnityEvent;

        private void Awake()
        {
            hearts = heartsLayoutGroup.GetComponentsInChildren<Image>().ToList();
        }

        public override void Show(bool won)
        {
            screenParent.SetActive(true);
            // screenParent.transform.DOPunchScale(Vector3.one, 0.2f);
            heartsLayoutGroup.startCorner = heartsLayoutGroup.transform.childCount % 2 == 0 ? GridLayoutGroup.Corner.UpperLeft : GridLayoutGroup.Corner.UpperRight;

            if (!won)
            {
                heartPoppedUnityEvent?.Invoke();
            }
        }

        public void DestroyHeart()
        {
            var heart = hearts[0];
            hearts.RemoveAt(0);

            Destroy(heart.gameObject);

            if (hearts.Count == 0)
            {
                stateMachine.ChangeState(State.DEATH);
            }
        }

        public override void Hide()
        {
            screenParent.SetActive(false);
        }
    }
}