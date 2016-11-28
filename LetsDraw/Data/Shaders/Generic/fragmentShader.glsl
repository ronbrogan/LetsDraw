//fragment shader
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

in vec3 position;
in vec2 texcoord;
in vec3 world_normal;

vec3 viewDifference = Matricies.ViewPosition - position;
float viewDistance = length(viewDifference);
vec3 viewDirection = normalize(viewDifference);

const vec4 lightColor = vec4(1, 0.98, 0.84, 1);
const vec3 lightDirection = vec3(-1, 1, 0.5);

vec4 lightCalculation(in PointLight light, in vec4 textureColor);
vec3 globalLighting(in vec3 textureColor);

void main()
{
	vec4 color = vec4(Data.DiffuseColor.rgb, Data.Alpha);
	if(Data.UseDiffuseMap){
		 color = texture(diffuse_map, texcoord);
	}
	
	// Sets ambient baseline
	vec3 finalColor = color.rgb * vec3(0.1, 0.1, 0.1);

	// Adds global lighting
	finalColor += globalLighting(color.rgb);

	// Accumulates point lights
	for(int i = 0; i < MAX_NUM_TOTAL_LIGHTS; i++)
	{
		if(PointLights.Lights[i].intensity <= 0.0) 
		{
			continue;
		}
		finalColor += lightCalculation(PointLights.Lights[i], color).rgb;
	}

	out_color = vec4(finalColor, Data.Alpha);
}

vec3 globalLighting(in vec3 textureColor)
{
	float cosTheta = clamp(dot(lightDirection, world_normal), 0.0, 1.0);
	vec3 light_diffuse = textureColor * lightColor.rgb * cosTheta;

	float specularFalloff = 1 / (viewDistance * viewDistance );

	vec3 halfwayDirection = normalize(lightDirection + viewDirection);
	float specularAngle = max(dot(world_normal, halfwayDirection), 0.0);
	float specularModifier = pow(specularAngle, Data.SpecularExponent);
	vec3 light_specular = lightColor.rgb * Data.SpecularColor.rgb * specularModifier;
	
	return light_diffuse + light_specular;
}

vec4 lightCalculation(in PointLight light, in vec4 textureColor)
{
	vec3 lightDir = light.position.xyz - position;
	float dist = length(lightDir);
	lightDir = normalize(lightDir);
	float cosTheta = max(dot(lightDir, world_normal), 0.0);
	if (cosTheta <= 0)
	{
		return vec4(0,0,0,0);
	}

	float lightCutoff = 10 / light.intensity;

	// Diffuse calculations
	float denom = dist/light.range + 1;
	float falloffTerm = 1/(denom*denom);
	falloffTerm = (falloffTerm - lightCutoff) / (1 - lightCutoff);
	falloffTerm = max(falloffTerm, 0);
	vec4 light_diffuse = light.color * textureColor * cosTheta * falloffTerm;

	// Specular calculations
	vec3 halfwayDirection = normalize(lightDir + viewDirection);
	float specularAngle = max(dot(world_normal, halfwayDirection), 0.0);
	float specularModifier = pow(specularAngle, Data.SpecularExponent);
	vec4 light_specular = vec4((light.color.rgb * Data.SpecularColor.rgb * specularModifier), 0) * falloffTerm;
	
	
	return light_diffuse + light_specular;
}