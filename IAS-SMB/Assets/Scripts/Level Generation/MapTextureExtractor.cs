using UnityEngine;

public static class MapTextureExtractor
{
    public static Color[,] GetTextureData(Texture2D texture, int widthOffset = 0, int heightOffset = 0)
    {
        return GetTextureData(texture, texture.width, texture.height, widthOffset, heightOffset);
    }

    public static Color[,] GetTextureData(Texture2D texture, int width, int height, int widthOffset = 0, int heightOffset = 0)
    {
        Color[,] result = new Color[width , height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                result[x, y] = texture.GetPixel(x,y);
            }
        }
        return result;
    }

    static Color[,] Get(Color color, int width, int height, int widthOffset = 0, int heightOffset = 0)
    {
        Color[,] result = new Color[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                result[x, y] = color;
            }
        }
        return result;
    }
}
