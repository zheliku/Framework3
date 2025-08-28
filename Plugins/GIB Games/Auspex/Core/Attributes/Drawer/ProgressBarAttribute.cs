using System;

namespace GIB.Auspex
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ProgressBarAttribute : DrawerAttribute
    {
        public string Name { get; private set; }
        public float MaxValue { get; set; }
        public string MaxValueName { get; private set; }
        public ButtonColors Color { get; private set; }

        public ProgressBarAttribute(string name, float maxValue, ButtonColors color = ButtonColors.Blue)
        {
            Name = name;
            MaxValue = maxValue;
            Color = color;
        }

        public ProgressBarAttribute(string name, string maxValueName, ButtonColors color = ButtonColors.Blue)
        {
            Name = name;
            MaxValueName = maxValueName;
            Color = color;
        }

        public ProgressBarAttribute(float maxValue, ButtonColors color = ButtonColors.Blue)
            : this("", maxValue, color)
        {
        }

        public ProgressBarAttribute(string maxValueName, ButtonColors color = ButtonColors.Blue)
            : this("", maxValueName, color)
        {
        }
    }
}
