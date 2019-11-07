Shader "Custom/TextShader"
{
    Properties
    {
        _MainTex ("Font Texture (RGB)", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
	_BGColor ("Background Color", Color) = (0,0,0,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
	LOD 200
	//Lighting Off Cull Off ZWrite Off Fog { Mode Off }
	//Blend SrcAlpha OneMinusSrcAlpha

	Pass {
	     Color [_Color]
	     SetTexture [_MainTex] {
	     		ConstantColor [_BGColor]
			combine constant
	     		
	     }
	     
	     SetTexture [_MainTex] {
	     		ConstantColor [_Color]
	     		//combine primary, texture * constant
			combine constant lerp(texture) previous
	     }
	}
    }
    FallBack "Diffuse"
}
