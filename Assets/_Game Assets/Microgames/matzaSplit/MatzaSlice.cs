using UnityEngine;
using UnityEngine.U2D;

namespace _Game_Assets.Microgames.matzaSplit
{
    public class MatzaSlice : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private SpriteRenderer sr;
        [SerializeField] private SpriteShapeController ssc;

        private MatzaManager mm;
        
        void Start()
        {
            if (sr == null) sr = GetComponent<SpriteRenderer>();
            if (ssc == null) ssc = GetComponentInChildren<SpriteShapeController>();
        }

        public void SetMatzaManager(MatzaManager mm)
        {
            this.mm = mm;
        }

        public void SetPolygon(Vector2[] vertices, bool other)
        {
            // Build spline for mask
            Spline spline = ssc.spline;
            spline.Clear();

            foreach (Vector2 point in vertices)
            {
                spline.InsertPointAt(0, new Vector3(point.x * 2 - 1, point.y * 2 - 1));
            }

            sr.maskInteraction = other ? SpriteMaskInteraction.VisibleOutsideMask : SpriteMaskInteraction.VisibleInsideMask;
        }

        private void OnMouseDown()
        {
            mm.OnMatzaSelected(this);
        }

        public void Hide()
        {
            sr.enabled = false;
        }

        public void Show()
        {
            sr.enabled = true;
        }

        internal void Reset()
        {
            transform.rotation.Set(0, 0, 0, 0);
        }
    }
}
