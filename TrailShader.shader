Shader "TrailShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Factor ("Texture Factor", Range(0,1)) = 1
		_MainTex ("Texture", 2D) = "white" {}
	}

	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}

		Stencil {
		   Ref 1
		   ReadMask 1
		   Comp NotEqual
		   Pass Replace
		}
	
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha // Classic Transparency
	
		Pass {  
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
			
				#include "UnityCG.cginc"

				struct appdata_t {
					float4 vertex : POSITION;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f {
					float4 vertex : SV_POSITION;
					half2 texcoord : TEXCOORD0;
				};

				float4 _Color;
				float _Factor;
				sampler2D _MainTex;
				float4 _MainTex_ST;
			
				v2f vert (appdata_t v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
					return o;
				}
			
				fixed4 frag (v2f i) : SV_Target
				{
					float4 rgb = tex2D(_MainTex, i.texcoord);
					float4 grey = saturate(((rgb.x + rgb.y + rgb.z) / 3) + _Factor);
					return grey * _Color;
				}
			ENDCG
		}
	}
}