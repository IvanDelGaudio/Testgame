using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

using S = System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CharacterMovement
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    public class CharacterControllerMover : MonoBehaviour
    {
        #region Public enums
        public enum OrientMode : byte
        {
            None,
            Movement,
            LookDirection
        }
        #endregion
        #region Private variables
        private CharacterController characterController = null;

        [Header("Anchors")]
        [SerializeField]
        private Transform renderRoot;

        [Header("Move params")]
        [SerializeField]
        [Min(0.25f)]
        private float speed = 5.0f;
        [SerializeField]
        [Min(0.0f)]
        private float jumpSpeed = 10.0f;
        [SerializeField]
        private OrientMode orientMode = OrientMode.Movement;
        [SerializeField]
        [Min(0.0f)]
        private float orientReachTime = 0.15f;
        private Vector3 orientCurrentSpeed = Vector3.zero;

        [Header("Debug")]
        [SerializeField]
        bool useSimpleMove = false;
        [SerializeField]
        bool useCustomFloorDetection = false;

        [Header("Floor detection params")]
        [SerializeField]
        [Range(0f, 0.1f)]
        private float floorDetectionOffset = 0.001f;
        private float verticalSpeed = 0.0f;
        #endregion
        #region Private properties
        private bool isGrounded
        {
            get
            {
                if (useCustomFloorDetection)
                {
                    Ray ray = new Ray(
                        transform.TransformPoint(characterController.center) - transform.up * (0.5f * characterController.height - characterController.radius),
                        Vector3.down
                    );
                    return Physics.SphereCast(
                        ray,
                        characterController.radius,
                        characterController.skinWidth + floorDetectionOffset
                    );
                }
                else
                    return characterController.isGrounded;
            }
        }
        #endregion
        #region Lifecycle
        void Awake()
        {
            characterController = GetComponent<CharacterController>();
        }
        void Start()
        {
            //	Compensate the skin width by pushing the render model down and make it match the floor
            if (renderRoot != null)
                renderRoot.position -= transform.up * characterController.skinWidth;
        }
#if UINTY_EDITOR
	void OnGUI()
	{
		GUILayout.Box(isGrounded ? "Grounded" : "Not Grounded");
		GUILayout.Box($"Vertical speed: {verticalSpeed:0.00}m/s");
	}
#endif
        void FixedUpdate()
        {
            /*
			 * We're manipulating dilated modifications to the
			 * vertical speed, which is a physical quantity, in
			 * the fixed update to make it more stable.
			 */

            //	Reset vertical speed when touching ground
            if (isGrounded)
                verticalSpeed = 0.0f;

            //	Apply gravity
            verticalSpeed -= 9.81f * Time.deltaTime;
        }
        void Update()
        {
            //	Apply jump
            if (
                Input.GetButtonDown("Jump") &&
                isGrounded
            )
                verticalSpeed += jumpSpeed;

            //	Handle aim input
            if (Input.GetButtonDown("Fire2"))
                orientMode = OrientMode.LookDirection;
            else if (Input.GetButtonUp("Fire2"))
                orientMode = OrientMode.Movement;

            //	Calculate move direction
            Vector3 moveDirection = Vector3.zero;
            Transform cam = Camera.main.transform;
            moveDirection += Input.GetAxis("Horizontal") * cam.right;
            moveDirection += Input.GetAxis("Vertical") * cam.forward;

            //	Planarize and keep original magnitude
            float moveMagnitude = moveDirection.magnitude;
            moveDirection.y = 0.0f;
            moveDirection.Normalize();
            moveDirection *= moveMagnitude;

            //	Apply movement
            Move(moveDirection * speed);

            //	Orient to movement
            if (orientMode != OrientMode.None)
            {
                //	Prepare target rotation
                Quaternion targetRotation = transform.rotation;

                //	Fill target rotation depending on the orient mode
                switch (orientMode)
                {
                    case OrientMode.Movement:
                        if (moveDirection.sqrMagnitude > Mathf.Epsilon)
                            targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                        break;
                    case OrientMode.LookDirection:
                        Vector3 cameraLookDirection = Camera.main.transform.forward;
                        cameraLookDirection.y = 0.0f;
                        if (cameraLookDirection.sqrMagnitude > Mathf.Epsilon)
                            targetRotation = Quaternion.LookRotation(cameraLookDirection, Vector3.up);
                        break;
                    default:
                        Debug.LogError($"Orient mode {orientMode} is not handled");
                        break;
                }

                //	Smooth rotate to target rotation
                transform.rotation = SmoothDampRotation(
                    transform.rotation, //	FROM
                    targetRotation, //	TO
                    ref orientCurrentSpeed, //	Speed (updated by teh function itself)
                    orientReachTime //	How long the rotation reaches the target)
                );
            }
        }
        #endregion
        #region Public methods
        public void Move(Vector3 direction)
        {
            if (useSimpleMove)
                characterController.SimpleMove(direction);
            else
            {
                direction += Vector3.up * verticalSpeed;
                characterController.Move(direction * Time.deltaTime);
            }
        }
        #endregion
        #region Private static methods
        private static Quaternion SmoothDampRotation(Quaternion from, Quaternion to, ref Vector3 currentVelocity, float smoothTime)
        {
            //	Quat -> Euler
            Vector3 fromEuler = from.eulerAngles;
            Vector3 toEuler = to.eulerAngles;

            //	Smooth damp angle, axis by axis
            Vector3 smoothedAngle = new Vector3(
                Mathf.SmoothDampAngle(fromEuler.x, toEuler.x, ref currentVelocity.x, smoothTime),
                Mathf.SmoothDampAngle(fromEuler.y, toEuler.y, ref currentVelocity.y, smoothTime),
                Mathf.SmoothDampAngle(fromEuler.z, toEuler.z, ref currentVelocity.z, smoothTime)
            );

            //	Euler -> Quat
            return Quaternion.Euler(smoothedAngle);
        }
        #endregion
    }
}

