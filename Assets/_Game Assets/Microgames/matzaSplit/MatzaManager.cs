using System;
using _Game_Assets.Microgames.matzaSplit;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.U2D;

public class MatzaManager : MonoBehaviour
{

    [SerializeField] private Animator animator;

    [Header("Matza Components")]
    [SerializeField] private MatzaSlice matzaLeft;
    [SerializeField] private MatzaSlice matzaRight;

    [Header("Text")]
    [SerializeField] private PercentageText percentageText;

    [Header("Split properties")]
    [SerializeField] int CUT_DETAIL_AMOUNT_MIN = 5;
    [SerializeField] int CUT_DETAIL_AMOUNT_MAX = 7;
    [SerializeField] float CUT_DEPTH_PERCENTAGE = 0.3f;

    [Header("Events")]
    [SerializeField] private UnityEvent<bool> matzaChosenUnityEvent;

    private Vector2[] slicePolygon;
    private MatzaSlice biggerMatza;
    private Boolean interactable;
    private float area;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (animator == null) animator = gameObject.GetComponent<Animator>();

        if (matzaLeft == null) matzaLeft = gameObject.transform.GetChild(0).GetComponent<MatzaSlice>();
        if (matzaRight == null) matzaRight = gameObject.transform.GetChild(1).GetComponent<MatzaSlice>();

        matzaLeft.setMatzaManager(this);
        matzaRight.setMatzaManager(this);

        interactable = false;

        //RemoveSpriteMasks();
        BringMatza();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AllowInteraction() {
        interactable = true;
    }

    internal void OnMatzaSelected(MatzaSlice matzaSlice)
    {
        if (biggerMatza == null || !interactable) return;

        bool choiceCorrect = biggerMatza == matzaSlice;

        matzaChosenUnityEvent.Invoke(choiceCorrect);

        if (!choiceCorrect) PlayWrongAnimation(matzaSlice);
        else PlayCorrectAnimation(matzaSlice);

        percentageText.Show(matzaSlice == matzaLeft ? area : 1-area, choiceCorrect);

        interactable = false;
    }

    void PlayWrongAnimation(MatzaSlice matzaSlice)
    {
        animator.Play(matzaSlice == matzaLeft ? "matzaThrowLeft" : "matzaThrowRight");
        animator.Play(matzaSlice == matzaLeft ? "matzaFadeRight" : "matzaFadeLeft");
    }
    void PlayCorrectAnimation(MatzaSlice matzaSlice)
    {
        animator.Play(matzaSlice == matzaLeft ? "matzaBringInLeft" : "matzaBringInRight");
        animator.Play(matzaSlice == matzaLeft ? "matzaFadeRight" : "matzaFadeLeft");
    }

    void BringMatza()
    {
        RemoveSpriteMasks();
        animator.Play("FlyIn");
        biggerMatza = null;

        matzaLeft.Reset();
        matzaRight.Reset();
    }

    void RemoveSpriteMasks()
    {
        matzaLeft.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
        matzaRight.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;

        matzaRight.Hide();
    }

    void CreatePolygonAndSetSpriteMasks()
    {
        int cuts = UnityEngine.Random.Range(CUT_DETAIL_AMOUNT_MIN, CUT_DETAIL_AMOUNT_MAX) + 2;
        int triangleAmount = (cuts - 1) * 2;
        area = 0;

        slicePolygon = new Vector2[2 + cuts];

        slicePolygon[0] = new Vector2(0, 1f);
        slicePolygon[1] = new Vector2(0, 0f);

        float x;
        float last_x = -1;
        for (int cut_i = 0; cut_i < cuts; cut_i++)
        {
            x = 0.5f + UnityEngine.Random.Range(-0.5f, 0.5f) * CUT_DEPTH_PERCENTAGE;

            if (last_x != -1)
                area += (last_x + x) / 2f / (cuts - 1);

            slicePolygon[2 + cut_i] = new Vector2(
                x,// * (cut_i % 2 == 0 ? 1 : -1),
                (float)cut_i / (cuts - 1)
            );

            last_x = x;
        }


        matzaLeft.SetPolygon(slicePolygon, false);
        matzaRight.SetPolygon(slicePolygon, true);

        matzaRight.Show();

        biggerMatza = area > 0.5f ? matzaLeft : matzaRight;
    }
}
