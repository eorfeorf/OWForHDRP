using System;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace OW.Scripts.Player
{
    [RequireComponent(typeof(PlayerMover))]
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        
        private PlayerMover mover;
        
        private static readonly int IsMove = Animator.StringToHash("IsMove");
        private static readonly int JumpTrigger = Animator.StringToHash("JumpTrigger");
        private static readonly int MoveRatio = Animator.StringToHash("MoveRatio");

        private void Awake()
        {
            mover = GetComponent<PlayerMover>();
        }

        private void Start()
        {
            mover.IsMove.Subscribe(isMove =>
            {
                animator.SetBool(IsMove, isMove);
                // Debug.Log(isMove);
            }).AddTo(this);

            mover.MoveRatio.Subscribe(moveRatio =>
            {
                animator.SetFloat(MoveRatio, moveRatio);
            }).AddTo(this);
            
            mover.Jumped.Subscribe(jumped =>
            {
                animator.SetTrigger(JumpTrigger);
            }).AddTo(this);
        }
    }
}
