using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;
using UnityUtils;

namespace _Game_Assets.Microgames.splitRedSea
{
    public class CutPlane : MonoBehaviour
    {
        [SerializeField] private float cuttingPointDistanceThreshold;
        [SerializeField] private List<Vector2> points;
        private Vector2 lastPoint;
        
        private bool isCutting, isDoneCutting;
        private Vector2 startCuttingPos, endCuttingPos;
        
        [SerializeField] private GameObject planeToCut;
        [SerializeField] private Camera mainCamera;
        private Vector2 mousePosition;
        
        [SerializeField] private Vector2 planeUpperCorner, planeLowerCorner;
        [SerializeField] private Vector2 planeUpperMiddle, planeLowerMiddle;

        [SerializeField] private Material material;

        private void Start()
        {
            isCutting = false;
            isDoneCutting = false;
            
            startCuttingPos = Vector2.zero;
            endCuttingPos = Vector2.zero;
            
            points.Clear();
        }

        void Update()
        {
            mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            
            // Start cutting
            if (!isCutting && Input.GetMouseButtonDown(0))
            {
                isCutting = Physics2D.Raycast(mousePosition, Vector2.zero);
                if (isCutting)
                {
                    startCuttingPos = mousePosition;
                    AddPoint(mousePosition);
                }
            }
            
            // While cutting
            if (isCutting && !isDoneCutting && Input.GetMouseButton(0))
            {
                if (Vector2.Distance(lastPoint, mousePosition) > cuttingPointDistanceThreshold)
                {
                    AddPoint(mousePosition);
                }
            }

            // End cutting
            if (isCutting && Input.GetMouseButtonUp(0))
            {
                endCuttingPos = mousePosition;
                isDoneCutting = true;
                AddPoint(mousePosition);
                CutMesh();
            }
        }

        private void AddPoint(Vector2 point)
        {
            points.Add(point);
            lastPoint = point;
        }

        private void CutMesh()
        {
            points.Insert(0, GetClosestPointOnPlane(points[0]));
            points.Insert(points.Count - 1, GetClosestPointOnPlane(points.Last()));
            
            
            Spline spline = planeToCut.GetComponent<SpriteShapeController>().spline;
            
            for (int i = 0; i < points.Count - 1; i++)
            {
                spline.InsertPointAt(0, points[i]);
            }
            
            int lastIndex = spline.GetPointCount() - 1;

            for (int i = 0; i < 4; i++)
            {
                Debug.Log("Removed");
                spline.RemovePointAt(spline.GetPointCount() - 1);
            }
            
            spline.InsertPointAt(0, GetCloserCorner(spline.GetPosition(0)));
            spline.InsertPointAt(spline.GetPointCount(), GetCloserCorner(spline.GetPosition(spline.GetPointCount() - 1)));
        }

        private Vector2 GetCloserCorner(Vector2 point)
        {
            return Vector2.Distance(point, planeUpperCorner) < Vector2.Distance(point, planeLowerCorner) ? planeUpperCorner : planeLowerCorner;
        }

        private Vector2 GetClosestPointOnPlane(Vector2 point)
        {
            return Vector2.Distance(point, planeUpperMiddle) < Vector2.Distance(point, planeLowerMiddle) ? planeUpperMiddle : planeLowerMiddle;
        }


        [SerializeField] private float debugDrawRadius = 0.1f;
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(mainCamera.ScreenPointToRay(Input.mousePosition).origin, debugDrawRadius);

            Gizmos.color = Color.green;
            for (int i = 0; i < points.Count; i++)
            {
                if (i < points.Count - 1)
                {
                    Gizmos.DrawLine(points[i], points[i + 1]);
                }
            }
        }
    }
}
