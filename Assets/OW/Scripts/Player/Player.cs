using OW.Scripts.Camera;
using UniRx;
using UnityEngine;

namespace OW.Scripts.Player
{
    public class Player : MonoBehaviour
    {
        public PlayerMover Mover { get; private set; }

        // 外部依存.
        private ReactiveProperty<Vector3> cameraMoverPlayerPosition;
        
        private void Awake()
        {
            Mover = GetComponent<PlayerMover>();
        }

        public void Initialize(CameraMover cameraMover)
        {
            Mover.Initialize(cameraMover);
        }
    }
}
