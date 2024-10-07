Shader "Custom/Panel"
{
    Properties
        {
            _Color1 ("Color 1", Color) = (1,1,1,1)
            _Color2 ("Color 2", Color) = (0,0,0,1)
            _GradientSize ("Gradient Size", Range(0.1, 10)) = 1
            _Opacity ("Opacity", Range(0, 1)) = 1
            _GradientStart ("Gradient Start Point", Vector) = (0, 0, 0, 0)
            _GradientEnd ("Gradient End Point", Vector) = (1, 1, 0, 0)
            [HideInInspector] _GradientMode ("Gradient Mode", Float) = 0 // Now we will use 3 values: 0 = Linear, 1 = Radial, 2 = Constant
            _CornerRadius ("Corner Radius", Range(0, .2)) = 0.1
            _ConstantColor ("Constant Color", Color) = (0, 1, 0, 1) // New property for constant color
        }
        SubShader
        {
            Tags { "RenderType"="Transparent"   "Queue"="Transparent" "IgnoreProjector"="True" "CanUseSpriteAtlas"="True" }
            LOD 100

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull False
            
            Pass
            {
                CGPROGRAM
                #pragma multi_compile_instancing
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata_t
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                    float4 color : COLOR;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                    float4 color : COLOR; 
                };
                CBUFFER_START(UnityPerMaterial)
                fixed4 _Color1;
                fixed4 _Color2;
                float _GradientSize;
                float _Opacity;
                float2 _GradientStart;
                float2 _GradientEnd;
                float _GradientMode;
                float _CornerRadius; 
                fixed4 _ConstantColor; // New constant color
                CBUFFER_END
                v2f vert (appdata_t v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    o.color = v.color;
                    return o;
                }

                fixed4 frag (v2f i) : SV_Target
                {
                    float2 uv = i.uv;
                    fixed4 color = fixed4(0, 0, 0, 0);

                    // Corner radius handling
                    float2 cornerMin = float2(_CornerRadius, _CornerRadius);
                    float2 cornerMax = float2(1.0 - _CornerRadius, 1.0 - _CornerRadius);

                    if ((uv.x < cornerMin.x && uv.y < cornerMin.y) ||
                        (uv.x > cornerMax.x && uv.y < cornerMin.y) ||
                        (uv.x < cornerMin.x && uv.y > cornerMax.y) ||
                        (uv.x > cornerMax.x && uv.y > cornerMax.y))
                    {
                        float2 cornerCenter = step(float2(0.5, 0.5), uv) * cornerMax + (1 - step(float2(0.5, 0.5), uv)) * cornerMin;
                        float distToCorner = length(uv - cornerCenter);

                        if (distToCorner > _CornerRadius)
                        {
                            discard; // Discard pixels outside the radius
                        }
                    }

                    if (_GradientMode == 1) // Radial gradient mode
                    {
                        float dist = length(uv - float2(0.5, 0.5)) * _GradientSize;
                        dist = saturate(dist);
                        color = lerp(_Color1, _Color2, dist);
                        color.a = lerp(_Color1.a, _Color2.a, dist) * _Opacity;
                    }
                    else if (_GradientMode == 0) // Linear gradient mode
                    {
                        float2 gradientDirection = _GradientEnd - _GradientStart;
                        float gradientLength = length(gradientDirection);
                        gradientDirection /= gradientLength;

                        float2 uvToStart = uv - _GradientStart;
                        float projection = dot(uvToStart, gradientDirection) / (gradientLength / _GradientSize);

                        projection = saturate(projection);
                        color = lerp(_Color1, _Color2, projection);
                        color.a = lerp(_Color1.a, _Color2.a, projection) * _Opacity;
                    }
                    else if (_GradientMode == 2) // Constant color mode
                    {
                        color = _ConstantColor; // Use the constant color
                        color.a *= _Opacity; // Apply opacity
                    }
                    color.a *= i.color.a;
                    return color;
                }
                ENDCG
            }
        }
    CustomEditor "GradientShaderEditor"
}