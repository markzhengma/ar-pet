Shader "Custom/Transparent Shadow Receiver" {
Properties {
}
 
SubShader {
    Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
    LOD 200
    ZWrite Off
    Blend Zero SrcColor
 
CGPROGRAM
#pragma surface surf ShadowOnly alphatest:_Cutoff
fixed4 _Color;
 
struct Input {
    float2 uv_MainTex;
};
 
inline fixed4 LightingShadowOnly (SurfaceOutput s, fixed3 lightDir, fixed atten)
{
    fixed4 c;
    c.rgb = s.Albedo * atten;
    c.a = s.Alpha;
    return c;
}
 
void surf (Input IN, inout SurfaceOutput o) {
	o.Albedo = 1;
    o.Alpha = 1;
}
 
ENDCG
}
 
Fallback "Transparent/Cutout/VertexLit"
}