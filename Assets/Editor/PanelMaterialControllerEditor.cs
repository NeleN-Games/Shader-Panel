using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PanelMaterialController))]
public class PanelMaterialControllerEditor : Editor
{
    private SerializedProperty _gradientMode;
    private SerializedProperty _color1;
    private SerializedProperty _color2;
    private SerializedProperty _constantColor;
    private SerializedProperty _gradientSize;
    private SerializedProperty _opacity;
    private SerializedProperty _gradientStart;
    private SerializedProperty _gradientEnd;
    private SerializedProperty _cornerRadius;
    private SerializedProperty _mainMaterial;

    private void OnEnable()
    {
        // Link serialized properties
        _gradientMode = serializedObject.FindProperty("gradientMode");
        _color1 = serializedObject.FindProperty("color1");
        _color2 = serializedObject.FindProperty("color2");
        _constantColor = serializedObject.FindProperty("constantColor");
        _gradientSize = serializedObject.FindProperty("gradientSize");
        _opacity = serializedObject.FindProperty("opacity");
        _gradientStart = serializedObject.FindProperty("gradientStart");
        _gradientEnd = serializedObject.FindProperty("gradientEnd");
        _cornerRadius = serializedObject.FindProperty("cornerRadius");
        _mainMaterial = serializedObject.FindProperty("mainMaterial");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        EditorGUILayout.PropertyField(_gradientMode);
        
        PanelMaterialController.GradientMode mode = (PanelMaterialController.GradientMode)_gradientMode.enumValueIndex;

        switch (mode)
        {
            case PanelMaterialController.GradientMode.Linear:
                EditorGUILayout.PropertyField(_color1);
                EditorGUILayout.PropertyField(_color2);
                EditorGUILayout.PropertyField(_gradientStart);
                EditorGUILayout.PropertyField(_gradientEnd);
                EditorGUILayout.PropertyField(_gradientSize);
                break;

            case PanelMaterialController.GradientMode.Radial:
                EditorGUILayout.PropertyField(_color1);
                EditorGUILayout.PropertyField(_color2);
                EditorGUILayout.PropertyField(_gradientSize);
                break;

            case PanelMaterialController.GradientMode.Constant:
                EditorGUILayout.PropertyField(_constantColor);
                break;
        }
        
        EditorGUILayout.Slider(_opacity, 0f, 1f, new GUIContent("Opacity"));
        EditorGUILayout.Slider(_cornerRadius, 0f, .3f, new GUIContent("Corner Radius"));
        EditorGUILayout.PropertyField(_mainMaterial);
        serializedObject.ApplyModifiedProperties();
    }
}
