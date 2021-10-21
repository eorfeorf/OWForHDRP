using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace City
{
    [ExecuteAlways]
    public class ProceduralBuildings : MonoBehaviour
    {
        [SerializeField] private BuildingScriptableObject OrignalPrefabs; 
        [SerializeField] private int Floors; // 階数.
        [SerializeField] private Vector3 Size = new Vector3(1, 1, 1);    
    
    
        private GameObject wall = default;
        private GameObject wallFront = default;
        private GameObject roof = default;

        private void Awake()
        {
        }

        private void Start()
        {
        }

        private void Rebuild()
        {
            // サイズ.
            // 階層.
            // 屋上.
        }
    }
}