//https://answers.unity.com/questions/418008/render-transparent-material-without-double-occlusi.html
Shader "Custom/TestShader"
{
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        // _Glossiness ("Smoothness", Range(0,1)) = 0.5
        // _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    
    SubShader {
	// Tags { "RenderType"="Opaque"}
	// LOD 200

	// CGPROGRAM

	// #pragma surface surf Standard noshadow alpha:fade

	// #pragma target 3.0

	// fixed4 _Color;

	

	// struct Input {
	//     float2 uv_MainTex;
	// };

	// UNITY_INSTANCING_BUFFER_START(Props)
        // // put more per-instance properties here
        // UNITY_INSTANCING_BUFFER_END(Props)

	// sampler2D _MainTex;

	// void surf(Input IN, inout SurfaceOutputStandard o){
	//     o.Albedo = _Color.rgb;
	//     o.Alpha = _Color.a;
	// }
	// ENDCG

	Pass {ColorMask 0}


	Tags {"Queue"="Overlay" "RenderType"="Transparent"}
	LOD 200

	Blend SrcAlpha OneMinusSrcAlpha

	CGPROGRAM

	#pragma surface surf Standard noshadow alpha:premul

	#pragma target 3.0

	fixed4 _Color;

	

	struct Input {
	    float2 uv_MainTex;
	};

	UNITY_INSTANCING_BUFFER_START(Props)
        // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

	sampler2D _MainTex;

	void surf(Input IN, inout SurfaceOutputStandard o){
	    o.Albedo = _Color.rgb;
	    o.Alpha = _Color.a;
	}
	ENDCG
    }
    FallBack "Diffuse"
}
