// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:32719,y:32712,varname:node_3138,prsc:2|emission-7863-OUT,alpha-5420-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:32471,y:32812,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.07843138,c2:0.3921569,c3:0.7843137,c4:1;n:type:ShaderForge.SFN_Tex2d,id:4568,x:32086,y:33041,ptovrint:False,ptlb:node_4568,ptin:_node_4568,varname:node_4568,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:c8e449fcd903a934ba1d0288c449e850,ntxv:0,isnm:False;n:type:ShaderForge.SFN_OneMinus,id:4794,x:32302,y:33041,varname:node_4794,prsc:2|IN-4568-R;n:type:ShaderForge.SFN_Time,id:6059,x:31927,y:33190,varname:node_6059,prsc:2;n:type:ShaderForge.SFN_Multiply,id:5420,x:32452,y:33191,varname:node_5420,prsc:2|A-4794-OUT,B-2533-OUT,C-3108-OUT;n:type:ShaderForge.SFN_Cos,id:3108,x:32246,y:33216,varname:node_3108,prsc:2|IN-6059-TTR;n:type:ShaderForge.SFN_Abs,id:6368,x:32246,y:33440,varname:node_6368,prsc:2|IN-2225-OUT;n:type:ShaderForge.SFN_Noise,id:2225,x:32086,y:33440,varname:node_2225,prsc:2|XY-3804-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:9345,x:31718,y:33396,varname:node_9345,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Panner,id:3804,x:31894,y:33396,varname:node_3804,prsc:2,spu:1,spv:0|UVIN-9345-UVOUT;n:type:ShaderForge.SFN_DDX,id:2533,x:32414,y:33428,varname:node_2533,prsc:2|IN-6368-OUT;n:type:ShaderForge.SFN_Append,id:7863,x:32613,y:33410,varname:node_7863,prsc:2|A-2533-OUT,B-6368-OUT;proporder:7241-4568;pass:END;sub:END;*/

Shader "Shader Forge/LaserBeamShader" {
    Properties {
        _Color ("Color", Color) = (0.07843138,0.3921569,0.7843137,1)
        _node_4568 ("node_4568", 2D) = "white" {}
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
            uniform sampler2D _node_4568; uniform float4 _node_4568_ST;
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
                float4 node_3555 = _Time;
                float2 node_3804 = (i.uv0+node_3555.g*float2(1,0));
                float2 node_2225_skew = node_3804 + 0.2127+node_3804.x*0.3713*node_3804.y;
                float2 node_2225_rnd = 4.789*sin(489.123*(node_2225_skew));
                float node_2225 = frac(node_2225_rnd.x*node_2225_rnd.y*(1+node_2225_skew.x));
                float node_6368 = abs(node_2225);
                float node_2533 = ddx(node_6368);
                float3 emissive = float3(float2(node_2533,node_6368),0.0);
                float3 finalColor = emissive;
                float4 _node_4568_var = tex2D(_node_4568,TRANSFORM_TEX(i.uv0, _node_4568));
                float4 node_6059 = _Time;
                float node_5420 = ((1.0 - _node_4568_var.r)*node_2533*cos(node_6059.a));
                return fixed4(finalColor,node_5420);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
