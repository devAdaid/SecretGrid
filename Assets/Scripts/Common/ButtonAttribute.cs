using UnityEngine;
using System;
#if UNITY_EDITOR
using JetBrains.Annotations;
#endif

#if UNITY_EDITOR
[MeansImplicitUse]
#endif
[AttributeUsage(AttributeTargets.Method)]
public class ButtonAttribute : PropertyAttribute
{
    public string ButtonLabel;

    public ButtonAttribute(string buttonLabel = null)
    {
        this.ButtonLabel = buttonLabel;
    }
}
