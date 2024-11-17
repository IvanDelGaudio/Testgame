using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraManagement
{
    [RequireComponent(typeof(Camera))]
    public class OrbitCamera : MonoBehaviour
    {
        #region Public structs
        [System.Serializable]
        public struct AxesInversion
        {
            public bool invertX;
            public bool invertY;
        }
        #endregion
        #region Public enums
        public enum OffsetSpace
        {
            World,
            TargetLocal
        }
        #endregion
        #region Public variables
        [Header("Focus")]
        public Transform target = null;
        public OffsetSpace offsetSpace = OffsetSpace.World;
        public Vector3 targetOffset = Vector3.up;
        [Min(0.01f)]
        public float targetFollowDistance = 10.0f;
        [Tooltip("How long it will take to reach the target position when following")]
        public float followSmoothTime = 0.25f;
        [Tooltip("How long it will take to center the focus point when looking at")]
        public float lookAtSmoothTime = 1.0f;

        [Range(0.0f, 90.0f)]
        public float maxUpAngle = 50.0f;
        [Range(0.0f, -90.0f)]
        public float maxDownAngle = -50.0f;

        [Header("Collision check")]
        public LayerMask collisioncheckLayers = Physics.AllLayers;
        public float probeSize = 0.1f;

        [Header("Axes CFG")]
        public Vector2 mouseSensitivity = Vector2.one;
        public Vector2 padSensitivity = Vector2.one * 75.0f;
        public Vector2 padDrag = Vector2.one * 150.0f;
        public AxesInversion invertMouseAxes;
        public AxesInversion invertPadAxes;
        #endregion
        #region Private variables
        //	Cached reference to the camera component on this game object (assigned on awake)
        private Camera cam = null;

        private float angleAroundVertical = 0.0f;
        private float currentVelocityAroundVertical = 0.0f;
        private float angleAroundRight = 0.0f;
        private float currentVelocityAroundRight = 0.0f;

        //	Support variables for smooth damp functions
        private Vector3 moveVelocity = Vector3.zero;
        private Vector3 rotateVelocity = Vector3.zero;
        #endregion
        void Awake()
        {
            cam = GetComponent<Camera>();
        }
        void LateUpdate()
        {
            //	Reduce inertia
            currentVelocityAroundVertical = Mathf.MoveTowards(currentVelocityAroundVertical, 0, Time.deltaTime * padDrag.x);
            currentVelocityAroundRight = Mathf.MoveTowards(currentVelocityAroundRight, 0, Time.deltaTime * padDrag.y);

            //	Apply input
            //...mouse
            AddRotationAroundVertical(Input.GetAxis("Look Horizontal") * mouseSensitivity.x * (invertMouseAxes.invertX ? -1 : 1));
            AddRotationAroundRight(Input.GetAxis("Look Vertical") * mouseSensitivity.y * (invertMouseAxes.invertY ? -1 : 1));
            //...pad
            AddRotationRateAroundVertical(Input.GetAxis("Look Horizontal Rate") * padSensitivity.x * (invertPadAxes.invertX ? -1 : 1));
            AddRotationRateAroundRight(-/* to add coherence between hw */Input.GetAxis("Look Vertical Rate") * padSensitivity.y * (invertPadAxes.invertY ? -1 : 1));

            //	Take current target's position in world space
            Vector3 focusPoint;

            //	Apply target offset
            switch (offsetSpace)
            {
                case OffsetSpace.World:
                    focusPoint = target.position + targetOffset;
                    break;
                case OffsetSpace.TargetLocal:
                    focusPoint = target.transform.TransformPoint(targetOffset);
                    break;
                default:
                    Debug.LogError($"{offsetSpace} not supported");
                    focusPoint = Vector3.zero;
                    break;
            }

            //	Calculate look direction
            Vector3 lookDirection = Vector3.forward;
            lookDirection = Quaternion.AngleAxis(-angleAroundRight, Vector3.right) * lookDirection;
            lookDirection = Quaternion.AngleAxis(angleAroundVertical, Vector3.up) * lookDirection;
            //	SAME AS
            //Vector3 lookDirection = Quaternion.AngleAxis(angleAroundVertical, Vector3.up) * Quaternion.AngleAxis(-angleAroundRight, Vector3.right) * Vector3.forward;

            //	Place camera
            transform.position = focusPoint - lookDirection * targetFollowDistance;

            //	Orient camera
            transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);

            //	Check collisions
            RaycastHit hitInfo;
            Vector3 distance = transform.position - focusPoint;
            Ray ray = new Ray(focusPoint, distance.normalized);
            bool hitAnything = Physics.SphereCast(
                ray,
                probeSize,
                out hitInfo,
                distance.magnitude,
                collisioncheckLayers
            );
            if (hitAnything)
            {
                //	Vector3 center = hitInfo.point	NOOOOOO!
                //	Vector3 center = hitInfo.point + hitInfo.normal * probeSize	// NI
                Vector3 center = ray.origin + ray.direction * hitInfo.distance;
                transform.position = center;
            }
        }

        private void AddRotationAroundVertical(float degrees)
        {
            //	Apply angle offset
            angleAroundVertical += degrees;

            //	Wrap angle
            angleAroundVertical = (angleAroundVertical + 360) % 360;
        }
        private void AddRotationRateAroundVertical(float degreesPerSecond)
        {
            //	Apply inertia
            AddRotationAroundVertical(currentVelocityAroundVertical * Time.deltaTime);

            //	Boost inertia
            if (Mathf.Abs(degreesPerSecond) < 0.1f)
                return;
            if (Mathf.Sign(degreesPerSecond) != Mathf.Sign(currentVelocityAroundVertical))
                currentVelocityAroundVertical = degreesPerSecond;
            else
                currentVelocityAroundVertical = Mathf.Sign(degreesPerSecond) * Mathf.Max(Mathf.Abs(currentVelocityAroundVertical), Mathf.Abs(degreesPerSecond));
        }

        private void AddRotationAroundRight(float degrees)
        {
            //	Apply angle offset
            angleAroundRight += degrees;

            //	Clamp angle
            angleAroundRight = Mathf.Clamp(angleAroundRight, maxDownAngle, maxUpAngle);
        }
        private void AddRotationRateAroundRight(float degreesPerSecond)
        {
            //	Apply inertia
            AddRotationAroundRight(currentVelocityAroundRight * Time.deltaTime);

            //	Boost inertia
            if (Mathf.Abs(degreesPerSecond) < 0.1f)
                return;
            if (Mathf.Sign(degreesPerSecond) != Mathf.Sign(currentVelocityAroundRight))
                currentVelocityAroundRight = degreesPerSecond;
            else
                currentVelocityAroundRight = Mathf.Sign(degreesPerSecond) * Mathf.Max(Mathf.Abs(currentVelocityAroundRight), Mathf.Abs(degreesPerSecond));
        }
    }
}
