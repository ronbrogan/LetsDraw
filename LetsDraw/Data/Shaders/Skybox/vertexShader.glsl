//vertex shader
#version 450 core
layout(location = 0) in vec3 local_position;
layout(location = 1) in vec2 in_texture;
layout(location = 2) in vec3 local_normal;

uniform mat4 projection_matrix, view_matrix, model_matrix;

out vec2 texcoord;

void main()
{
	texcoord = in_texture;

	gl_Position = projection_matrix * view_matrix * vec4(local_position, 1);
}