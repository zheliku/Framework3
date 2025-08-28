using System;

namespace GIB.Auspex
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class HorizontalLineAttribute : DrawerAttribute
    {
        public const float DefaultHeight = 2.0f;
        public const ButtonColors DefaultColor = ButtonColors.Gray;

        public float Height { get; private set; }
        public ButtonColors Color { get; private set; }

        public HorizontalLineAttribute(float height = DefaultHeight, ButtonColors color = DefaultColor)
        {
            Height = height;
            Color = color;
        }
    }
}
