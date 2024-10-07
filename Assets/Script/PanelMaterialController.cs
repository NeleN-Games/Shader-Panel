using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class PanelMaterialController : MonoBehaviour
{
    // Serialized fields for different properties
    [Header("Gradient Settings")]
    public GradientMode gradientMode = GradientMode.Linear; // Mode selector
    public Color color1 = Color.white;    // For gradient mode
    public Color color2 = Color.black;    // For gradient mode
    public Color constantColor = Color.green; // For constant mode
    public float gradientSize = 1.0f;     // Gradient size
    [Range(0f,1f)] public float opacity = 1.0f;          // Opacity of the panel
    public Vector2 gradientStart = Vector2.zero;   // Gradient start for linear
    public Vector2 gradientEnd = Vector2.one;      // Gradient end for linear
    [Range(0f,3f)]public float cornerRadius = 0.1f;     // Corner radius for curved corners

    public Material mainMaterial;
    private Material _materialInstance;
    private Image _image;

    // Property IDs for shader properties
    private int _color1ID;
    private int _color2ID;
    private int _gradientModeID;
    private int _constantColorID;
    private int _gradientSizeID;
    private int _opacityID;
    private int _gradientStartID;
    private int _gradientEndID;
    private int _cornerRadiusID;
  
    public enum GradientMode
    {
        Linear,
        Radial,
        Constant
    }
    // This method will be called to update material properties based on current mode
    private void UpdateMaterialProperties()
    {
        if (_materialInstance == null) return;

        // Common settings
        _materialInstance.SetFloat(_opacityID, opacity);
        _materialInstance.SetFloat(_cornerRadiusID, cornerRadius);

        // Apply the specific properties based on the selected gradient mode
        switch (gradientMode)
        {
            case GradientMode.Linear:
                _materialInstance.SetFloat(_gradientModeID, 0); // Linear mode
                _materialInstance.SetColor(_color1ID, color1);
                _materialInstance.SetColor(_color2ID, color2);
                _materialInstance.SetVector(_gradientStartID, gradientStart);
                _materialInstance.SetVector(_gradientEndID, gradientEnd);
                _materialInstance.SetFloat(_gradientSizeID, gradientSize);
                break;

            case GradientMode.Radial:
                _materialInstance.SetFloat(_gradientModeID, 1); // Radial mode
                _materialInstance.SetColor(_color1ID, color1);
                _materialInstance.SetColor(_color2ID, color2);
                _materialInstance.SetFloat(_gradientSizeID, gradientSize);
                break;

            case GradientMode.Constant:
                _materialInstance.SetFloat(_gradientModeID, 2); // Constant mode
                _materialInstance.SetColor(_constantColorID, constantColor);
                break;
        }

        // Force the CanvasRenderer to refresh the material
        _image.canvasRenderer.SetMaterial(_materialInstance, null);
    }

    public void ChangeMaterialProperties()
    {
        // Sets resources
        if (mainMaterial==null) mainMaterial=Resources.Load<Material>("Arts/Simple_Panel");
     
        // Initialize the shader property IDs
        _color1ID = Shader.PropertyToID("_Color1");
        _color2ID = Shader.PropertyToID("_Color2");
        _gradientModeID = Shader.PropertyToID("_GradientMode");
        _constantColorID = Shader.PropertyToID("_ConstantColor");
        _gradientSizeID = Shader.PropertyToID("_GradientSize");
        _opacityID = Shader.PropertyToID("_Opacity");
        _gradientStartID = Shader.PropertyToID("_GradientStart");
        _gradientEndID = Shader.PropertyToID("_GradientEnd");
        _cornerRadiusID = Shader.PropertyToID("_CornerRadius");

        // Cache the Image component
        if (_image==null)
        {
            _image = GetComponent<Image>();
           // imagePreset.ApplyTo(this);
        }
        // Create an instance of the material to ensure changes only affect this specific Image
        if (_materialInstance == null)
        {
            _materialInstance = Instantiate(mainMaterial);
            _image.material = _materialInstance;
        }
        UpdateMaterialProperties();
      
    }
    private void OnValidate()
    {
        ChangeMaterialProperties();
    }
}
