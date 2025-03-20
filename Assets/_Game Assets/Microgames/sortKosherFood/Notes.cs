using UnityEngine;
using Random = External_Packages.Random;

namespace _Game_Assets.Microgames.sortKosherFood
{
    public class Notes : MonoBehaviour
    {
        [SerializeField] private Transform[] notesTransforms;
        [SerializeField] private Vector2[] notesPositions;
        [SerializeField] private DraggingController draggingController;
        
        void Start()
        {
            bool reverse = Random.RandomBool();

            if (reverse)
            {
                notesTransforms[0].position = notesPositions[1];
                notesTransforms[1].position = notesPositions[0];
            }

            draggingController.kosherOnRight = reverse;
        }
    }
}
