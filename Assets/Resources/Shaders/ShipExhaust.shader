// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:33753,y:33224,varname:node_3138,prsc:2|emission-6978-OUT,alpha-7108-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:32546,y:33022,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.07843138,c2:0.3921569,c3:0.7843137,c4:1;n:type:ShaderForge.SFN_UVTile,id:9295,x:32743,y:33518,varname:node_9295,prsc:2|WDT-5599-OUT,HGT-8393-OUT,TILE-9614-OUT;n:type:ShaderForge.SFN_Vector1,id:5599,x:32594,y:33418,varname:node_5599,prsc:2,v1:1;n:type:ShaderForge.SFN_Vector1,id:8393,x:32333,y:33520,varname:node_8393,prsc:2,v1:1;n:type:ShaderForge.SFN_Tex2d,id:8671,x:32963,y:33518,ptovrint:False,ptlb:Noise,ptin:_Noise,varname:_Noise,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:d9a4f1b75d6a9cd46890f99a6bc6fead,ntxv:0,isnm:False|UVIN-9295-UVOUT;n:type:ShaderForge.SFN_Time,id:8632,x:32220,y:33586,varname:node_8632,prsc:2;n:type:ShaderForge.SFN_Vector1,id:2610,x:32198,y:33783,varname:node_2610,prsc:2,v1:-0.25;n:type:ShaderForge.SFN_Multiply,id:9614,x:32556,y:33572,varname:node_9614,prsc:2|A-8632-T,B-2610-OUT;n:type:ShaderForge.SFN_Tex2d,id:6964,x:33027,y:32974,ptovrint:False,ptlb:TopBottomFade,ptin:_TopBottomFade,varname:_TopBottomFade,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:6ddf4f131ee8e1b4a8889e4d40a6778f,ntxv:1,isnm:False;n:type:ShaderForge.SFN_Vector1,id:6483,x:32874,y:33007,varname:node_6483,prsc:2,v1:-1;n:type:ShaderForge.SFN_Vector1,id:4952,x:32913,y:33189,varname:node_4952,prsc:2,v1:1;n:type:ShaderForge.SFN_Multiply,id:8684,x:33182,y:33063,varname:node_8684,prsc:2|A-6964-RGB,B-6483-OUT;n:type:ShaderForge.SFN_Add,id:3111,x:33216,y:33219,varname:node_3111,prsc:2|A-8684-OUT,B-4952-OUT;n:type:ShaderForge.SFN_Subtract,id:7855,x:33184,y:33467,varname:node_7855,prsc:2|A-3111-OUT,B-8671-RGB;n:type:ShaderForge.SFN_Multiply,id:6978,x:32426,y:33262,varname:node_6978,prsc:2|A-7241-RGB,B-7855-OUT;n:type:ShaderForge.SFN_Min,id:476,x:33426,y:33572,varname:node_476,prsc:2|A-3111-OUT,B-9709-OUT;n:type:ShaderForge.SFN_ComponentMask,id:7108,x:33592,y:33490,varname:node_7108,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-476-OUT;n:type:ShaderForge.SFN_TexCoord,id:5438,x:32973,y:33780,varname:node_5438,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_OneMinus,id:9709,x:33184,y:33699,varname:node_9709,prsc:2|IN-5438-U;proporder:7241-8671-6964;pass:END;sub:END;*/

Shader "Shader Forge/ShipExhaust" {
    Properties {
        _Color ("Color", Color) = (0.07843138,0.3921569,0.7843137,1)
        _Noise ("Noise", 2D) = "white" {}
        _TopBottomFade ("TopBottomFade", 2D) = "gray" {}
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _Color;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform sampler2D _TopBottomFade; uniform float4 _TopBottomFade_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 _TopBottomFade_var = tex2D(_TopBottomFade,TRANSFORM_TEX(i.uv0, _TopBottomFade));
                float3 node_3111 = ((_TopBottomFade_var.rgb*(-1.0))+1.0);
                float node_5599 = 1.0;
                float4 node_8632 = _Time;
                float node_9614 = (node_8632.g*(-0.25));
                float2 node_9295_tc_rcp = float2(1.0,1.0)/float2( node_5599, 1.0 );
                float node_9295_ty = floor(node_9614 * node_9295_tc_rcp.x);
                float node_9295_tx = node_9614 - node_5599 * node_9295_ty;
                float2 node_9295 = (i.uv0 + float2(node_9295_tx, node_9295_ty)) * node_9295_tc_rcp;
                float4 _Noise_var = tex2D(_Noise,TRANSFORM_TEX(node_9295, _Noise));
                float3 emissive = (_Color.rgb*(node_3111-_Noise_var.rgb));
                float3 finalColor = emissive;
                return fixed4(finalColor,min(node_3111,(1.0 - i.uv0.r)).r);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
