using System;
namespace WhispConditions {
    public class IsRandomValHigherThan : Condition {

        private float highestValue;
        private float targetValue;

        public IsRandomValHigherThan(float highestValue, float targetValue) {
            this.highestValue = highestValue;
            this.targetValue = targetValue;
        }

        protected override bool MyCondition() {
            return UnityEngine.Random.Range(0, highestValue) > targetValue;
        }
    }
}
