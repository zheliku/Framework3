using UnityEngine;

namespace GIB.Auspex
{
    public enum ButtonColors
    {
        Clear,
        White,
        Black,
        Gray,
        Red,
        Pink,
        Orange,
        Yellow,
        Green,
        Blue,
        Indigo,
        Violet
    }

    public static class EColorExtensions
    {
        public static Color GetColor(this ButtonColors color)
        {
            switch (color)
            {
                case ButtonColors.Clear:
                    return new Color32(0, 0, 0, 0);
                case ButtonColors.White:
                    return new Color32(255, 255, 255, 255);
                case ButtonColors.Black:
                    return new Color32(0, 0, 0, 255);
                case ButtonColors.Gray:
                    return new Color32(128, 128, 128, 255);
                case ButtonColors.Red:
                    return new Color32(255, 150, 150, 255);
                case ButtonColors.Pink:
                    return new Color32(255, 152, 203, 255);
                case ButtonColors.Orange:
                    return new Color32(255, 210, 120, 255);
                case ButtonColors.Yellow:
                    return new Color32(255, 211, 0, 255);
                case ButtonColors.Green:
                    return new Color32(180, 255, 180, 255);
                case ButtonColors.Blue:
                    return new Color32(180, 180, 255, 255);
                case ButtonColors.Indigo:
                    return new Color32(75, 0, 130, 255);
                case ButtonColors.Violet:
                    return new Color32(210, 170, 255, 255);
                default:
                    return new Color32(0, 0, 0, 255);
            }
        }
    }
}
