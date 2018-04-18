using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace Util
{
    public class PlateFactory
    {
        public static readonly PlateFactory Instance = new PlateFactory();
        private static GameObject _platePrefab;

        private readonly Queue<GameObject> _availablePlates = new Queue<GameObject>(20);
        private readonly List<GameObject> _generatedPlates = new List<GameObject>(20);

        private PlateFactory()
        {
        }

        // ReSharper disable once MemberCanBePrivate.Global
        private GameObject GetRawPlate()
        {
            GameObject plate = null;

            if (_availablePlates.Count > 0)
            {
                plate = _availablePlates.Dequeue();
            }

            if (plate != null)
            {
                plate.SetActive(true);
                return plate;
            }
            
            if (_platePrefab == null)
            {
                _platePrefab = Resources.Load<GameObject>("Prefab/Plate");
            }

            plate = Object.Instantiate(_platePrefab);
            plate.layer = LayerMask.NameToLayer("Plate");
                
            _generatedPlates.Add(plate);

            return plate;
        }

        // Get a plate with random config.
        public GameObject GetRandomPlate()
        {
            var rawPlate = GetRawPlate();
            (rawPlate.GetComponent<PlateConfig>() ?? rawPlate.AddComponent<PlateConfig>()).Randomize();
            return rawPlate;
        }

        // Retrieve currently useless plate for further usage.
        public void RetrievePlate(Transform plate)
        {
            plate.gameObject.SetActive(false);
            _availablePlates.Enqueue(plate.gameObject);
        }

        public bool ActivePlateExists()
        {
            return _generatedPlates.Any(item => item && item.activeSelf);
        }
    }
}