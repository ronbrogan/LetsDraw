//vertex shader
#version 450 core
layout(location = 0) in vec3 in_position;
layout(location = 1) in vec2 in_texture;

uniform mat4 projection_matrix, view_matrix;
uniform mat4 model;

out vec2 texcoord;

void main()
{

	texcoord = in_texture;

	//gl_Position = projection_matrix * view_matrix * rotate_y * rotate_x *rotate_z * vec4(in_position, 1);
	gl_Position = projection_matrix * view_matrix * model * vec4(in_position, 1);
	
}