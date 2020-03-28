// Transparency - https://answers.unity.com/questions/272749/how-to-write-unlit-surface-shader.html
// Fresnel Effect - https://www.ronja-tutorials.com/2018/05/26/fresnel.html
// Screenspace Texture - https://docs.unity3d.com/Manual/SL-SurfaceShaderExamples.html
// Overlapping Fix - https://answers.unity.com/questions/418008/render-transparent-material-without-double-occlusi.html
Shader "Custom/Hologram"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
	_Scan ("Scanline Texture", 2D) = "gray" {}
	_ScanSize ("Scanline Size", Range(1,1000)) = 20
	_ScanStrength("Scanline Strength", Range(0,1)) = 0.2
        //_Glossiness ("Smoothness", Range(0,1)) = 0.5
        //_Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {

	Pass {
	    ColorMask 0
	}
	
        Tags {
	    "Queue"="Transparent"
	    "RenderType"="Transparent"
	}

	// Pass {
	//     ZWrite On
	//     ColorMask 0
	// }

	//UsePass "Transparent/Diffuse/FORWARD"
	
	//Pass {
	Blend SrcAlpha OneMinusSrcAlpha
	ZWrite On
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf StandardSpecular noshadow alpha

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
	sampler2D _Scan;
	float _ScanSize;
	float _ScanStrength;

        struct Input
        {
	    float2 uv_MainTex;
	    float3 worldNormal;
	    float3 viewDir;
	    float4 screenPos;
	    INTERNAL_DATA
        };

        // half _Glossiness;
        // half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
        // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

	// fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten){
	//     return fixed4(0,0,0,0);
	// }

        void surf (Input IN, inout SurfaceOutputStandardSpecular o)
        {
	    // Albedo comes from a texture tinted by color
	    fixed4 c = tex2D (_MainTex, IN.uv_MainTex);// * _Color;
	    c = dot(c.rgb,float3(0.3,0.59,0.11)) * _Color;
	    float fresnel = dot(IN.worldNormal,IN.viewDir);

	    fixed2 screenUV = fixed2(IN.screenPos.x,IN.screenPos.y) / IN.screenPos.w;
	    //fixed2 screenUV = fixed2(IN.screenPos.x,IN.screenPos.y+_Time.x) / IN.screenPos.w;
	    screenUV *= float2(_ScanSize,_ScanSize);
	    screenUV.y += _Time.z;
	    //screenUV.y += _Time;
	    fixed4 lines = tex2D(_Scan, screenUV);
	    
	    //o.Albedo = c.rgb;
	    o.Emission = saturate(1-fresnel)* c.rgb + lines.rgb*_ScanStrength; //c.rgb;
	    // Metallic and smoothness come from slider variables
	    //o.Metallic = _Metallic;
	    //o.Smoothness = _Glossiness;
	    o.Specular = 0;
	    o.Alpha = c.a;
        }
        ENDCG
	//}
	// Pass {
	//     ZWrite Off
	//     Blend SrcAlpha OneMinusSrcAlpha
	// }
	// CGPROGRAM
	// #pragma surface surf StandardSpecular noshadow //alpha

        // // Use shader model 3.0 target, to get nicer looking lighting
        // #pragma target 3.0

        // sampler2D _MainTex;
	// sampler2D _Scan;
	// float _ScanSize;
	// float _ScanStrength;

        // struct Input
        // {
	//     float2 uv_MainTex;
	//     float3 worldNormal;
	//     float3 viewDir;
	//     float4 screenPos;
	//     INTERNAL_DATA
        // };

        // // half _Glossiness;
        // // half _Metallic;
        // fixed4 _Color;

        // // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // // #pragma instancing_options assumeuniformscaling
        // UNITY_INSTANCING_BUFFER_START(Props)
        // // put more per-instance properties here
        // UNITY_INSTANCING_BUFFER_END(Props)

	// // fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten){
	// //     return fixed4(0,0,0,0);
	// // }

	// void surf(Input IN, inout SurfaceOutputStandardSpecular o){
	//     o.Alpha = _Color.a;
	    
	// }
	// ENDCG
	
    }
    FallBack "Diffuse"
}
