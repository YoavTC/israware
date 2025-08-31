using _Game_Assets.Scripts.Definitions;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

namespace _Game_Assets.Scripts.Screen_Handlers
{
    public class ScoreScreen : BaseScreen
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private MMF_Player feedback;

        private int score;

        public override void Show(bool won)
        {
            if (won)
            {    
                screenParent.SetActive(true);
                feedback.PlayFeedbacks();
                score++;
            }
        }

        public void UpdateScoreText()
        {
            scoreText.text = score.ToString();
        }

        public override void Hide()
        {
            screenParent.SetActive(false);
        }
    }
}