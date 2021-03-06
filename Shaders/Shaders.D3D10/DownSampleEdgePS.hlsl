#define KERNEL_SIZE 3

struct PS_RenderQuadInput
{
    float4 position : SV_POSITION; 
    float2 texCoord : TEXCOORD0; 
};

Texture2D Texture : register(t0);
SamplerState sTexture : register(s0);

float4 main(PS_RenderQuadInput input) : SV_TARGET
{	
   float color = 0;
   int start = -KERNEL_SIZE / 2;
   int end = KERNEL_SIZE / 2 + 1;
   float width, height;
   Texture.GetDimensions(width, height);
   float tx = 1.0 / width;
   float ty = 1.0 / height;

    [unroll]
	for(float y = start ; y < end ; y++)
	{
		[unroll]
		for(float x = start ; x < end ; x++)
		{
			color += Texture.Sample(sTexture, input.texCoord + float2(x*tx, y*ty)).r;
		}
	}		

	float r = saturate(color);	
	return float4(r,r,r,1);
}