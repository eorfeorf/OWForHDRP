using UniRx;
using UnityEngine;

namespace OW.Scripts.Camera
{
    public interface ICameraToPlayerParameter
    {
        IReactiveProperty<Quaternion> CameraRotation { get; }
    }
}
