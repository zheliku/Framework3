using GIB.Auspex;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GIB.EventsProDemo
{
    public class ButtonExamples : MonoBehaviour
    {
        [BoxGroup("This is a Box Group")]
        [SerializeField] private int blah;
        [BoxGroup("This is a Box Group")]
        [SerializeField] private int otherBlah;

        [SerializeField] private int thirdBlah;

        [Button(buttonSize: ButtonSizes.Standard)]
        public void StandardButton() { }

        [Button(buttonSize: ButtonSizes.Standard, buttonColor: ButtonColors.Red)]
        public void StandardRedButton() { }

        [Button(buttonSize: ButtonSizes.Standard, buttonColor: ButtonColors.Blue)]
        public void StandardBlueButton() { }

        [Button(buttonSize: ButtonSizes.Large)]
        public void LargeButton() { }

        [Button(buttonSize: ButtonSizes.Huge)]
        public void HugeButton() { }
    }
}
