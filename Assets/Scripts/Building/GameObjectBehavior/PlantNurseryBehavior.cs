﻿using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Resources;
using JetBrains.Annotations;
using Map;
using Ressource;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Building.GameObjectBehavior
{
    /// <summary>
    ///     Omportement de la pépinière.
    /// </summary>
    public class PlantNurseryBehavior : MonoBehaviour
    {
        private BuildingManager _buildingManager;
        public float Interval;
        public float Radius;
        public GameObject Tree;
        public float WaitingTime;

        /// <summary>
        ///     Méthode appeler à l'instanciation du GameObject.
        /// </summary>
        private void Start()
        {
            InvokeRepeating(nameof(PlantTree), WaitingTime, Interval);
            _buildingManager = BuildingManager.GetInstance();
        }

        /// <summary>
        ///     Plante un arbre sur la case la plus proche.
        /// </summary>
        private void PlantTree()
        {
            var tile = SelectNearestEmptySpace();
            if (tile != null)
            {
                var tree = Instantiate(Tree, new Vector3(tile.X, 1, tile.Z), Quaternion.identity);

                if (tile.Doodad)
                {
                    Destroy(tile.Doodad);
                    tile.Doodad = null;
                }

                tile.Resource = tree;
                tile.Resource.AddComponent<NavMeshObstacle>().carving = true;
                tile.ResourceEnum = ResourceEnum.Wood;
            }
        }

        /// <summary>
        ///     Récupère la case libre la plus proche.
        /// </summary>
        /// <returns>Modèle de case.</returns>
        [CanBeNull]
        private TileModel SelectNearestEmptySpace()
        {
            var tileArray = _buildingManager.TileArray;
            var nearestResources = new List<NearestResource>();
            for (var i = 0; i < tileArray.GetLength(0); i++)
            for (var j = 0; j < tileArray.GetLength(1); j++)
                if (!tileArray[i, j].Resource && !tileArray[i, j].Building &&
                    tileArray[i, j].TileEnum == TileEnum.Land)
                {
                    var distance = Vector3.Distance(new Vector3(i, 1, j), transform.position);
                    if (distance < Radius)
                        nearestResources.Add(new NearestResource
                        {
                            Distance = distance,
                            Tile = tileArray[i, j]
                        });
                }

            return nearestResources.FirstOrDefault(w => w.Distance == nearestResources.Min(m => m.Distance))?.Tile;
        }
    }
}