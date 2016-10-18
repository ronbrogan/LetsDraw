//fragment shader
#version 450 core
 
layout(location = 0) out vec4 out_color;

uniform sampler2D texture1;
 
in vec2 texcoord;
in vec3 world_normal;
 
void main()
{
	vec3 lightColor = vec3(1, 0.98, 0.84);
	vec3 lightDirection = vec3(1, 0.707, 0.5);

	float cosTheta = clamp(dot(world_normal, lightDirection), 0, 1);

	vec4 diffuse = texture(texture1, texcoord);
	vec4 ambient = diffuse * vec4(0.15, 0.15, 0.15, 1);

	out_color = ambient + diffuse * vec4(lightColor, 1) * cosTheta;
}