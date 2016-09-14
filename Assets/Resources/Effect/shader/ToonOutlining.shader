Shader "Toon/ToonOutliningShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_OutlineColor ("Outline Color", Color) = (.5,.5,.5,1)
		_OutlineWidth ("Outline width", Range(.1, 1.5)) = 1.0
		_MainColor("Main Color", Color) = (.5,.5,.5,1)
		_IntensityAdjust("Intensity Adjust", Range(0.0, 2.0)) = 1.0
		_OutlineAlpha("Outline Alpha", Range(0.0, 1.0)) = 0.0
		_OutlineIntensityAdjust("Outline Intensity Adjust", Range(0.0, 2.0)) = 1.0
		_BodyAdjustOffset("Adjust Offset", Range(.01, 0.2)) = .05
		_ToonCololAdjustTex("ToonShader Cubemap(RGB)", CUBE) = "" { }
		[Toggle] _IsOrthoCamera("Camera is orthogonal", int) = 1
	}

	SubShader
	{
		Tags { "RenderType"="Transparent" }
		LOD 100

		Pass
		{
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			uniform float _OutlineWidth;
			uniform float _BodyAdjustOffset;
			uniform float4 _OutlineColor;
			uniform bool _IsOrthoCamera;
			uniform float _OutlineIntensityAdjust;
			uniform float _OutlineAlpha;

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				UNITY_FOG_COORDS(0)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			v2f vert(appdata v)
			{
				v2f o;
				
				o.vertex = mul(UNITY_MATRIX_MV, v.vertex);

				float3 normal = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
				o.vertex.xyz = o.vertex.xyz + _OutlineWidth * normal.xyz;

				normal = _IsOrthoCamera == 0 ? normalize(o.vertex.xyz) : float3(0,0,-1);
				o.vertex.xyz = o.vertex.xyz + _BodyAdjustOffset * normal.xyz;

				o.vertex = mul(UNITY_MATRIX_P, o.vertex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = _OutlineColor * _OutlineIntensityAdjust;
				col.a = _OutlineAlpha;
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
				ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			samplerCUBE _ToonCololAdjustTex;
			float4    _MainTex_ST;
			float4	  _MainColor;
			float	  _IntensityAdjust;

			struct appdata {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f {
				float4 pos : SV_POSITION;
				float3 cubenormal : NORMAL;
				float2 texcoord : TEXCOORD0;
				UNITY_FOG_COORDS(1)
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.cubenormal = mul(UNITY_MATRIX_MV, float4(v.normal, 0));
				UNITY_TRANSFER_FOG(o,o.pos);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 color = tex2D(_MainTex, i.texcoord);
				fixed4 toonAdjustColor = texCUBE(_ToonCololAdjustTex, i.cubenormal);
				color = color * toonAdjustColor * _MainColor * _IntensityAdjust;
				UNITY_APPLY_FOG(i.fogCoord, color);
				return color;
			}
			ENDCG
		}
	}
}
