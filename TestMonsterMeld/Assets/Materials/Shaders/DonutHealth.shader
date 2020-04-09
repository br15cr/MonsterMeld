Shader "Custom/DonutHealth"
{
    Properties
    {
	_Color ("Base Color",Color) = (1,1,1,1)
	_Color1 ("Hurt Color", Color) = (1,1,1,1)
	_Color2 ("Health Color", Color) = (1,1,1,1)
	_Offset ("Offset",Range(0,1)) = 0
	_Length ("Length",Range(0,1)) = 1
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
	_HurtTex ("Hurt Texture", 2D) = "white" {}
	_HealTex ("Health Texture", 2D) = "white" {}
	_Mask ("Mask",2D) = "white" {}
        // _Glossiness ("Smoothness", Range(0,1)) = 0.5
        // _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

	Blend SrcAlpha OneMinusSrcAlpha
	ZWrite On

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf StandardSpecular noshadow fullforwardshadows alpha

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
	sampler2D _Mask;
	sampler2D _HurtTex;
	sampler2D _HealTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        // half _Glossiness;
        // half _Metallic;
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

        void surf (Input IN, inout SurfaceOutputStandardSpecular o)
        {
	    float pi = 3.14159265;
	    float x = clamp(IN.uv_MainTex.x + _Offset,0,1);
            // Albedo comes from a texture tinted by color
            fixed4 main = tex2D (_MainTex, IN.uv_MainTex) * _Color;
	    //fixed4 c = lerp(_Color1,_Color2,x);
	    fixed4 hurt = tex2D(_HurtTex,IN.uv_MainTex);
	    fixed4 heal = tex2D(_HealTex,IN.uv_MainTex);
	    
	    float circ = (atan2(IN.uv_MainTex.y*2-1,IN.uv_MainTex.x*2-1)/pi)/2+0.5-_Offset*_Length;
	    float circ2 = (atan2(IN.uv_MainTex.y*2-1,IN.uv_MainTex.x*2-1)/pi)/2+0.5-_Length;
	    circ2 = ceil(circ2);
	    circ = 1-ceil(circ);
	    fixed4 c = lerp(hurt*_Color1,heal*_Color2,circ); //fixed4 c = lerp(_Color1,_Color2,circ);
	    fixed4 al = tex2D(_Mask,IN.uv_MainTex);
	    c = lerp(c,main,circ2); //c = lerp(c,_Color,circ2);
	    //c = al;
	    //fixed4 c = circ;
	    
            //o.Albedo = c.rgb;
	    o.Emission = c.rgb * al.r;
            // Metallic and smoothness come from slider variables
            //o.Metallic = _Metallic;
            //o.Smoothness = _Glossiness;
            o.Alpha = al.r;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
