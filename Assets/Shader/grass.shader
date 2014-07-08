// Shader created with Shader Forge Beta 0.32 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.32;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,hqsc:True,hqlp:False,blpr:0,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:True,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.1280277,fgcg:0.1953466,fgcb:0.2352941,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:33087,y:32489|emission-39-OUT,clip-9-A,voffset-36-OUT;n:type:ShaderForge.SFN_TexCoord,id:3,x:33909,y:32678,uv:0;n:type:ShaderForge.SFN_Sin,id:5,x:33909,y:32841|IN-30-OUT;n:type:ShaderForge.SFN_Tex2d,id:9,x:33910,y:32480,ptlb:diffuse,ptin:_diffuse,tex:16e2a47b297c2724998279ddee5b616a,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Time,id:28,x:34308,y:32768;n:type:ShaderForge.SFN_FragmentPosition,id:29,x:34308,y:32927;n:type:ShaderForge.SFN_Add,id:30,x:34101,y:32841|A-28-T,B-29-X;n:type:ShaderForge.SFN_Multiply,id:36,x:33586,y:32775|A-3-V,B-5-OUT,C-48-OUT;n:type:ShaderForge.SFN_VertexColor,id:38,x:33910,y:32298;n:type:ShaderForge.SFN_Multiply,id:39,x:33631,y:32354|A-38-RGB,B-9-RGB;n:type:ShaderForge.SFN_ValueProperty,id:48,x:33909,y:33031,ptlb:speed,ptin:_speed,glob:False,v1:0.04;proporder:9-48;pass:END;sub:END;*/

Shader "Shader Forge/grass" {
    Properties {
        _diffuse ("diffuse", 2D) = "white" {}
        _speed ("speed", Float ) = 0.04
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers d3d11 opengl xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _diffuse; uniform float4 _diffuse_ST;
            uniform float _speed;
            struct VertexInput {
                float4 vertex : POSITION;
                float4 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.uv0;
                o.vertexColor = v.vertexColor;
                float4 node_28 = _Time + _TimeEditor;
                float node_36 = (o.uv0.g*sin((node_28.g+mul(_Object2World, v.vertex).r))*_speed);
                v.vertex.xyz += float3(node_36,node_36,node_36);
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                float2 node_54 = i.uv0;
                float4 node_9 = tex2D(_diffuse,TRANSFORM_TEX(node_54.rg, _diffuse));
                clip(node_9.a - 0.5);
////// Lighting:
////// Emissive:
                float3 emissive = (i.vertexColor.rgb*node_9.rgb);
                float3 finalColor = emissive;
/// Final Color:
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ShadowCollector"
            Tags {
                "LightMode"="ShadowCollector"
            }
            
            Fog {Mode Off}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCOLLECTOR
            #define SHADOW_COLLECTOR_PASS
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcollector
            #pragma exclude_renderers d3d11 opengl xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _diffuse; uniform float4 _diffuse_ST;
            uniform float _speed;
            struct VertexInput {
                float4 vertex : POSITION;
                float4 uv0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_COLLECTOR;
                float4 uv0 : TEXCOORD5;
                float4 posWorld : TEXCOORD6;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.uv0;
                float4 node_28 = _Time + _TimeEditor;
                float node_36 = (o.uv0.g*sin((node_28.g+mul(_Object2World, v.vertex).r))*_speed);
                v.vertex.xyz += float3(node_36,node_36,node_36);
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_SHADOW_COLLECTOR(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                float2 node_55 = i.uv0;
                float4 node_9 = tex2D(_diffuse,TRANSFORM_TEX(node_55.rg, _diffuse));
                clip(node_9.a - 0.5);
                SHADOW_COLLECTOR_FRAGMENT(i)
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Cull Off
            Offset 1, 1
            
            Fog {Mode Off}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma exclude_renderers d3d11 opengl xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _diffuse; uniform float4 _diffuse_ST;
            uniform float _speed;
            struct VertexInput {
                float4 vertex : POSITION;
                float4 uv0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float4 uv0 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.uv0;
                float4 node_28 = _Time + _TimeEditor;
                float node_36 = (o.uv0.g*sin((node_28.g+mul(_Object2World, v.vertex).r))*_speed);
                v.vertex.xyz += float3(node_36,node_36,node_36);
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                float2 node_56 = i.uv0;
                float4 node_9 = tex2D(_diffuse,TRANSFORM_TEX(node_56.rg, _diffuse));
                clip(node_9.a - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
