using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GIB.Auspex;
using UnityEngine.Events;


namespace GIB.EventsProDemo
{
    public class ButtonTestScript : MonoBehaviour
    {
        // This shows a normal Infobox
        [InfoBox("This is an infobox!", EInfoBoxType.Normal)]
        
        // This shows a warning
        [InfoBox("This is a warning infobox!", EInfoBoxType.Warning)]

        // This shows an error
        [InfoBox("This is an error Infobox!", EInfoBoxType.Error)]

        // A range slider between 0 and 10
        [Range(0, 10f)] public float sliderFloat;

        [Button]
        public void Test()
        {
            Debug.Log("Test a thing");
        }

    }
}


