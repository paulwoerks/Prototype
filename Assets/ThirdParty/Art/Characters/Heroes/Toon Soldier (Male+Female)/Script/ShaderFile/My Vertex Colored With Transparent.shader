Shader "Custom/Vertex Colored TransParent" {
Properties {
    _Color ("Main Color", Color) = (1,1,1,1)
    _MainTex ("Base (RGB)", 2D) = "white" {}
}
 
SubShader {
	Tags {"Queue"="Transparent" "RenderType"="Transparent"}//give to transparent
    Pass {
        ColorMaterial AmbientAndDiffuse
        Blend SrcAlpha OneMinusSrcAlpha //Add This
        Lighting Off
        Fog { Mode Off }
        SetTexture [_MainTex] {
            Combine texture * primary, texture * primary
        }
        SetTexture [_MainTex] {
            constantColor [_Color]
            Combine previous * constant DOUBLE, previous * constant
        } 
    }
}
 
//Fallback " VertexLit", 1
}