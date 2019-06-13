﻿using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace GravitySimulator.Unity {

    public class GravitySimulator : Updatable {

        private MultiSpawner _spawner;
        private ParticleRandomizer _randomizer;

        private IList<MassParticle> _particles = new List<MassParticle>();
        private float _tSinceTick = 0f;

        public float TickPeriod = 0.1f;
        public float GravitationalConstant = 5;
        [Min(0f)]
        public float TimeScale = 1f;

        protected override void BetterAwake() {
            base.BetterAwake();

            RegisterUpdatesAutomatically = true;
            BetterFixedUpdate = doFixedUpdate;
        }
        private void Start() {
            _spawner.Spawn();
            _randomizer.RandomizeParticles(_particles, _spawner.transform);
        }

        public void Inject(MultiSpawner spawner, ParticleRandomizer randomizer) {
            _spawner = spawner;
            _randomizer = randomizer;
        }
        [Button]
        public void ToggleTrails() {
            for (int p = 0; p < _particles.Count; ++p)
                _particles[p].TrailRenderer.enabled = !_particles[p].TrailRenderer.enabled;
        }

        private void doFixedUpdate(float fixedDeltaTime) {
            Time.timeScale = TimeScale;

            // Only continue if another tick has passed...
            _tSinceTick += fixedDeltaTime;
            if (_tSinceTick < TickPeriod)
                return;
            _tSinceTick -= TickPeriod;

            // Calculate gravitational forces
            var _forces = new Vector3[_particles.Count];
            Vector3 vectBw, forceBw;
            float distBw, forceMag;
            int p0, p1;
            for (p0 = 0; p0 < _particles.Count; ++p0) {
                for (p1 = p0 + 1; p1 < _particles.Count; ++p1) {
                    vectBw = (_particles[p1].Rigidbody.position - _particles[p0].Rigidbody.position);
                    distBw = vectBw.magnitude;
                    forceMag = GravitationalConstant * _particles[p0].Rigidbody.mass * _particles[p1].Rigidbody.mass / (distBw * distBw);
                    forceBw = forceMag * vectBw / distBw;
                    _forces[p0] += forceBw;
                    _forces[p1] -= forceBw;
                }
            }

            // Apply gravitational forces
            for (p0 = 0; p0 < _particles.Count; ++p0)
                _particles[p0].Rigidbody.AddForce(_forces[p0]);
        }

        public void AddParticle(MassParticle particle) => _particles.Add(particle);
        public void RemoveParticle(MassParticle particle) => _particles.Remove(particle);

    }

}