using System;
using OW.Scripts.Camera;
using UniRx;
using UnityEngine;

namespace OW.Scripts.Player
{
    public enum Status
    {
        Stay,
        Walk,
        Run,
        Jump,
    }

    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] private float walkSpeed = 2.0f; //移動速度
        [SerializeField] private float jumpHeight = 1.0f; //ジャンプ速度
        [SerializeField] private float gravity = -9.81f; //重力
        [SerializeField] private float runSpeed = 3f;

        public IReactiveProperty<Status> State => new ReactiveProperty<Status>(Scripts.Player.Status.Stay);

        private CharacterController characterController;
        private bool isGrounded;
        private float playerVelocityY;

        // アニメーション.
        public IObservable<bool> IsMove => isMove;
        private readonly Subject<bool> isMove = new();
        public IObservable<Unit> Jumped => jumped;
        private readonly Subject<Unit> jumped = new();
        public IObservable<float> MoveRatio => moveRatio;
        private readonly Subject<float> moveRatio = new();

        // 外部依存.
        private ICameraMoveParameter cameraMoveParameter;
        private Quaternion cameraMoverRotation;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
        }

        public void Initialize(CameraMover cameraMover)
        {
            // ICameraMoveParameter
            cameraMoveParameter = cameraMover;

            // ICameraToPlayerParameter
            ICameraToPlayerParameter cameraToPlayerParameter = cameraMover;
            cameraToPlayerParameter.CameraRotation.Subscribe(q => { cameraMoverRotation = q; }).AddTo(this);
        }

        private void Update()
        {
            var isJump = false;

            isGrounded = characterController.isGrounded;

            if (isGrounded && playerVelocityY <= 0f)
            {
                playerVelocityY = 0f;
            }

            var run = GetRunSpeed();
            var move = GetInputAxis();
            var moveWalk = cameraMoverRotation * move.normalized * walkSpeed;
            var moveRun = cameraMoverRotation * move.normalized * run;
            move = moveWalk + moveRun;
            characterController.Move(move * Time.deltaTime);

            if (move != Vector3.zero)
            {
                transform.forward = move;
            }

            if (GetInputJump() && isGrounded)
            {
                playerVelocityY += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
                isJump = true;
            }

            playerVelocityY += gravity * Time.deltaTime;
            characterController.Move(new Vector3(0, playerVelocityY, 0) * Time.deltaTime);

            // 状態.
            UpdateState(move.magnitude, walkSpeed, isJump);

            // アニメーション
            UpdateAnimation(move.magnitude, walkSpeed, runSpeed, isJump);


            // 外部パラメータ.
            cameraMoveParameter.TargetPosition.Value = transform.position;
        }

        private void UpdateState(float moveLength, float walkSpeed, bool isJump)
        {
            if (!isJump && playerVelocityY <= 0f)
            {
                if (moveLength <= 0f)
                {
                    State.Value = Status.Stay;
                }
                else if (moveLength <= walkSpeed)
                {
                    State.Value = Status.Walk;
                }
                else
                {
                    State.Value = Status.Run;
                }
            }
            else
            {
                State.Value = Status.Jump;
            }
        }

        private void UpdateAnimation(float moveLength, float walkSpeed, float runSpeed, bool isJump)
        {
            if (!isJump)
            {
                isMove.OnNext(moveLength != 0f);
                moveRatio.OnNext(moveLength / (walkSpeed + runSpeed));
            }
            else
            {
                jumped.OnNext(Unit.Default);
            }
        }


        #region Input

        private Vector3 GetInputAxis()
        {
#if true
            float retH = 0f;
            if (Input.GetKey(KeyCode.A))
            {
                retH += -1f;
            }

            if (Input.GetKey(KeyCode.D))
            {
                retH += 1f;
            }

            float retV = 0f;
            if (Input.GetKey(KeyCode.S))
            {
                retV += -1f;
            }

            if (Input.GetKey(KeyCode.W))
            {
                retV += 1f;
            }

            return new Vector3(retH, 0f, retV).normalized;
#else
            return new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
#endif
        }

        private float GetRunSpeed()
        {
            return Input.GetKey(KeyCode.LeftShift) ? runSpeed : 0f;
        }

        private bool GetInputJump()
        {
            return Input.GetButtonDown("Jump");
        }

        #endregion
    }
}