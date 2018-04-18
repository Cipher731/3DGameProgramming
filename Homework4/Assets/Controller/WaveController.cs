using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using Util;
using Random = UnityEngine.Random;

namespace Controller
{
    public class WaveController : MonoBehaviour
    {
        private class Counter
        {
            private readonly int _min;
            private readonly int _max;

            public Counter(int min, int max)
            {
                _min = min;
                _max = max;
            }

            public int GetRandomValue()
            {
                return new System.Random().Next(_min, _max);
            }
        }

        private readonly PlateFactory _factory = PlateFactory.Instance;

        // The generator of number of plates to generate per wave.
        private readonly Counter _platesPerWave = new Counter(10, 15);
        private bool _coroutineBusy;

        public void EmitPlates()
        {
            StartCoroutine(EmitPlatesCoroutine());
        }

        private IEnumerator EmitPlatesCoroutine()
        {
            _coroutineBusy = true;

            var plateNum = _platesPerWave.GetRandomValue();
            for (var i = 0; i < plateNum; i++)
            {
                var plate = _factory.GetRandomPlate();
                var left = Random.Range(0, 2) == 0;
                plate.transform.position = new Vector3(left ? -15f : 15f, Random.Range(1f, 3f), 2);

                var config = plate.GetComponent<PlateConfig>();

                plate.GetComponent<Renderer>().material.color = config.Color.GetRawColor();
                plate.transform.localScale = config.Size.GetRawSizeScale() * new Vector3(1.5f, 0.2f, 1f);
                var speed = config.Speed.GetRawSpeed();

                var movement = plate.GetComponent<PlateMovement>() ?? plate.AddComponent<PlateMovement>();
                movement.Speed = speed * new Vector3(left ? 1f : -1f, Random.Range(-0.2f, 0.2f), 0) *
                                 (float) Math.Pow(1.1, GameController.Instance.CurrentRound);

                yield return new WaitForSeconds(1f - 0.8f / GameController.Instance.TotalRound *
                                                GameController.Instance.CurrentRound);
            }

            _coroutineBusy = false;
        }

        private void Update()
        {
            if (!Input.GetMouseButtonDown((int) MouseButton.LeftMouse)) return;

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            LayerMask mask = LayerMask.GetMask("Plate");
            if (Physics.Raycast(ray, out hit, mask))
            {
                HitPlate(hit.transform);
            }
        }

        private void HitPlate(Transform plate)
        {
            GameController.Instance.Score += plate.GetComponent<PlateConfig>().GetScore();
            _factory.RetrievePlate(plate);
        }

        public bool IsIdle()
        {
            return !_factory.ActivePlateExists() && !_coroutineBusy;
        }
    }
}