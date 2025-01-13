using System.Collections;
using System.Collections.Generic;
using UnityEditor.Sprites;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace Line_Compass
{
    public class Line_Compass : MonoBehaviour
    {
        #region Public Variables
        int i = 0;
        public List<Transform> doors;
        public Transform character;
        // public Transform door;
        public RectTransform comapass;
        public RectTransform objectiveMarker;
        public Camera cam;
        public Text mDistance;
        [Range(0.10f, 360.0f)]
        public float fildOfView = 180;
        #endregion
        #region Private Variables
        private void SetMarkerPosition(RectTransform Marker, Transform objectDoor)
        {
            float distance = Vector3.Distance(character.position, objectDoor.position);
            Vector3 view = cam.transform.forward;
            Vector3 viewDistance = objectDoor.position - cam.transform.position;
            distance = Mathf.FloorToInt(distance);
            mDistance.text = $"{distance} m";
            view.y = 0.0f;
            viewDistance.y = 0.0f;
            float angle = Vector3.SignedAngle(view, viewDistance, Vector3.up);
            float positionMarker = Mathf.InverseLerp(-(fildOfView / 2), (fildOfView / 2), angle);
            positionMarker = (positionMarker * 2) - 1;
            positionMarker *= comapass.rect.width / 2;
            Marker.anchoredPosition = new Vector2(positionMarker, 0);
        }
        private void CreateMarker()
        {
            Instantiate(objectiveMarker);
        }
        #endregion
        #region Lifecycle
        private void Awake()
        {
            Debug.Log(doors.Count);
            int a = doors.Count;
            for (int i = 0; i < a; i++)
            {
                Debug.Log(doors.Count);
                CreateMarker();
            }
        }
        private void Update()
        {
            /*
            Vector2 directionToMarker = new Vector2(door.transform.position.x-cam.transform.position.x, door.transform.position.z - cam.transform.position.z);
            float distace = (door.transform.position.x - character.transform.position.x) + (door.transform.position.z - character.transform.position.z);
            float angle = Vector2.Angle(directionToMarker,new Vector2 (cam.transform.forward.x,cam.transform.forward.z));
            float marker = Mathf.Clamp(angle, minMarker, maxMarker);
            Debug.Log(marker);
             */
            //float distance = Vector3.Distance(character.position, door.position);
            //Vector3 view = cam.transform.forward;
            //Vector3 viewDistance = door.position - cam.transform.position;
            //distance=Mathf.FloorToInt(distance);
            //mDistance.text = $"{distance} m";
            //view.y = 0.0f;
            //viewDistance.y = 0.0f;
            //
            //float angle = Vector3.SignedAngle(view, viewDistance, Vector3.up);
            //float positionMarker = Mathf.InverseLerp(-(fildOfView / 2), (fildOfView / 2), angle);
            //positionMarker = (positionMarker * 2) - 1;
            //positionMarker *= comapass.rect.width/2;
            //objectiveMarker.anchoredPosition = new Vector2(positionMarker,0);
            //foreach (Transform doors in transform)
            //{
            // SetMarkerPosition(objectiveMarker, door);
            //}

        }
        #endregion
    }
}
