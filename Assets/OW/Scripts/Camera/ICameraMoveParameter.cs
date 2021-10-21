using UniRx;
using UnityEngine;

namespace OW.Scripts.Player
{
    public interface ICameraMoveParameter
    {
        ReactiveProperty<Vector3> TargetPosition { get; }
    }
}
