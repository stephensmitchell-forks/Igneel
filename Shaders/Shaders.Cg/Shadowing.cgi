#define SHADOWING

#ifndef GLOBALS
	#include "Globals.hlsli"
#endif

cbuffer cbShadowMap
{	
	float SHADOW_EPSILON;	
};

//GLOBALS
static float4 gPositionL;
static float4 gShadowTexCoord;


void ComputeShadowTexCoord()
{
	float3 posL = gPositionL.xyz / gPositionL.w;
	gShadowTexCoord = float4(posL.x * 0.5f + 0.5f, -posL.y * 0.5f + 0.5f, posL.z , 1);
	gShadowTexCoord.z -= SHADOW_EPSILON;
}
