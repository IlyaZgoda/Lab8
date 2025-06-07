#version 330 core

in vec3 Normal;
in vec3 FragPos;

out vec4 FragColor;

uniform vec3 objectColor;
uniform vec3 lightPos;
uniform vec3 lightColor;
uniform vec3 viewPos;
uniform bool isSun;
uniform float time;


float noise(vec2 p) {
    return fract(sin(dot(p, vec2(12.9898, 78.233))) * 43758.5453);
}

void main()
{
    if (isSun) {

        vec2 uv = FragPos.xy * 5.0;
        

        float n1 = noise(uv + time * 0.1);
        float n2 = noise(uv * 2.0 - time * 0.05);
        

        vec3 sunColor = objectColor;
        

        sunColor = mix(
            sunColor * 0.9,       
            sunColor * 1.2,       
            n1 * n2               
        );
        

        vec3 viewDir = normalize(viewPos - FragPos);
        vec3 normal = normalize(Normal);
        float rim = 1.0 - dot(normal, viewDir);
        float glow = pow(rim, 2.0) * 0.8; 

        sunColor = mix(sunColor, vec3(1.0, 0.7, 0.4), glow);
        sunColor = clamp(sunColor, 0.0, 1.5); 
        
        FragColor = vec4(sunColor, 1.0);
        return;
    }


    float ambientStrength = 0.3;
    vec3 ambient = ambientStrength * lightColor;

    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(lightPos - FragPos);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = diff * lightColor;

    float specularStrength = 1;
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);  
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
    vec3 specular = specularStrength * spec * lightColor;  

    vec3 result = (ambient + diffuse + specular) * objectColor;
    FragColor = vec4(result, 1.0);
}