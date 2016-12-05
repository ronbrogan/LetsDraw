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
layout(location = 3) in vec3 tangent;
layout(location = 4) in vec3 bitangent;

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
	bool UseNormalMap;
	bool UseSpecularMap;
} Data;

layout(std140, binding = 2) uniform PointLightContainer
{
	PointLight Lights[MAX_NUM_TOTAL_LIGHTS];
} PointLights;

out vec3 position;
out vec2 texcoord;
out vec3 world_normal;
out mat3 TBN;

void main()
{
	mat4 modelView = Matricies.ViewMatrix * Data.ModelMatrix;
	mat3 mat3nm = mat3(Data.NormalMatrix);

	texcoord = in_texture;
	world_normal = normalize(mat3nm * local_normal);
	position = local_position;

	if (Data.UseNormalMap) {
		vec3 world_tangent = normalize(mat3nm * tangent);
		vec3 world_bitangent = normalize(mat3nm * bitangent);

		TBN = transpose(mat3(
			world_tangent,
			world_bitangent,
			world_normal
		));
	}
	else {
		TBN = mat3(1);
	}

	

	gl_Position = Matricies.ProjectionMatrix * modelView * vec4(local_position, 1);
}