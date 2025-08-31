using _Game_Assets.Scripts.Reusables;
using DG.Tweening;
using External_Packages.Extra_Components;
using TMPro;
using UnityEngine;

namespace _Game_Assets.Scripts
{
    public class DeathManager : MonoBehaviour
    {
        [SerializeField] private TweenSettings deathTweenSettings;

        [Header("Audio")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private float deathTransitionTargetPitch;

        [Header("UI")]
        [SerializeField] private TMP_Text deathText;
        [SerializeField] private float deathTextTargetScale;
        [SerializeField] private TweenSettings deathTextScaleTweenSettings;
        [SerializeField] private TweenSettings deathTextFadeTweenSettings;
        [Space]
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private string scoreTextFormat;
        [SerializeField] private float scoreTextTargetScale;
        [SerializeField] private TweenSettings scoreTextScaleTweenSettings;


        void Start()
        {
            // Show cursor
            Cursor.visible = true;

            // Get score from player pref
            int score = PlayerPrefs.GetInt("_SCORE", 0);
            scoreText.text = string.Format(scoreTextFormat, score);

            // Slow pitch down
            musicSource.DOPitch(deathTransitionTargetPitch, deathTweenSettings.duration)
                .SetAs(deathTweenSettings.GetParams());

            // Fade in & scale death text
            deathText.DOFade(1f, deathTextFadeTweenSettings.duration).SetAs(deathTextFadeTweenSettings.GetParams());
            deathText.transform.DOScale(deathTextTargetScale, deathTextScaleTweenSettings.duration).SetAs(deathTextScaleTweenSettings.GetParams());

            // Scale score text
            scoreText.transform.DOScale(scoreTextTargetScale, scoreTextScaleTweenSettings.duration).SetAs(scoreTextScaleTweenSettings.GetParams());
        }
    }
}
