using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class AttachTextureFromStreamingAssetsWebGL : MonoBehaviour
{
    public string textureFileName = "heatmap.png"; // �e�N�X�`���t�@�C����
    [SerializeField] float transparency = 0.5f; // �����x�i0: ���S����, 1: �s�����j
    private string textureUrl;

    void Start()
    {
        // StreamingAssets�t�H���_���̃e�N�X�`���t�@�C����URL���擾
        textureUrl = System.IO.Path.Combine(Application.streamingAssetsPath, textureFileName);

        // �e�N�X�`�������݂��邩�񓯊��Ŋm�F
        StartCoroutine(CheckAndLoadTexture());
    }

    IEnumerator CheckAndLoadTexture()
    {
        using (UnityWebRequest www = UnityWebRequest.Head(textureUrl))
        {
            yield return www.SendWebRequest();

            if (!www.isHttpError && !www.isNetworkError)
            {
                // �e�N�X�`�������݂���ꍇ�̓��[�h
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

                // ���b�s���O���[�h�ƃt�B���^�[���[�h�̐ݒ�
                loadedTexture.wrapMode = TextureWrapMode.Clamp;
                loadedTexture.filterMode = FilterMode.Bilinear;

                Material loadedMaterial = new Material(Shader.Find("Standard"));
                loadedMaterial.mainTexture = loadedTexture;

                // �A���t�@�`�����l���𐳂��������ݒ�
                loadedMaterial.SetFloat("_Mode", 2); // �������[�h
                loadedMaterial.EnableKeyword("_ALPHABLEND_ON");
                loadedMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                loadedMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                loadedMaterial.SetInt("_ZWrite", 0);
                loadedMaterial.DisableKeyword("_ALPHATEST_ON");
                loadedMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                loadedMaterial.renderQueue = 3000;

                // �}�e���A���̓������[�h��ݒ�
                SetMaterialTransparency(loadedMaterial, transparency);

                // �I�u�W�F�N�g�Ƀ}�e���A����ݒ�
                GetComponent<Renderer>().material = loadedMaterial;
                Debug.Log("Texture attached: " + textureFileName);
            }
            else
            {
                Debug.LogError("Failed to load texture: " + www.error);
            }
        }
    }
    // �����x��ݒ肷��֐�
    void SetMaterialTransparency(Material material, float alpha)
    {
        // �}�e���A���̃����_�����O���[�h��Transparent�ɐݒ�
        material.SetFloat("_Mode", 3); // 3��Transparent���[�h���Ӗ����܂�
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;

        // �����x��ݒ�i�A���t�@�l�𒲐��j
        Color color = material.color;
        color.a = alpha; // �����x��ݒ�
        material.color = color;
    }

}
