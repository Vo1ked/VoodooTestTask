Shader "Custom/StandardWithStencil"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0.0,1.0)) = 0.5
        _Metallic ("Metallic", Range(0.0,1.0)) = 0.0
        _StencilRef ("Stencil Reference", Float) = 1
        _StencilComp ("Stencil Comparison", Float) = 3 // 8 corresponds to 'Equal'
        _StencilPass ("Stencil Pass", Float) = 0 // 0 corresponds to 'Keep'
        _StencilFail ("Stencil Fail", Float) = 0 // 0 corresponds to 'Keep'
        _StencilZFail ("Stencil ZFail", Float) = 0 // 0 corresponds to 'Keep'
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Stencil
        {
            Ref [_StencilRef]
            Comp [_StencilComp]
            Pass [_StencilPass]
            Fail [_StencilFail]
            ZFail [_StencilZFail]
        }

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        sampler2D _MainTex;
        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
