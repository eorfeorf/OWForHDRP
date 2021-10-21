using OW.Scripts.Camera;
using UnityEngine;
using UnityEngine.Assertions;

namespace OW.Scripts.Managers
{
    public class StageManager : MonoBehaviour
    {
        private UnityEngine.Camera camera = null;
        private Player.Player player = null;

        private void Awake()
        {
            camera = FindObjectOfType<UnityEngine.Camera>();
            Assert.IsNotNull(camera);
            player = FindObjectOfType<Player.Player>();
            Assert.IsNotNull(player);
        }

        private void Start()
        {
            var cameraMover = camera.GetComponent<CameraMover>();
            Assert.IsNotNull(cameraMover);
            player.Initialize(cameraMover);
            cameraMover.Initialize(player.transform.rotation.eulerAngles);
        }
    }
}
