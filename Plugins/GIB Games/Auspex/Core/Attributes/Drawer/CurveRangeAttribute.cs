using System;
using UnityEngine;

namespace GIB.Auspex
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class CurveRangeAttribute : DrawerAttribute
    {
        public Vector2 Min { get; private set; }
        public Vector2 Max { get; private set; }
        public ButtonColors Color { get; private set; }

        public CurveRangeAttribute(Vector2 min, Vector2 max, ButtonColors color = ButtonColors.Clear)
        {
            Min = min;
            Max = max;
            Color = color;
        }

        public CurveRangeAttribute(ButtonColors color)
            : this(Vector2.zero, Vector2.one, color)
        {
        }

        public CurveRangeAttribute(float minX, float minY, float maxX, float maxY, ButtonColors color = ButtonColors.Clear)
            : this(new Vector2(minX, minY), new Vector2(maxX, maxY), color)
        {
        }
    }
}
