using UnityEngine;
using UnityEngine.UI;

namespace GIB.EventsProDemo
{
    public class FireMissile : MonoBehaviour
    {
        [SerializeField] private bool unreadyOnChange;

        [SerializeField] private Image commanderImage;
        [SerializeField] private Text codeText;
        [SerializeField] private Text boostText;
        [SerializeField] private Text speedText;
        [SerializeField] private Text ResultText;
        [SerializeField] private Text commanderName;
        [SerializeField] private Text commanderId;

        [SerializeField] private SliderPro boostSlider;
        [SerializeField] private ScrollbarPro speedScroll;
        [SerializeField] private DropdownPro commanderDrop;
        [SerializeField] private TogglePro readyToggle;
        [SerializeField] private InputFieldPro approvalInput;
        [SerializeField] private ButtonPro fireButton;

        // A function with a float and bool argument.
        public void SetMissileBoost(float boostLevel, bool readyToFire)
        {
            boostText.text = "Missile has a boost level of\n" + boostLevel.ToString("F2");
            if (unreadyOnChange)
                readyToggle.SetIsOnWithoutNotify(readyToFire);

        }

        // Another function with a float and bool argument.
        public void SetMissileSpeed(float speedLevel, bool readyToFire)
        {
            speedText.text = "Missile has a speed of\n" +speedLevel.ToString("F2");
            if (unreadyOnChange)
                readyToggle.SetIsOnWithoutNotify(readyToFire);
        }

        // A function with a bool and an int.
        public void FireMissileAtTarget(bool readyToFire, int MissileId)
        {

        }

        // A function with an int, string, Sprite, and bool argument.
        public void FireMissileFromCommand(int value, string name, Sprite commander, bool readyToFire)
        {
            commanderImage.sprite = commander;
            commanderImage.color = Color.white;
            commanderName.text = $"Cmdr. {name}";
            commanderId.text = $"Power level: {value}";
            if (unreadyOnChange)
                readyToggle.SetIsOnWithoutNotify(readyToFire);
        }

        // A function with a string and bool argument.
        public void SetMissileCode(string foo, bool readyToFire)
        {
            codeText.text = "Missile code\n"+foo;
            if (unreadyOnChange)
                readyToggle.SetIsOnWithoutNotify(readyToFire);
        }

        // A function with a GameObject argument.
        public void FireMissileAtGameObject(GameObject go)
        {

        }

        // A function with a bool argument.
        public void SetUnreadyOnChange(bool state)
        {
            unreadyOnChange = state;
        }

        // Another function with a bool argument.
        public void FireMissileNow()
        {
            string resultString = "";

            if (boostSlider.value < 0.15f)
                resultString += "Not enough boost!\n";
            if (speedScroll.value < 0.25f)
                resultString += "Not enough Speed!\n";
            if (commanderDrop.value == 0)
                resultString += "No commander selected!\n";
            if (approvalInput.text == "")
                resultString += "No code entered!\n";
            if (!readyToggle.isOn)
                resultString += "Not ready to fire!\n";

            if (resultString == "")
            {
                resultString = "Firing missile!";
                Debug.Log("Boom");
            }
            ResultText.text = resultString;
        }

        // A function with a Vector2 and bool argument.
        public void FireMissileAtVector(Vector2 vector, bool isOn)
        {
            
        }

        // A function with 10 arguments.
        public void FireComplicatedMissile(int missileId, string targetName, Sprite targetSprite, Vector3 vector, Transform location, float otherfloat, Collider collider, Vector3 direction, Quaternion rotation)
        {
            Debug.Log("Boom");
        }
    }
}
