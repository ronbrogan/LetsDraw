//vertex shader
#version 450 core
const int MAX_NUM_TOTAL_LIGHTS = 8;
struct PointLight {
	vec4 position;
	vec4 color;
	float intensity;
	float range;
	bool castsShadows;
	bool anotherFlag;
};

layout(location = 0) in vec3 local_position;
layout(location = 1) in vec2 in_texture;
layout(location = 2) in vec3 local_normal;

layout(std140, binding = 0) uniform MatrixUniform
{
	mat4 ViewMatrix;
	mat4 ProjectionMatrix;
	mat4 DetranslatedViewMatrix;
	vec3 ViewPosition;
} Matricies;

layout(std140, binding = 1) uniform GenericUniform
{
	mat4 ModelMatrix;
	mat4 NormalMatrix;
	vec4 DiffuseColor;
	vec4 SpecularColor;
	float Alpha;
	float SpecularExponent;
	bool UseDiffuseMap;
} Data;

layout(std140, binding = 2) uniform PointLightContainer
{
	PointLight Lights[MAX_NUM_TOTAL_LIGHTS];
} PointLights;

out vec3 position;
out vec2 texcoord;
out vec3 world_normal;

void main()
{
	mat4 modelView = Matricies.ViewMatrix * Data.ModelMatrix;
	texcoord = in_texture;
	world_normal = normalize(mat3(Data.NormalMatrix) * local_normal);
	position = local_position;

	gl_Position = Matricies.ProjectionMatrix * modelView * vec4(local_position, 1);
}