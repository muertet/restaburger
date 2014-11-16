Shader "TextureDraw" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _MainTex ("Background", 2D) = "white" {}
 _Drawing ("Drawing", 2D) = "white" {}
}
SubShader { 
 Pass {
  SetTexture [_MainTex] { combine texture, texture alpha }
  SetTexture [_Drawing] { ConstantColor (0,0,0,1) combine previous * texture }
 }
}
}