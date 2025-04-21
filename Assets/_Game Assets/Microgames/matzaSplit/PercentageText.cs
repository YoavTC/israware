using System;
using TMPro;
using UnityEngine;

namespace _Game_Assets.Microgames.matzaSplit
{
    public class PercentageText : MonoBehaviour
    {

        [SerializeField] private Animator animator;
        [SerializeField] private TextMeshProUGUI textMeshPro;


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (animator == null) animator = GetComponent<Animator>();
            if (textMeshPro == null) textMeshPro = GetComponent<TextMeshProUGUI>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void Show(float percentage, bool correct) {
            textMeshPro.SetText(String.Format("{0:0.00}", percentage * 100) + "%");
            textMeshPro.color = correct ? Color.green : Color.red;
            gameObject.transform.position = Input.mousePosition;
            animator.Play("show");
        }
    }
}
