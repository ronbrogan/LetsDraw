//fragment shader
#version 450 core
 
layout(location = 0) out vec4 out_color;

uniform sampler2D texture1;
 
in vec2 texcoord;
 
void main()
{
  vec4 color = texture(texture1, texcoord);
  /*if (color.a < 0.2)
	  discard;*/
  out_color = color;
}