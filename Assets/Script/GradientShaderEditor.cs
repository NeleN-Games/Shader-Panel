using UnityEditor;
using UnityEngine;

public class GradientShaderEditor : ShaderGUI
{
    enum GradientMode
    {
        Linear,
        Radial,
        Constant // Add Constant mode
    }

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        // Fetch properties
        MaterialProperty gradientModeProp = FindProperty("_GradientMode", properties);
        MaterialProperty color1 = FindProperty("_Color1", properties);
        MaterialProperty color2 = FindProperty("_Color2", properties);
        MaterialProperty gradientSize = FindProperty("_GradientSize", properties);
        MaterialProperty opacity = FindProperty("_Opacity", properties);
        MaterialProperty gradientStart = FindProperty("_GradientStart", properties);
        MaterialProperty gradientEnd = FindProperty("_GradientEnd", properties);
        MaterialProperty cornerRadius = FindProperty("_CornerRadius", properties); 
        MaterialProperty constantColor = FindProperty("_ConstantColor", properties); // Add constant color property

        // Cast gradient mode to an enum
        GradientMode gradientMode = (GradientMode)gradientModeProp.floatValue;

        // Display GUI for gradient mode selection
        gradientMode = (GradientMode)EditorGUILayout.EnumPopup("Gradient Mode", gradientMode);
        gradientModeProp.floatValue = (float)gradientMode;

        // Apply the correct shader keyword based on the selected mode
        foreach (Material mat in materialEditor.targets)
        {
            if (gradientMode == GradientMode.Radial)
            {
                mat.SetFloat("_GradientMode", 1); // Radial
            }
            else if (gradientMode == GradientMode.Constant)
            {
                mat.SetFloat("_GradientMode", 2); // Constant
            }
            else
            {
                mat.SetFloat("_GradientMode", 0); // Linear
            }
        }

        // Display other properties conditionally
        if (gradientMode == GradientMode.Linear || gradientMode == GradientMode.Radial)
        {
            materialEditor.ShaderProperty(color1, color1.displayName);
            materialEditor.ShaderProperty(color2, color2.displayName);
            if (gradientMode == GradientMode.Radial || gradientMode == GradientMode.Linear)
            {
                materialEditor.ShaderProperty(gradientSize, gradientSize.displayName);
            }
        }

        if (gradientMode == GradientMode.Linear)
        {
            materialEditor.ShaderProperty(gradientStart, gradientStart.displayName);
            materialEditor.ShaderProperty(gradientEnd, gradientEnd.displayName);
        }

        if (gradientMode == GradientMode.Constant)
        {
            materialEditor.ShaderProperty(constantColor, constantColor.displayName); // Show only for Constant mode
        }
        // Display the corner radius property
        materialEditor.ShaderProperty(cornerRadius, cornerRadius.displayName);
        materialEditor.ShaderProperty(opacity, opacity.displayName);
        materialEditor.EnableInstancingField();
    }
}
