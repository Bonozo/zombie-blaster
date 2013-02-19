Shader "Lightning" {

    Properties {

        _MainTex("MainTex (RGB)", 2D) = "white" {}

        _NoiseTex("Noise (RGBA)", 2D) = "white" {}

        _Noise("Noise", FLOAT) = 0.2

        _Speed("Speed", FLOAT) = 0.2

        _FallOff("FallOff", FLOAT) = 2

        _Lines("Lines", FLOAT) = 0.2

        _Color("Tint Color", COLOR) = (0.5,0.5,0.5,0.5)

        _Width("Width", FLOAT) = 0.2

    }

    SubShader {

        CGPROGRAM

        #pragma surface surf Lambert

 

        sampler2D _MainTex;

 

        struct Input {

            float2 uv_MainTex;

        };

 

        void surf (Input IN, inout SurfaceOutput o) {

            half4 c = tex2D (_MainTex, IN.uv_MainTex);

            o.Albedo = c.rgb;

            o.Alpha = c.a;

        }

        ENDCG

    

        Pass{

            Blend SrcAlpha OneMinusSrcAlpha

            ZWrite Off

            Cull Off

            

            CGPROGRAM

            #pragma vertex vert

            #pragma fragment frag

            #include "UnityCG.cginc"

            

            float4 _NoiseTex_ST;

            sampler2D _NoiseTex;

            float _Speed;

            half4 _Color;

            float _FallOff;

            float _Lines;

            float _Noise;

            float _Width;

            

            struct data{

                float4 vertex : POSITION;

                float2 texcoord : TEXCOORD;

                float3 normal : NORMAL;

            };

            

            struct v2f{

                float4 position : POSITION;

                float2 uv : TEXCOORD;

                float viewAngle : TEXCOORD1;

                float ypos : TEXCOORD2;

            };

            

            v2f vert(data v){

                v2f o;

                o.position = mul(UNITY_MATRIX_MVP, v.vertex + float4(v.normal, 0) * _Width);

                o.uv = TRANSFORM_TEX(v.texcoord, _NoiseTex);

                o.viewAngle = 1- abs(dot(v.normal, normalize(ObjSpaceViewDir(v.vertex))));

                o.ypos = o.position.y;

                return o;

            }

            

            half4 frag(v2f i) : COLOR{

                float2 uvOffset1 = _Time.xy*_Noise;

                float2 uvOffset2 = -_Time.xx*_Noise;

                half4 noise1 = tex2D(_NoiseTex, i.uv + uvOffset1);

                half4 noise2 = tex2D(_NoiseTex, i.uv + uvOffset2);

                float noise = (dot(noise1, noise2) - 1) * _Noise;

                half4 col = sin((i.ypos*_Lines + _Time.x*_Speed + noise)*100);

                noise1 = tex2D(_NoiseTex, i.uv*6 + uvOffset1);

                noise2 = tex2D(_NoiseTex, i.uv*6 + uvOffset2);

                col.a *= saturate(1.3-(noise1.g+noise2.g)) * pow(i.viewAngle,_FallOff) * 15;

                return col * _Color * 2;

            }

            

            ENDCG

        }

    }

}