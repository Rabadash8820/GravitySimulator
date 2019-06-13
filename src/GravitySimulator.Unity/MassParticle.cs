using UnityEngine;
using UnityEngine.Assertions;

namespace GravitySimulator.Unity {

    public class MassParticle : MonoBehaviour {

        private GravitySimulator _simulator;

        public Transform RootTransform;
        public Rigidbody Rigidbody;
        public SphereCollider Collider;
        public TrailRenderer TrailRenderer;

        private void Awake() {
            DependencyInjector.ResolveDependenciesOf(this);

            Assert.IsNotNull(RootTransform, this.GetAssociationAssertion(nameof(RootTransform)));
            Assert.IsNotNull(Rigidbody, this.GetAssociationAssertion(nameof(Rigidbody)));
            Assert.IsNotNull(Collider, this.GetAssociationAssertion(nameof(Collider)));
            Assert.IsNotNull(TrailRenderer, this.GetAssociationAssertion(nameof(TrailRenderer)));
        }
        private void OnEnable() => _simulator.AddParticle(this);
        private void OnDisable() => _simulator.RemoveParticle(this);

        public void Inject(GravitySimulator simulator) => _simulator = simulator;

    }

}
