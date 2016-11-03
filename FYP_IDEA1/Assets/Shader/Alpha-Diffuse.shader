Shader "Custom/Transparent/Diffuse" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_Cutoff ("Alpha Cutoff", Range (0,1)) = 0.5
}

SubShader {
	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
	LOD 200

	Pass {
		Cull Off
		Lighting Off
		ZWrite off
		AlphaTest Greater [_Cutoff]
		SetTexture [_MainTex] {
			//combine texture * primary, texture
			//combine texture
			combine texture * texture
		}
	}

//	// First pass
//	// Render any pixels that are more than [_Cutoff] opaque
//	Pass {
//		Cull Front
//		AlphaTest Greater [_Cutoff]
//		SetTexture [_MainTex] {
//			combine texture * primary, texture
//		}
//	}
//
//	// Second pass
//	// Render the semitransparent details
//	Pass {
//		Cull Back
//		// Don't write to the depth buffer
//		ZWrite off
//		// Don't write pixels that have already written
//		ZTest Less
//		// Only render pixels less or equal to the value
//		AlphaTest LEqual [_Cutoff]
//		// Set up alpha blending
//		Blend SrcAlpha OneMinusSrcAlpha
//		SetTexture [_MainTex] {
//			combine texture * primary, texture
//		}
//	}

CGPROGRAM
#pragma surface surf Lambert alpha

sampler2D _MainTex;
fixed4 _Color;

struct Input {
	float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	o.Albedo = c.rgb;
	o.Alpha = c.a;
}
ENDCG
}

Fallback "Transparent/VertexLit"
}
