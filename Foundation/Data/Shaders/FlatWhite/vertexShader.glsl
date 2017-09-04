#version 450


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

layout(location = 0) in vec3 local_position;

void main() {
    gl_Position = Matricies.ProjectionMatrix * Matricies.ViewMatrix * Data.ModelMatrix * vec4(local_position, 1);
}