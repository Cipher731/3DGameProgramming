using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Circle : MonoBehaviour
{
    public int MaxParticles;
    public float Radius;
    public float Amplitude;
    public float EmissionTime;
    public float StartSize;

    private ParticleSystem _ps;
    private ParticleCoordinate[] _coordinates;
    private ParticleSystem.Particle[] _particles;
    private int _currentParticles;

    private class ParticleCoordinate
    {
        private float _rho;
        private readonly float _initialRho;
        private float _theta;
        private readonly int _direction;
        private float _time;

        public ParticleCoordinate(float rho, float theta, bool clockwise)
        {
            _rho = rho * Random.Range(0.9f, 1.1f);
            _initialRho = _rho;
            _theta = theta;
            _direction = clockwise ? 1 : -1;
            _time = Random.Range(0, 10f);
        }

        public Vector3 GetCartesianCoordnate()
        {
            return new Vector3(_rho * Mathf.Cos(_theta), _rho * Mathf.Sin(_theta), 0);
        }

        public void Rotate(float degree, bool ignoreDir = true)
        {
            _theta += (!ignoreDir ? _direction : -1) * degree * Random.Range(0, 1f);
        }

        public void PingPong(float amplitude)
        {
            _rho = _initialRho + Mathf.PingPong(_time * amplitude / 5, amplitude) - amplitude / 2;
            _time += Time.deltaTime;
        }
    }

    private void Awake()
    {
        _coordinates = new ParticleCoordinate[MaxParticles];
        _particles = new ParticleSystem.Particle[MaxParticles];

        _ps = GetComponent<ParticleSystem>();

        var mainModule = _ps.main;
        mainModule.startSpeed = 0;
        mainModule.startSize = StartSize;
        mainModule.maxParticles = MaxParticles;
        mainModule.startLifetime = float.MaxValue;
        mainModule.loop = false;
    }

    private void UpdateParticles()
    {
        for (var i = 0; i < _currentParticles; i++)
        {
            _particles[i].position = _coordinates[i].GetCartesianCoordnate();
        }
    }

    private void FixedUpdate()
    {
        if (_currentParticles >= MaxParticles) return;

        var newParticles = (int) (MaxParticles * Time.fixedDeltaTime / EmissionTime);
        if (newParticles + _currentParticles > MaxParticles)
        {
            newParticles = MaxParticles - _currentParticles;
        }

        _ps.Emit(newParticles);
        _ps.GetParticles(_particles);

        var offset = 0.25f * _currentParticles / MaxParticles - 1 / 6f;
        var minFactor = Mathf.Sqrt(1f * _currentParticles / MaxParticles) + offset;
        var maxFactor = minFactor + 1 / 6f;

        for (var i = 0; i < newParticles; i++)
        {
            _coordinates[_currentParticles + i] =
                new ParticleCoordinate(Radius,
                    Random.Range(minFactor, maxFactor) * -2 * Mathf.PI,
                    Random.Range(0, 1f) < 0.5f);
        }

        _currentParticles += newParticles;
    }

    private void Update()
    {
        for (var i = 0; i < _currentParticles; i++)
        {
            _coordinates[i].Rotate(0.01f / Mathf.PI, _currentParticles < MaxParticles);
            _coordinates[i].PingPong(Amplitude);
        }

        UpdateParticles();
        _ps.SetParticles(_particles, MaxParticles);
    }
}