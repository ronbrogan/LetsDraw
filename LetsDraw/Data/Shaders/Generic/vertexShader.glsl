//vertex shader
#version 450 core
layout(location = 0) in vec3 local_position;
layout(location = 1) in vec2 in_texture;
layout(location = 2) in vec3 local_normal;

layout(std140, binding = 1) uniform GenericUniform
{
	mat4 ModelMatrix;
	mat4 ViewMatrix;
	mat4 ProjectionMatrix;
	mat3 NormalMatrix;
	vec3 DiffuseColor;
	float Alpha;
	bool UseDiffuseMap;
} Data;

out vec2 texcoord;
out vec3 world_normal;

void main()
{
	texcoord = in_texture;
	mat3 normtrans = transpose(inverse(mat3(Data.ModelMatrix)));
	world_normal = normalize(normtrans * local_normal);

	gl_Position = Data.ProjectionMatrix * Data.ViewMatrix * Data.ModelMatrix * vec4(local_position, 1);
}