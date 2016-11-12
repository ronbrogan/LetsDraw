//fragment shader
#version 450 core
 
layout(location = 0) out vec4 out_color;

uniform bool use_diffuse_map;
uniform sampler2D diffuse_map;
uniform vec3 diffuse_color;

uniform float alpha;
 
in vec2 texcoord;
in vec3 world_normal;
 
void main()
{
	vec3 lightColor = vec3(1, 0.98, 0.84);
	vec3 lightDirection = vec3(-1, 1, 0.5);

	float cosTheta = clamp(dot(world_normal, lightDirection), 0, 1);

	vec4 light_ambient = vec4(lightColor, 1) * vec4(0.2, 0.2, 0.2, 1);
	vec4 light_diffuse = vec4(lightColor, 1) * cosTheta;
	vec4 lighting = light_ambient + light_diffuse;

	vec4 diffuse = vec4(diffuse_color, 0);

	if(use_diffuse_map){
		 diffuse = texture(diffuse_map, texcoord);
	}

	out_color = vec4(lighting.xyz * diffuse.xyz, alpha);
}