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

out vec2 texcoord;

void main()
{
	texcoord = in_texture;

	vec4 pos = Matricies.ProjectionMatrix * Matricies.DetranslatedViewMatrix * vec4(local_position, 1);

	// Augment z to make it appear to be the furthest away object.
	gl_Position = vec4(pos.x, pos.y, pos.w - 0.000002, pos.w);
}