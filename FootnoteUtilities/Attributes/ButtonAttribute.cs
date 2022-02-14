using System;

/// <summary>
///     This attribute tells the Editor to draw a Method on the Inspector Window.
///     Ref: https://github.com/dbrizov/NaughtyAttributes
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class ButtonAttribute : Attribute
{
    public ButtonAttribute(string label) => Label = label;

    public ButtonAttribute() { }

    public string Label { get; }
}
