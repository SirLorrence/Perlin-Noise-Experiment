# Perlin Noise Project

This was a interesting start to exploring what gradient noise is and what was Perlin noise. First I started with the simple way unity has it implemented with there own method in Mathf: 

```csharp
var sample = Mathf.PerlinNoise(xCoord, yCoord);
```

I was able to get the results I wanted. Being that each point on my size grid will pull a point from my Perlin 2d texture unity progenerates to determines the height  of where the object is create and placed.

![Cube Map with Unity Perlin](Perlin_Noise_Project_README/Perlin-Cube-Grid.png)

## Then I created my own...

Lots of research and looking at peoples code created this. I used Ken Perlin's original permutation array. 

```csharp
static int[] permutation =
    {
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
		}
```

Then I had to semi-teach myself what the dot product is and interpolation. (Which I need to go back over). To keep it short, there's a lot of math involved in this [algorithm](https://en.wikipedia.org/wiki/Perlin_noise) . 

### After a got the Noise code down, I created a test texture to see if everything is work.

![Perlin Texture](Perlin_Noise_Project_README/Perlin-Texture.png)

2D Perlin Texture

![Perlin Texture with Octaves applied](Perlin_Noise_Project_README/Perlin-Texture-Octaves.png)

2D Perlin Texture with Octaves added

### Then I applied to the a mesh grid and added at custom shader I found.

![Final Perlin mesh map](Perlin_Noise_Project_README/Perlin-Mesh.png)
