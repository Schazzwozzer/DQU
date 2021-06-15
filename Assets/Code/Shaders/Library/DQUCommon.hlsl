#ifndef DQU_COMMON_INCLUDED
#define DQU_COMMON_INCLUDED

// Nvidia's "empirical approximation"
half CalculateFresnel( 
    half fresnelBias, half fresnelScale, half fresnelPower, 
    float3 viewDir, float3 worldNormal )
{
	return fresnelBias + fresnelScale * 
        pow( max( 0, 1.0 + dot( -viewDir, worldNormal )), fresnelPower );
}

#endif