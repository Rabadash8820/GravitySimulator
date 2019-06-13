using UnityEngine;

namespace GravitySimulator.Unity {

    [CreateAssetMenu(menuName = "GravitySimulator.Unity/" + nameof(SimulationConfig), fileName = "simulation-config")]
    public class SimulationConfig : ScriptableObject {

        public float TickPeriod = 0.1f;
        public float GravitationalConstant = 5;
    }

}
