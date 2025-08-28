// This Attribute Processor is used in projects containing Odin Inspector,
// and allows Odin to draw certain elements without conflict.
#if ODIN_INSPECTOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GIB.Auspex;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using System;
using System.Reflection;

public class AuspexAttributeProcessor : OdinAttributeProcessor
{
    public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
    {
        var currentAttributeList = new List<Attribute>(attributes);
        foreach (var attr in currentAttributeList)
        {
            if (attr.GetType() == typeof(GIB.Auspex.InfoBoxAttribute))
            {
                var auspexInfobox = attr as GIB.Auspex.InfoBoxAttribute;
                if (auspexInfobox == null) return;

                string message = auspexInfobox.Text;
                EInfoBoxType boxType = auspexInfobox.Type;

                InfoMessageType odinType = boxType switch
                {
                    EInfoBoxType.Normal => InfoMessageType.Info,
                    EInfoBoxType.Warning => InfoMessageType.Error,
                    EInfoBoxType.Error => InfoMessageType.Error,
                    _ => InfoMessageType.Info
                };

                attributes.Add(new Sirenix.OdinInspector.InfoBoxAttribute(message, odinType));
            }

            if (attr.GetType() == typeof(GIB.Auspex.ButtonAttribute))
            {
                var auspexButton = attr as GIB.Auspex.ButtonAttribute;
                if (auspexButton == null) return;

                string message = auspexButton.Text;
                GIB.Auspex.ButtonSizes buttonSize = auspexButton.ButtonSize;

                Color odinColor = auspexButton.ButtonColor;

                Sirenix.OdinInspector.ButtonSizes odinSize = buttonSize switch
                {
                    GIB.Auspex.ButtonSizes.Standard => Sirenix.OdinInspector.ButtonSizes.Medium,
                    GIB.Auspex.ButtonSizes.Large => Sirenix.OdinInspector.ButtonSizes.Large,
                    GIB.Auspex.ButtonSizes.Huge => Sirenix.OdinInspector.ButtonSizes.Gigantic,
                    _ => Sirenix.OdinInspector.ButtonSizes.Small
                };

                attributes.Add(new Sirenix.OdinInspector.ButtonAttribute(message, odinSize));
                attributes.Add(new Sirenix.OdinInspector.GUIColorAttribute(odinColor.ToString()));
            }

            if (attr.GetType() == typeof(GIB.Auspex.ShowIfAttribute))
            {
                var auspexAttribute = attr as GIB.Auspex.ShowIfAttribute;
                if (auspexAttribute == null) return;

                string condition = auspexAttribute.Conditions[0];
                bool isInverted = auspexAttribute.Inverted;

                object odinValue = auspexAttribute.EnumValue;
                if (auspexAttribute.EnumValue != null)
                {

                    if (isInverted)
                    {
                        attributes.Add(new Sirenix.OdinInspector.HideIfAttribute(condition, odinValue));
                    }
                    else
                    {
                        attributes.Add(new Sirenix.OdinInspector.ShowIfAttribute(condition, odinValue));
                    }
                }
                else
                {
                    if (isInverted)
                    {
                        attributes.Add(new Sirenix.OdinInspector.HideIfAttribute(condition));
                    }
                    else
                    {
                        attributes.Add(new Sirenix.OdinInspector.ShowIfAttribute(condition));
                    }
                }

            }

            if (attr.GetType() == typeof(GIB.Auspex.MinMaxSliderAttribute))
            {
                var auspexSlider = attr as GIB.Auspex.MinMaxSliderAttribute;
                if (auspexSlider == null) return;

                float minValue = auspexSlider.MinValue;
                float maxValue = auspexSlider.MaxValue;

                attributes.Add(new Sirenix.OdinInspector.MinMaxSliderAttribute(minValue,maxValue));
            }
        }
    }
}
#endif