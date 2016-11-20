//fragment shader
#version 450 core
 
layout(location = 0) out vec4 out_color;

layout(std140, binding = 0) uniform MatrixUniform
{
	mat4 ViewMatrix;
	mat4 ProjectionMatrix;
	mat4 DetranslatedViewMatrix;
} Matricies;

layout(std140, binding = 1) uniform GenericUniform
{
	mat4 ModelMatrix;
	mat4 NormalMatrix;
	vec3 DiffuseColor;
	float Alpha;
	int DiffuseMapIndex;
	bool UseDiffuseMap;
} Data;

layout(binding = 2) uniform sampler2DArray Textures;

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

	vec4 diffuse = vec4(Data.DiffuseColor, 0);

	float bug = 0.0;

	if (Data.DiffuseMapIndex == 0) {
		bug = 1.0;
	}

	if(Data.UseDiffuseMap){
		 diffuse = texture(Textures, vec3(texcoord.x, texcoord.y, Data.DiffuseMapIndex));
	}

	vec4 color = vec4(lighting.xyz * diffuse.xyz, Data.Alpha);

	out_color = color;// vec4(bug, 0, 0, 1);
}