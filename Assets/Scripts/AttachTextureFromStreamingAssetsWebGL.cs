using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class AttachTextureFromStreamingAssetsWebGL : MonoBehaviour
{
    public string textureFileName = "heatmap.png"; // テクスチャファイル名
    [SerializeField] float transparency = 0.5f; // 透明度（0: 完全透明, 1: 不透明）
    private string textureUrl;

    void Start()
    {
        // StreamingAssetsフォルダ内のテクスチャファイルのURLを取得
        textureUrl = System.IO.Path.Combine(Application.streamingAssetsPath, textureFileName);

        // テクスチャが存在するか非同期で確認
        StartCoroutine(CheckAndLoadTexture());
    }

    IEnumerator CheckAndLoadTexture()
    {
        using (UnityWebRequest www = UnityWebRequest.Head(textureUrl))
        {
            yield return www.SendWebRequest();

            if (!www.isHttpError && !www.isNetworkError)
            {
                // テクスチャが存在する場合はロード
                StartCoroutine(LoadTexture());
            }
            else
            {
                Debug.LogWarning("Texture not found or failed to load: " + www.error);
            }
        }
    }

    IEnumerator LoadTexture()
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(textureUrl))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture2D loadedTexture = DownloadHandlerTexture.GetContent(www);

                // ラッピングモードとフィルターモードの設定
                loadedTexture.wrapMode = TextureWrapMode.Clamp;
                loadedTexture.filterMode = FilterMode.Bilinear;

                Material loadedMaterial = new Material(Shader.Find("Standard"));
                loadedMaterial.mainTexture = loadedTexture;

                // アルファチャンネルを正しく扱う設定
                loadedMaterial.SetFloat("_Mode", 2); // 透明モード
                loadedMaterial.EnableKeyword("_ALPHABLEND_ON");
                loadedMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                loadedMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                loadedMaterial.SetInt("_ZWrite", 0);
                loadedMaterial.DisableKeyword("_ALPHATEST_ON");
                loadedMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                loadedMaterial.renderQueue = 3000;

                // マテリアルの透明モードを設定
                SetMaterialTransparency(loadedMaterial, transparency);

                // オブジェクトにマテリアルを設定
                GetComponent<Renderer>().material = loadedMaterial;
                Debug.Log("Texture attached: " + textureFileName);
            }
            else
            {
                Debug.LogError("Failed to load texture: " + www.error);
            }
        }
    }
    // 透明度を設定する関数
    void SetMaterialTransparency(Material material, float alpha)
    {
        // マテリアルのレンダリングモードをTransparentに設定
        material.SetFloat("_Mode", 3); // 3はTransparentモードを意味します
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;

        // 透明度を設定（アルファ値を調整）
        Color color = material.color;
        color.a = alpha; // 透明度を設定
        material.color = color;
    }

}
