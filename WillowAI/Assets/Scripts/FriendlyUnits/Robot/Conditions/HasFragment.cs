using UnityEngine;
using System.Collections;

namespace FragmentCollector {

    public class HasFragment : InstanceBoundCondition<IFragmentCollector> {
        public HasFragment(IFragmentCollector target) : base(target) {
        }

        protected override bool MyCondition() {
            return target.FragmentsInPosessionCount > 0;
        }
    }

}