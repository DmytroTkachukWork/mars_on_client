void CalculateContrast_float( float contrastValue, float4 colorTarget, inout float4 rez )
{
	float t = 0.5 * ( 1.0 - contrastValue );
	rez = mul( float4x4( contrastValue,0,0,t, 0,contrastValue,0,t, 0,0,contrastValue,t, 0,0,0,1 ), colorTarget );
}