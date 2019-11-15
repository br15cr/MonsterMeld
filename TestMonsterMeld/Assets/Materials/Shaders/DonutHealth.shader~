Shader "Custom/DonutHealth"
{
    Properties
    {
	_Color1 ("Color 1", Color) = (1,1,1,1)
	_Color2 ("Color 2", Color) = (1,1,1,1)
	_Offset ("Offset",Range(0,1)) = 0
	_Length ("Length",Range(0,1)) = 1
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
	fixed4 _Color1;
	fixed4 _Color2;
	fixed _Offset;
	fixed _Length;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
		float pi = 3.14159265;
		float x = clamp(IN.uv_MainTex.x + _Offset,0,1);
            // Albedo comes from a texture tinted by color
            //fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
	    //fixed4 c = lerp(_Color1,_Color2,x);
	    float circ = 1-ceil((atan2(IN.uv_MainTex.y*2-1,IN.uv_MainTex.x*2-1)/pi)/2+0.5-_Offset*_Length);
	    fixed4 c = lerp(_Color1,_Color2,circ);
            //o.Albedo = c.rgb;
	    o.Emission = c.rgb;
            // Metallic and smoothness come from slider variables
            //o.Metallic = _Metallic;
            //o.Smoothness = _Glossiness;
            //o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
