//fragment shader
#version 450 core
 
layout(location = 0) out vec4 out_color;

layout (binding = 0) uniform sampler2D diffuse_map;

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
	vec3 DiffuseColor;
	float Alpha;
	float SpecularExponent;
	bool UseDiffuseMap;
} Data;

in vec3 position;
in vec2 texcoord;
in vec3 world_normal;
 
const vec4 lightColor = vec4(1, 0.98, 0.84, 1);
const vec3 lightDirection = vec3(-1, 1, 0.5);

void main()
{
	vec3 viewDirection = normalize(Matricies.ViewPosition - position);
	vec3 halfwayDirection = normalize(lightDirection + viewDirection);

	vec4 color = vec4(Data.DiffuseColor, Data.Alpha);
	if(Data.UseDiffuseMap){
		 color = texture(diffuse_map, texcoord);
	}
	vec4 tintedColor = vec4((color * lightColor).rgb, Data.Alpha);

	vec4 light_ambient = vec4((tintedColor * 0.15).rgb, Data.Alpha);

	float cosTheta = clamp(dot(lightDirection, world_normal), 0.0, 1.0);
	vec4 light_diffuse = vec4((tintedColor * cosTheta).rgb, Data.Alpha);

	float specularAngle = max(dot(world_normal, halfwayDirection), 0.0);
	float specularModifier = pow(specularAngle, 1024 - pow(Data.SpecularExponent, 1.6));
	vec4 light_specular = vec4((lightColor * specularModifier).rgb, 0);


	vec4 lighting = vec4((light_ambient + light_diffuse).rgb, Data.Alpha) + light_specular;

	out_color = lighting;
}