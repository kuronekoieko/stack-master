Shader "Ciconia Studio/Effects/Cartoon Water/Foam (Mobile)" {
    Properties {
        [Space(15)][Header(Foam Properties)]
        [Space(10)][MaterialToggle] _ScreenMode ("Screen Mode", Float ) = 0
        [Space(10)]_Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Foam Texture (Black and white)", 2D) = "white" {}
        _TilingFoam ("Tiling", Float ) = 1
        [Space(10)]_Foamspeed ("Foam speed", Range(0, 0.5)) = 0

        [Space(15)][Header(Tide Properties)]
        [Space(10)][MaterialToggle] _OverlayMode ("Overlay Mode", Float ) = 0
        [Space(10)]_Amount ("Amount", Range(-1, 1)) = 1
        _Contrast ("Contrast", Range(0, 10)) = 0
        [Space(10)]_Intensity ("Intensity", Range(0, 1)) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform float4 _Color;
            uniform float _Foamspeed;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform fixed _ScreenMode;
            uniform float _TilingFoam;
            uniform float _Amount;
            uniform float _Contrast;
            uniform float _Intensity;
            uniform fixed _OverlayMode;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                LIGHTING_COORDS(7,8)
                UNITY_FOG_COORDS(9)
                #if defined(LIGHTMAP_ON) || defined(UNITY_SHOULD_SAMPLE_SH)
                    float4 ambientOrLightmapUV : TEXCOORD10;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                #ifdef LIGHTMAP_ON
                    o.ambientOrLightmapUV.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                    o.ambientOrLightmapUV.zw = 0;
                #elif UNITY_SHOULD_SAMPLE_SH
                #endif
                #ifdef DYNAMICLIGHTMAP_ON
                    o.ambientOrLightmapUV.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
                #endif
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                UNITY_LIGHT_ATTENUATION(attenuation,i, i.posWorld.xyz);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
/////// GI Data:
                UnityLight light;
                #ifdef LIGHTMAP_OFF
                    light.color = lightColor;
                    light.dir = lightDirection;
                    light.ndotl = LambertTerm (normalDirection, light.dir);
                #else
                    light.color = half3(0.f, 0.f, 0.f);
                    light.ndotl = 0.0f;
                    light.dir = half3(0.f, 0.f, 0.f);
                #endif
                UnityGIInput d;
                d.light = light;
                d.worldPos = i.posWorld.xyz;
                d.worldViewDir = viewDirection;
                d.atten = attenuation;
                #if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
                    d.ambient = 0;
                    d.lightmapUV = i.ambientOrLightmapUV;
                #else
                    d.ambient = i.ambientOrLightmapUV;
                #endif
                Unity_GlossyEnvironmentData ugls_en_data;
                ugls_en_data.roughness = 1.0 - 0;
                ugls_en_data.reflUVW = viewReflectDirection;
                UnityGI gi = UnityGlobalIllumination(d, 1, normalDirection, ugls_en_data );
                lightDirection = gi.light.dir;
                lightColor = gi.light.color;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += gi.indirect.diffuse;
                float node_3783 = ((i.uv0.g+_Amount)*_Contrast);
                float4 node_8955 = _Time;
                float node_7662_ang = node_8955.g;
                float node_7662_spd = _Foamspeed;
                float node_7662_cos = cos(node_7662_spd*node_7662_ang);
                float node_7662_sin = sin(node_7662_spd*node_7662_ang);
                float2 node_7662_piv = float2(0.5,0.5);
                float2 node_2694 = ((i.uv0*4.0)*_TilingFoam);
                float2 node_7662 = (mul(node_2694-node_7662_piv,float2x2( node_7662_cos, -node_7662_sin, node_7662_sin, node_7662_cos))+node_7662_piv);
                float4 _FoamTexture1 = tex2D(_MainTex,TRANSFORM_TEX(node_7662, _MainTex));
                float node_200_ang = node_8955.g;
                float node_200_spd = (-1*_Foamspeed);
                float node_200_cos = cos(node_200_spd*node_200_ang);
                float node_200_sin = sin(node_200_spd*node_200_ang);
                float2 node_200_piv = float2(0.5,0.5);
                float2 node_200 = (mul(node_2694-node_200_piv,float2x2( node_200_cos, -node_200_sin, node_200_sin, node_200_cos))+node_200_piv);
                float2 node_9929 = (node_200+float2(0.6,0.6));
                float4 _FoamTexture2 = tex2D(_MainTex,TRANSFORM_TEX(node_9929, _MainTex));
                float3 node_5599 = (saturate((_FoamTexture1.rgb*_FoamTexture2.rgb))*1.663);
                float3 node_3470 = saturate(lerp( node_5599, saturate((1.0-(1.0-node_5599)*(1.0-node_5599))), _ScreenMode ));
                float3 Diffuse = (_Color.rgb*lerp( saturate((1.0-((1.0-node_3470)/node_3783))), saturate(( node_3470 > 0.5 ? (1.0-(1.0-2.0*(node_3470-0.5))*(1.0-node_3783)) : (2.0*node_3470*node_3783) )), _OverlayMode ));
                float3 diffuseColor = Diffuse;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                float AlphaMask = node_3783;
                fixed4 finalRGBA = fixed4(finalColor,saturate((AlphaMask*_Intensity)));
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform float4 _Color;
            uniform float _Foamspeed;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform fixed _ScreenMode;
            uniform float _TilingFoam;
            uniform float _Amount;
            uniform float _Contrast;
            uniform float _Intensity;
            uniform fixed _OverlayMode;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                LIGHTING_COORDS(7,8)
                UNITY_FOG_COORDS(9)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                UNITY_LIGHT_ATTENUATION(attenuation,i, i.posWorld.xyz);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float node_3783 = ((i.uv0.g+_Amount)*_Contrast);
                float4 node_4869 = _Time;
                float node_7662_ang = node_4869.g;
                float node_7662_spd = _Foamspeed;
                float node_7662_cos = cos(node_7662_spd*node_7662_ang);
                float node_7662_sin = sin(node_7662_spd*node_7662_ang);
                float2 node_7662_piv = float2(0.5,0.5);
                float2 node_2694 = ((i.uv0*4.0)*_TilingFoam);
                float2 node_7662 = (mul(node_2694-node_7662_piv,float2x2( node_7662_cos, -node_7662_sin, node_7662_sin, node_7662_cos))+node_7662_piv);
                float4 _FoamTexture1 = tex2D(_MainTex,TRANSFORM_TEX(node_7662, _MainTex));
                float node_200_ang = node_4869.g;
                float node_200_spd = (-1*_Foamspeed);
                float node_200_cos = cos(node_200_spd*node_200_ang);
                float node_200_sin = sin(node_200_spd*node_200_ang);
                float2 node_200_piv = float2(0.5,0.5);
                float2 node_200 = (mul(node_2694-node_200_piv,float2x2( node_200_cos, -node_200_sin, node_200_sin, node_200_cos))+node_200_piv);
                float2 node_9929 = (node_200+float2(0.6,0.6));
                float4 _FoamTexture2 = tex2D(_MainTex,TRANSFORM_TEX(node_9929, _MainTex));
                float3 node_5599 = (saturate((_FoamTexture1.rgb*_FoamTexture2.rgb))*1.663);
                float3 node_3470 = saturate(lerp( node_5599, saturate((1.0-(1.0-node_5599)*(1.0-node_5599))), _ScreenMode ));
                float3 Diffuse = (_Color.rgb*lerp( saturate((1.0-((1.0-node_3470)/node_3783))), saturate(( node_3470 > 0.5 ? (1.0-(1.0-2.0*(node_3470-0.5))*(1.0-node_3783)) : (2.0*node_3470*node_3783) )), _OverlayMode ));
                float3 diffuseColor = Diffuse;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                float AlphaMask = node_3783;
                fixed4 finalRGBA = fixed4(finalColor * saturate((AlphaMask*_Intensity)),0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "Meta"
            Tags {
                "LightMode"="Meta"
            }
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #include "UnityMetaPass.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform float4 _Color;
            uniform float _Foamspeed;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform fixed _ScreenMode;
            uniform float _TilingFoam;
            uniform float _Amount;
            uniform float _Contrast;
            uniform fixed _OverlayMode;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                return o;
            }
            float4 frag(VertexOutput i) : SV_Target {
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                o.Emission = 0;
                
                float node_3783 = ((i.uv0.g+_Amount)*_Contrast);
                float4 node_8378 = _Time;
                float node_7662_ang = node_8378.g;
                float node_7662_spd = _Foamspeed;
                float node_7662_cos = cos(node_7662_spd*node_7662_ang);
                float node_7662_sin = sin(node_7662_spd*node_7662_ang);
                float2 node_7662_piv = float2(0.5,0.5);
                float2 node_2694 = ((i.uv0*4.0)*_TilingFoam);
                float2 node_7662 = (mul(node_2694-node_7662_piv,float2x2( node_7662_cos, -node_7662_sin, node_7662_sin, node_7662_cos))+node_7662_piv);
                float4 _FoamTexture1 = tex2D(_MainTex,TRANSFORM_TEX(node_7662, _MainTex));
                float node_200_ang = node_8378.g;
                float node_200_spd = (-1*_Foamspeed);
                float node_200_cos = cos(node_200_spd*node_200_ang);
                float node_200_sin = sin(node_200_spd*node_200_ang);
                float2 node_200_piv = float2(0.5,0.5);
                float2 node_200 = (mul(node_2694-node_200_piv,float2x2( node_200_cos, -node_200_sin, node_200_sin, node_200_cos))+node_200_piv);
                float2 node_9929 = (node_200+float2(0.6,0.6));
                float4 _FoamTexture2 = tex2D(_MainTex,TRANSFORM_TEX(node_9929, _MainTex));
                float3 node_5599 = (saturate((_FoamTexture1.rgb*_FoamTexture2.rgb))*1.663);
                float3 node_3470 = saturate(lerp( node_5599, saturate((1.0-(1.0-node_5599)*(1.0-node_5599))), _ScreenMode ));
                float3 Diffuse = (_Color.rgb*lerp( saturate((1.0-((1.0-node_3470)/node_3783))), saturate(( node_3470 > 0.5 ? (1.0-(1.0-2.0*(node_3470-0.5))*(1.0-node_3783)) : (2.0*node_3470*node_3783) )), _OverlayMode ));
                float3 diffColor = Diffuse;
                o.Albedo = diffColor;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
}
