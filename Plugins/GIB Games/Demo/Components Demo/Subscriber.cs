using UnityEngine;
using GIB;

namespace GIB.EventsProDemo
{
    public class Subscriber : MonoBehaviour
    {
        // Start is called before the first frame update
        public UnityEngine.Events.UnityAction FooBar;
        void Start()
        {
            FooBar = Bar;
            GIB.EventTriggers.EventController.Subscribe("foo", FooBar);
        }

        // Update is called once per frame
        void Bar()
        {

        }
    }
}
