using UnityEngine;

namespace City
{
    [CreateAssetMenu(fileName = "Building", menuName = "City/BuildingScriptableObject", order = 1)]
    public class BuildingScriptableObject : ScriptableObject
    {
        public GameObject OrigWall;
        public GameObject OrigWallFront;
        public GameObject OrigRoof;
    }
}