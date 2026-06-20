using UnityEngine;
using System.IO;

// reference: LKHGames Shader Graph Exposed Gradient https://www.youtube.com/watch?v=K3dPvA2MtTY

public class GradientGenerator : MonoBehaviour
{
    public Gradient gradient;
    public string savingPath = "/GradientTextureGenerator/GeneratedTexture/";

    public float width = 256;
    public float height = 64;

    private Texture2D gradientTexture;
    private Texture2D tempTexture;

    public bool isHorizontal = true;

    Texture2D GenerateGradientTexture(Gradient grad)
    {
        if (tempTexture == null) // create temporary texture if it doesn't exist
        {
            tempTexture = new Texture2D((int)width, (int)height);
        }
        for (int x= 0; x < width; x++) // iterate through pixels of x
        {
            for (int y = 0; y < height; y++) // iterate through pixels of y
            {
                float t = isHorizontal ? (float)x / (width - 1) : (float)y / (height - 1);
                Color color = grad.Evaluate(t);
                tempTexture.SetPixel(x, y, color);  
            }
        }

        tempTexture.wrapMode = TextureWrapMode.Clamp;
        tempTexture.Apply();
        return tempTexture;
    }

    public void BakeGradientTexture()
    {
        gradientTexture = GenerateGradientTexture(gradient);
        byte[] bytes = gradientTexture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + savingPath + "GradientTexture_" + Random.Range(0, 999999) + ".png", bytes);
        Debug.Log("Gradient texture saved to: " + Application.dataPath + savingPath + "GradientTexture.png");
    }
}
