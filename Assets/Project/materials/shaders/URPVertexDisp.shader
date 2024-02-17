    Shader "Custom/URPVertexDisp"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BaseColor("Base Color", color) = (1,1,1,1)
        _Smoothness("Smoothness", Range(0,1)) = 0
        _Metallic("Metallic", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalRenderPipeline" }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"    
            #include "Assets/materials/shaders/Noise/SimplexNoise3D.hlsl"

            float3 applyDisp(float3 vertex){
                float3 pos = vertex.xyz/10;
                pos.y += _Time*10;
                vertex.y += 2*SimplexNoise3D(pos) -2.0;
                return vertex;
            }


            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 normal : NORMAL;
                float4 tangent : TANGENT;
                float4 texcoord1 : TEXCOORD1;
                UNITY_VERTEX_INPUT_INSTANCE_ID // SINGLE PASS INSTANCING DECLARATION
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                float3 normalWS : TEXCOORD2;
                float3 viewDir : TEXCOORD3;
                float3 position : TEXCOORD4;
                DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 5); // texcoord5
                float3 tangent : TEXCOORD6;
                float3 bitangent : TEXCOORD7;
                UNITY_VERTEX_OUTPUT_STEREO // SINGLE PASS INSTANCING DECLARATION
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _BaseColor;
            float _Smoothness, _Metallic;

            v2f vert (appdata v)
            {   
                // // getting tangent and bitangent
                // float3 T = cross(float3(0, 1, 0), v.normal.xyz);
                // float3 B = cross(v.normal.xyz, T);
                
                // apply vertex displacement
                float3 originalPos = v.vertex.xyz;
                v.vertex.xyz = applyDisp(v.vertex.xyz);
                //
                // reacalculating normals
                
                float3 normal = v.normal.xyz;
                float3 tangent = v.tangent.xyz;
                float3 bitangent = cross( normal, tangent.xyz );
                float3 nt = ( applyDisp( originalPos + tangent.xyz * 0.01 ) - v.vertex.xyz );
                float3 nb = ( applyDisp( originalPos + bitangent * 0.01 ) - v.vertex.xyz );
                normal = cross( nt, nb );


                v2f o;
                UNITY_SETUP_INSTANCE_ID(v); // SINGLE PASS INSTANCING DECLARATION
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); // SINGLE PASS INSTANCING DECLARATION
                o.positionWS = TransformObjectToWorld(v.vertex.xyz);
                o.normalWS = TransformObjectToWorldNormal(normal);
                o.viewDir = normalize(_WorldSpaceCameraPos - o.positionWS);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.vertex = TransformWorldToHClip(o.positionWS);
                o.position = v.vertex.xyz; 

                // o.tangent = T;
                // o.bitangent = B;

                OUTPUT_LIGHTMAP_UV( v.texcoord1, unity_LightmapST, o.lightmapUV );
                OUTPUT_SH(o.normalWS.xyz, o.vertexSH );

                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                // IF INSTEAD OF SMOOTH SHADING WANT FLAT SHADING, USE THIS TO CALCULATE NORMALS
                float3 ddxPos = ddx(i.position);
                float3 ddyPos = ddy(i.position)  * _ProjectionParams.x;
                float3 flatNormalsWS = TransformObjectToWorldNormal(cross(ddxPos, ddyPos));

                half4 col = tex2D(_MainTex, i.uv);
                InputData inputdata = (InputData)0; // initialize inputdata struct defined in input.hlsl
                inputdata.positionWS = i.positionWS; // world space position
                inputdata.normalWS = normalize(flatNormalsWS); // world space normal
                inputdata.viewDirectionWS = i.viewDir; // world space view direction
                inputdata.bakedGI = SAMPLE_GI( i.lightmapUV, i.vertexSH, inputdata.normalWS ); //  global illumination

                SurfaceData surfacedata; // initialize surfacedata struct defined in SurfaceData.hlsl
                surfacedata.albedo = i.normalWS-0.8;//_BaseColor; // albedo color
                surfacedata.specular = 1;
                surfacedata.metallic = _Metallic;
                surfacedata.smoothness = _Smoothness;
                surfacedata.normalTS = 1;
                surfacedata.emission = 0;
                surfacedata.occlusion = 1;
                surfacedata.alpha = 0;
                surfacedata.clearCoatMask = 0;
                surfacedata.clearCoatSmoothness = 0;
                

                return UniversalFragmentPBR(inputdata, surfacedata);
            }
            ENDHLSL
        }
    }
}

