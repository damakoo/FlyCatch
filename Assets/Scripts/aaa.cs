using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aaa : MonoBehaviour
{
    Texture2D heatmapTexture;
    MeshRenderer planeRenderer;
    // Start is called before the first frame update
    void Start()
    {
        heatmapTexture = new Texture2D(1000, 1000);
        planeRenderer = GetComponent<MeshRenderer>();
        for (int x = 0; x < 1000; x++)
        {
            for (int y = 0; y < 1000; y++)
            {
                Color color = Color.red;
                if (x % 5 == 0)
                {
                    color = Color.yellow;
                }
                else if (x % 5 == 1)
                {
                    color = Color.blue;
                }
                else if (x % 5 == 2)
                {
                    color = Color.green;
                }
                else if (x % 5 == 3)
                {
                    color = Color.white;
                }
                else if (x % 5 == 4)
                {
                    color = Color.gray;
                }
                color = new Color(1,0,0,1);
                heatmapTexture.SetPixel(x, y, color);
            }
        }
        heatmapTexture.Apply(); // •ÏX‚ð“K—p
        planeRenderer.material.mainTexture = heatmapTexture;

    }
}
