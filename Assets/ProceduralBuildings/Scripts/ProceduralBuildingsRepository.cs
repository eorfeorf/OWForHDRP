using System;
using System.Collections;
using System.Collections.Generic;
using City;
using UnityEngine;
using UnityEngine.Pool;

namespace ProceduralBuildings
{
    public class ProceduralBuildingsRepository : MonoBehaviour
    {
        private List<GameObject> rooms = new List<GameObject>(1000);

        IObjectPool<Room> pool;

        private void Awake()
        {
            pool = new ObjectPool<Room>(OnCreatePool,OnGetPool,OnReleasePool,OnDestroyPool,true, 100);
        }

        private Room OnCreatePool()
        {
            throw new NotImplementedException();
        }

        private void OnGetPool(Room obj)
        {
            throw new NotImplementedException();
        }

        private void OnReleasePool(Room obj)
        {
            throw new NotImplementedException();
        }

        private void OnDestroyPool(Room obj)
        {
            throw new NotImplementedException();
        }

        // public void Init(BuildingScriptableObject originalPrefabs)
        // {
        //     foreach(var w in walls)
        //     {
        //         walls.Add(Instantiate(originalPrefabs.OrigWall, transform)); 
        //     }
        //
        //     foreach (var w in wallFronts)
        //     {
        //         wallFronts.Add(Instantiate(originalPrefabs.OrigWallFront, transform));
        //     }
        //
        //     foreach (var w in roofs)
        //     {
        //         roofs.Add(Instantiate(originalPrefabs.OrigRoof, transform));
        //     }
        // }
    }
}