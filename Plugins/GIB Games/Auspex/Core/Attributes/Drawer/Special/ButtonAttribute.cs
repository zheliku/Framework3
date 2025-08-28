using System;
using UnityEngine;

namespace GIB.Auspex
{

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ButtonAttribute : SpecialCaseDrawerAttribute
    {
        public string Text { get; private set; }
        public ButtonMode SelectedEnableMode { get; private set; }

        public const ButtonColors DefaultColor = ButtonColors.Clear;
        public Color ButtonColor { get; private set; }
        public ButtonSizes ButtonSize { get; private set; }

        public ButtonAttribute(string text = null, ButtonMode enabledMode = ButtonMode.AlwaysEnable, string buttonColor = default, ButtonSizes buttonSize = ButtonSizes.Standard)
        {
            this.Text = text;
            this.SelectedEnableMode = enabledMode;
            this.ButtonSize = buttonSize;
            if (buttonColor == default)
            {
                this.ButtonColor = Color.white;
            }
            else
            {
                Color tempColor = Color.white;
                ColorUtility.TryParseHtmlString(buttonColor, out tempColor);
                this.ButtonColor = tempColor;
            }
        }

        public ButtonAttribute(ButtonColors buttonColor, string text = null, ButtonMode enabledMode = ButtonMode.AlwaysEnable, ButtonSizes buttonSize = ButtonSizes.Standard)
        {
            this.Text = text;
            this.SelectedEnableMode = enabledMode;
            this.ButtonSize = buttonSize;
            this.ButtonColor = buttonColor == DefaultColor ? Color.white : buttonColor.GetColor();
        }
    }

    public enum ButtonMode
    {
        /// <summary>
        /// Button should be active always
        /// </summary>
        AlwaysEnable,
        /// <summary>
        /// Button should be active only in editor
        /// </summary>
        EditorOnly,
        /// <summary>
        /// Button should be active only in playmode
        /// </summary>
        PlaymodeOnly
    }

    public enum ButtonSizes
    {
        Standard,
        Large,
        Huge
    }
}
