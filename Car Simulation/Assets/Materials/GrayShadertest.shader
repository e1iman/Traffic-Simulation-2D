Shader "Custom/GrayShadertest" 
{ 
	Properties 
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_EffectAmount ("Effect Amount", Range (0, 1)) = 1.0 
	} 
		
	SubShader 
	{ 
		Tags { "RenderType"="Opaque" } LOD 200

		CGPROGRAM
		#pragma surface surf Lambert
 
		sampler2D _MainTex;
		uniform float _EffectAmount;
 
		 struct Input {
			float2 uv_MainTex;
		};
 
		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = lerp(c.rgb, (c.r + c.g + c.b)/3, _EffectAmount);
			o.Alpha = c.a;
		}
		ENDCG
	} 
 FallBack "Diffuse"
}