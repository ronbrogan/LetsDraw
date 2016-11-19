//vertex shader
#version 450 core
layout(location = 0) in vec3 local_position;
layout(location = 1) in vec2 in_texture;
layout(location = 2) in vec3 local_normal;

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
	bool UseDiffuseMap;
} Data;

out vec2 texcoord;
out vec3 world_normal;

void main()
{
	texcoord = in_texture;
	world_normal = normalize(mat3(Data.NormalMatrix) * local_normal);

	gl_Position = Matricies.ProjectionMatrix * Matricies.ViewMatrix * Data.ModelMatrix * vec4(local_position, 1);
}