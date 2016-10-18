//vertex shader
#version 450 core
layout(location = 0) in vec3 local_position;
layout(location = 1) in vec2 in_texture;
layout(location = 2) in vec3 local_normal;

uniform mat4 projection_matrix, view_matrix;
uniform mat4 model;
uniform mat3 normal_matrix;

out vec2 texcoord;
out vec3 world_normal;

void main()
{

	texcoord = in_texture;
	world_normal = normal_matrix * local_normal;

	//gl_Position = projection_matrix * view_matrix * rotate_y * rotate_x *rotate_z * vec4(in_position, 1);
	gl_Position = projection_matrix * view_matrix * model * vec4(local_position, 1);
	
}