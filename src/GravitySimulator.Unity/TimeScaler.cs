using UnityEngine;

namespace GravitySimulator.Unity {

    public class TimeScaler : Updatable {

        [Min(0f)]
        public float TimeScale = 1f;

        protected override void BetterAwake() {
            base.BetterAwake();

            RegisterUpdatesAutomatically = true;
            BetterUpdate = doUpdate;
        }

        private void doUpdate(float fixedDeltaTime) => Time.timeScale = TimeScale;

    }

}
