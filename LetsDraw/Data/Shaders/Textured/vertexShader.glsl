//vertex shader
#version 450 core
layout(location = 0) in vec3 local_position;
layout(location = 1) in vec2 in_texture;
layout(location = 2) in vec3 local_normal;

uniform mat4 projection_matrix, view_matrix, model_matrix;
uniform mat3 normal_matrix;

out vec2 texcoord;
out vec3 world_normal;

void main()
{
	texcoord = in_texture;
	world_normal = normalize(normal_matrix * local_normal);

	gl_Position = projection_matrix * view_matrix * model_matrix * vec4(local_position, 1);
}