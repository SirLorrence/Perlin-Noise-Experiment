using UnityEngine;
using UnityEngine.Assertions.Comparers;

public static class Noise
{
    /*
     * From Perlin noise wiki....im not writting all that out
     * https://en.wikipedia.org/wiki/Perlin_noise
     */
    static int[] permutation = {
        151, 160, 137, 91, 90, 15, 131, 13, 201, 95, 96, 53, 194, 233, 7, 225, 140, 36,
        103, 30, 69, 142, 8, 99, 37, 240, 21, 10, 23, 190, 6, 148, 247, 120, 234, 75, 0,
        26, 197, 62, 94, 252, 219, 203, 117, 35, 11, 32, 57, 177, 33, 88, 237, 149, 56,
        87, 174, 20, 125, 136, 171, 168, 68, 175, 74, 165, 71, 134, 139, 48, 27, 166,
        77, 146, 158, 231, 83, 111, 229, 122, 60, 211, 133, 230, 220, 105, 92, 41, 55,
        46, 245, 40, 244, 102, 143, 54, 65, 25, 63, 161, 1, 216, 80, 73, 209, 76, 132,
        187, 208, 89, 18, 169, 200, 196, 135, 130, 116, 188, 159, 86, 164, 100, 109,
        198, 173, 186, 3, 64, 52, 217, 226, 250, 124, 123, 5, 202, 38, 147, 118, 126,
        255, 82, 85, 212, 207, 206, 59, 227, 47, 16, 58, 17, 182, 189, 28, 42, 223, 183,
        170, 213, 119, 248, 152, 2, 44, 154, 163, 70, 221, 153, 101, 155, 167, 43,
        172, 9, 129, 22, 39, 253, 19, 98, 108, 110, 79, 113, 224, 232, 178, 185, 112,
        104, 218, 246, 97, 228, 251, 34, 242, 193, 238, 210, 144, 12, 191, 179, 162,
        241, 81, 51, 145, 235, 249, 14, 239, 107, 49, 192, 214, 31, 181, 199, 106,
        157, 184, 84, 204, 176, 115, 121, 50, 45, 127, 4, 150, 254, 138, 236, 205,
        93, 222, 114, 67, 29, 24, 72, 243, 141, 128, 195, 78, 66, 215, 61, 156, 180,

        //in case the maximum value of 255 is passed
        151, 160, 137, 91, 90, 15, 131, 13, 201, 95, 96, 53, 194, 233, 7, 225, 140, 36,
        103, 30, 69, 142, 8, 99, 37, 240, 21, 10, 23, 190, 6, 148, 247, 120, 234, 75, 0,
        26, 197, 62, 94, 252, 219, 203, 117, 35, 11, 32, 57, 177, 33, 88, 237, 149, 56,
        87, 174, 20, 125, 136, 171, 168, 68, 175, 74, 165, 71, 134, 139, 48, 27, 166,
        77, 146, 158, 231, 83, 111, 229, 122, 60, 211, 133, 230, 220, 105, 92, 41, 55,
        46, 245, 40, 244, 102, 143, 54, 65, 25, 63, 161, 1, 216, 80, 73, 209, 76, 132,
        187, 208, 89, 18, 169, 200, 196, 135, 130, 116, 188, 159, 86, 164, 100, 109,
        198, 173, 186, 3, 64, 52, 217, 226, 250, 124, 123, 5, 202, 38, 147, 118, 126,
        255, 82, 85, 212, 207, 206, 59, 227, 47, 16, 58, 17, 182, 189, 28, 42, 223, 183,
        170, 213, 119, 248, 152, 2, 44, 154, 163, 70, 221, 153, 101, 155, 167, 43,
        172, 9, 129, 22, 39, 253, 19, 98, 108, 110, 79, 113, 224, 232, 178, 185, 112,
        104, 218, 246, 97, 228, 251, 34, 242, 193, 238, 210, 144, 12, 191, 179, 162,
        241, 81, 51, 145, 235, 249, 14, 239, 107, 49, 192, 214, 31, 181, 199, 106,
        157, 184, 84, 204, 176, 115, 121, 50, 45, 127, 4, 150, 254, 138, 236, 205,
        93, 222, 114, 67, 29, 24, 72, 243, 141, 128, 195, 78, 66, 215, 61, 156, 180
    };
    private const int HASH_MASK = 255;
    private const int GRADIENTS_MASK_1D = 1;
    private const int GRADIENTS_MASK_2D = 7;
    private static Vector2[] gradients2D = {
        new Vector2(1f, 0f), // left
        new Vector2(-1f, 0f), // right
        new Vector2(0f, 1f), // up
        new Vector2(0f, -1f), // down

        new Vector2(1f, 1f).normalized,
        new Vector2(-1f, 1f).normalized,
        new Vector2(1f, 1f).normalized,
        new Vector2(-1f, -1f).normalized,
    };
    public static float Perlin(Vector3 point, float freq) // 1D -- only return the x in the point
    {
        //basic one-dimensional gradient function is g(x) = x

        point *= freq;
        int i0 = Mathf.FloorToInt(point.x);

        //interpolation
        float t0 = point.x - i0;

        float rightGradient = t0 - 1f;
        i0 &= HASH_MASK; // & operator is a bitwise discards everything except the lease significantly bit 

        int i1 = i0 + 1;

        //convert hash values int gradients
        int g0 = permutation[i0] & GRADIENTS_MASK_1D;
        int g1 = permutation[i1] & GRADIENTS_MASK_1D;

        //compute values
        float v0 = g0 * t0;
        float v1 = g1 * rightGradient;

        float t = Smooth(t0);

        return Mathf.Lerp(v0, v1, t) * 2f;
    }

    public static float Perlin2D(Vector3 point, float freq) // as the name implies 
    {
        // g(x, y) = ax + by
        point *= freq;
        int ix0 = Mathf.FloorToInt(point.x);
        int iy0 = Mathf.FloorToInt(point.y);

        float tx0 = point.x - ix0;
        float ty0 = point.y - iy0;

        float tx1 = tx0 - 1f;
        float ty1 = ty0 - 1f;

        ix0 &= HASH_MASK;
        iy0 &= HASH_MASK;
        
        int ix1 = ix0 + 1;
        int iy1 = iy0 + 1;

        int hash0 = permutation[ix0];
        int hash1 = permutation[ix1];

        //compute the hash values
        Vector2 h00 = gradients2D[permutation[hash0 + iy0] & GRADIENTS_MASK_2D];
        Vector2 h10 = gradients2D[permutation[hash1 + iy0] & GRADIENTS_MASK_2D];
        Vector2 h01 = gradients2D[permutation[hash0 + iy1] & GRADIENTS_MASK_2D];
        Vector2 h11 = gradients2D[permutation[hash1 + iy1] & GRADIENTS_MASK_2D];

        float v00 = Dot(h00, tx0, ty0);
        float v10 = Dot(h10, tx1, ty0);
        float v01 = Dot(h01, tx0, ty1);
        float v11 = Dot(h11, tx1, ty1);

        float tx = Smooth(tx0);
        float ty = Smooth(ty0);

        return Mathf.Lerp(Mathf.Lerp(v00, v10, tx), Mathf.Lerp(v01, v11, tx), ty) * Mathf.Sqrt(2f); // returns 1 or 0 
    }
    // the DOT Product function
    static float Dot(Vector2 g, float x, float y)=>g.x * x + g.y * y;

    public static float Smooth(float t)=>t * t * t * (t * (t * 6f - 15) + 10);

    public static float Layer2D(Vector3 point, float freq, int octaves){
        float sum = Perlin2D(point, freq);
        float amplitude = 1f;
        float range = 1f;

        for(int o = 1; o < octaves; o++){
            freq *= 2f;
            amplitude *= 0.5f;
            range += amplitude;
            sum += Perlin2D(point, freq) * amplitude;
        }

        return sum / range;
    }
}
