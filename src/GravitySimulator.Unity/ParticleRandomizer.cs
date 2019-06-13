using System.Collections.Generic;
using UnityEngine;

namespace GravitySimulator.Unity {

    public class ParticleRandomizer : MonoBehaviour {

        private RandomNumberGenerator _rand;

        public float MaxXDistanceFromOrigin = 25f;
        public float MaxYDistanceFromOrigin = 2f;
        public float MaxZDistanceFromOrigin = 25f;
        public float MinSpeed = 0f;
        public float MaxSpeed = 20f;
        [Min(0f)]
        public float MinScale = 0.5f;
        [Min(0f)]
        public float MaxScale = 3f;
        public float Density = 10f;

        private void Awake() {
            DependencyInjector.ResolveDependenciesOf(this);
        }

        public void Inject(RandomNumberGenerator rand) {
            _rand = rand;
        }
        public void RandomizeParticles(IList<MassParticle> particles, Transform origin) {
            for (int rb = 0; rb < particles.Count; ++rb) {
                MassParticle particle = particles[rb];

                float speed = Random.Range(MinSpeed, MaxSpeed);
                particle.Rigidbody.velocity = speed * Random.insideUnitSphere;

                float x = Random.Range(0f, MaxXDistanceFromOrigin);
                float y = Random.Range(0f, MaxYDistanceFromOrigin);
                float z = Random.Range(0f, MaxZDistanceFromOrigin);
                particle.Rigidbody.position = origin.TransformPoint(x, y, z);

                float scale = Random.Range(MinScale, MaxScale);
                particle.RootTransform.localScale = scale * Vector3.one;
                particle.Rigidbody.SetDensity(Density);
            }
        }

    }

}
