using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GIB.Auspex;
using UnityEngine.Events;

namespace GIB.EventsProDemo
{
    public class SomeButton : MonoBehaviour
    {
        public EventPro OnPushButton;

        public bool ArmSystem;

        // These will only show in Inspector if ArmSystem is true.
        [ShowIf("ArmSystem")]
        public bool SwitchA;
        [ShowIf("ArmSystem")]
        public bool SwitchB;

        // This will only show in Inspector if both SwitchA and SwitchB is true.
        [ShowIf("BothSwitches")]
        public FrequencySet Frequency;

        // This will only show in Inspector if Frequency is set to Cosmic.
        [ShowIf("Frequency", FrequencySet.Cosmic)]
        public int ConfirmationCode;

        // This will only show in Inspector if the confirmation code is set to "1234".
        [ShowIf("CodeIsRight")]
        [Button]
        public void DoThing()
        {
            OnPushButton.Invoke();
        }

        private bool BothSwitches()
        {
            return SwitchA && SwitchB;
        }

        private bool CodeIsRight()
        {
            return ConfirmationCode == 1234;
        }

        public enum FrequencySet
        {
            Low,
            High,
            Uber,
            Cosmic,
            Grandma
        }
    }
}
