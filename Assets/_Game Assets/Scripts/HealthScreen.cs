using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Game_Assets.Scripts
{
    public class HealthScreen : BaseScreen
    {
        [SerializeField] private GridLayoutGroup heartsLayoutGroup;
        private List<Image> hearts;

        private void Awake()
        {
            hearts = heartsLayoutGroup.GetComponentsInChildren<Image>().ToList();
        }

        public override void Show(bool won)
        {
            screenParent.SetActive(true);
            screenParent.transform.DOPunchScale(Vector3.one, 0.2f);
            heartsLayoutGroup.startCorner = heartsLayoutGroup.transform.childCount % 2 == 0 ? GridLayoutGroup.Corner.UpperLeft : GridLayoutGroup.Corner.UpperRight;

            if (won) return;
            
            var heart = hearts[0];
            hearts.RemoveAt(0);
                
            heart.transform.DOPunchScale(Vector3.one, 0.2f)
                .SetDelay(1f)
                .OnComplete(() =>
                {
                    Destroy(heart.gameObject);
                });

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