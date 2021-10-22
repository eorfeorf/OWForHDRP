using System;
using OW.Scripts.Player;
using UniRx;
using UnityEngine;

namespace OW.Scripts.Camera
{
    public class CameraMover : MonoBehaviour, ICameraMoveParameter, ICameraToPlayerParameter
    {
        [SerializeField] private bool invertY = false;
        [SerializeField] private float mouseSensitivity = 60.0f;
        [SerializeField] private float length = 3f;
        [SerializeField] private float maxAngleAxisX = 80f;
        [SerializeField] private float rotateSpeed = 2f;
        [SerializeField] private Vector3 adjustTargetPosition;

        private Vector3 rot;
        private Vector3 playerPosition;
        private static float maxAxisX;
        
        // ICameraToPlayerParameter
        public IReactiveProperty<Quaternion> CameraRotation => cameraRotation;
        private ReactiveProperty<Quaternion> cameraRotation = new ReactiveProperty<Quaternion>();
        
        // ICameraMoveParameter
        public ReactiveProperty<Vector3> TargetPosition { get; } = new ReactiveProperty<Vector3>();
        private Vector3 targetPos;
        
        // const
        const float k_MouseSensitivityMultiplier = 0.01f;

        private void Awake()
        {
            maxAxisX = maxAngleAxisX;
        }

        private void Start()
        {
            TargetPosition.Subscribe(pos =>
            {
                targetPos = pos + adjustTargetPosition;
            }).AddTo(this);
        }

        private void Update()
        {
            // 右クリックでカメラを回転させる.
            if (IsRightMouseButtonDown())
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            if (IsRightMouseButtonUp())
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

            // if (IsCameraRotationAllowed())
            {
                var mouseMovement = GetInputLookRotation() * k_MouseSensitivityMultiplier * mouseSensitivity * rotateSpeed;
                if (invertY)
                    mouseMovement.y = -mouseMovement.y;

                rot.x += mouseMovement.x;
                rot.y += mouseMovement.y;
                
                rot.x = Mathf.Clamp(rot.x, -maxAxisX, maxAxisX);
                

                // 座標用に計算
                var q = Quaternion.Euler(rot);
                
                // 座標計算.
                var localPos = q * (-Vector3.forward * length);
                transform.position = targetPos + localPos;
                
                // 回転計算.
                transform.LookAt(targetPos);
                
                
                // 外部パラメータ.
                var playerMoveQ = Quaternion.Euler(new Vector3(0f, rot.y, rot.z));
                cameraRotation.SetValueAndForceNotify(playerMoveQ);
            }
        }

        public void Initialize(Vector3 initRot)
        {
            rot = initRot;
        }
        

        #region method

        private static Vector2 GetInputLookRotation()
        {
            // try to compensate the diff between the two input systems by multiplying with empirical values
#if ENABLE_INPUT_SYSTEM
            var delta = lookAction.ReadValue<Vector2>();
            delta *= 0.5f; // Account for scaling applied directly in Windows code by old input system.
            delta *= 0.1f; // Account for sensitivity setting on old Mouse X and Y axes.
            return delta;
#else
#if true
            var axisY = 0f;
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                axisY += 1;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                axisY += -1;
            }

            var axisX = 0f;
            if (Input.GetKey(KeyCode.DownArrow))
            {
                axisX += -1;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                axisX += 1;
            }

            return new Vector2(axisX, axisY);
#else
            return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
#endif
#endif
        }

        private static bool IsCameraRotationAllowed()
        {
#if ENABLE_INPUT_SYSTEM
            bool canRotate = Mouse.current != null ? Mouse.current.rightButton.isPressed : false;
            canRotate |= Gamepad.current != null ? Gamepad.current.rightStick.ReadValue().magnitude > 0 : false;
            return canRotate;
#else
            return Input.GetMouseButton(1);
#endif
        }

        private static bool IsRightMouseButtonDown()
        {
#if ENABLE_INPUT_SYSTEM
            return Mouse.current != null ? Mouse.current.rightButton.isPressed : false;
#else
            return Input.GetMouseButtonDown(1);
#endif
        }

        private static bool IsRightMouseButtonUp()
        {
#if ENABLE_INPUT_SYSTEM
            return Mouse.current != null ? !Mouse.current.rightButton.isPressed : false;
#else
            return Input.GetMouseButtonUp(1);
#endif
        }

        #endregion

    }
}