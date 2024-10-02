using System.Collections;
using System.Collections.Generic;
using Photon.Pun.Demo.Cockpit;
using UnityEngine;

public class Makeheatmap : MonoBehaviour
{
    // 補間に使用するパラメータ
    [SerializeField] float power = 2f;
    [SerializeField] int neighbors = 5;
    Texture2D heatmapTexture;
    MeshRenderer HeatmapRenderer;
    [SerializeField] GenerateInitialParameter _generateInitialParameter;
    [SerializeField] GameObject hostPlayer;
    [SerializeField] GameObject clientPlayer;
    Vector3 BottomLeft;
    Vector3 UpperRight;

    public int gridResolution => _generateInitialParameter.gridResolution;
    private float[,] HostApproachedRateDataList;
    private float[,] ClientApproachedRateDataList;
    private float[,] MatchingScoreDataList;
    // Start is called before the first frame update
    void Start()
    {
        HostApproachedRateDataList = new float[gridResolution, gridResolution];
        ClientApproachedRateDataList = new float[gridResolution, gridResolution];
        MatchingScoreDataList = new float[gridResolution, gridResolution];

        BottomLeft = new Vector3(Mathf.Min(hostPlayer.transform.position.x, clientPlayer.transform.position.x), 0, Mathf.Min(hostPlayer.transform.position.z, clientPlayer.transform.position.z));
        UpperRight = new Vector3(Mathf.Max(hostPlayer.transform.position.x, clientPlayer.transform.position.x), 0, Mathf.Max(hostPlayer.transform.position.z, clientPlayer.transform.position.z));

        heatmapTexture = new Texture2D(gridResolution, gridResolution);
                HeatmapRenderer = GetComponent<MeshRenderer>();
        for (int x = 0; x < gridResolution; x++)
        {
            for (int y = 0; y < gridResolution; y++)
            {
                Color color = new Color(0, 0, 0, 0);
                heatmapTexture.SetPixel(x, y, color);
            }
        }
        heatmapTexture.Apply(); // 変更を適用
        //HeatmapRenderer.material = new Material(HeatmapRenderer.material); // マテリアルをインスタンス化
        HeatmapRenderer.material.mainTexture = heatmapTexture;
    }

    public void MakeTexture(List<float> HostApproachedRateAll, List<float> ClientApproachedRateAll, List<Vector3> FallenPointAll)
    {
        heatmapTexture = new Texture2D(gridResolution, gridResolution);
        for (int i = 0; i < gridResolution; i++)
        {
            for (int j = 0; j < gridResolution; j++)
            {
                // グリッド上の点の座標 (x, z)
                float x = BottomLeft.x + (UpperRight.x - BottomLeft.x) * i / (float)(gridResolution - 1);
                float z = BottomLeft.z + (UpperRight.z - BottomLeft.z) * j / (float)(gridResolution - 1);

                // 補間を実行して結果を格納
                HostApproachedRateDataList[i, j] = InverseDistanceWeighted(x, z, HostApproachedRateAll, FallenPointAll);
                ClientApproachedRateDataList[i, j] = InverseDistanceWeighted(x, z, ClientApproachedRateAll, FallenPointAll);
                MatchingScoreDataList[i, j] = CalculateMatchingScore(HostApproachedRateDataList[i, j], ClientApproachedRateDataList[i, j]);
                //Debug.Log(HostApproachedRateDataList[i, j].ToString()+";" + ClientApproachedRateDataList[i, j].ToString()+";" +  MatchingScoreDataList[i, j].ToString());
                heatmapTexture.SetPixel(i, j, GetMatchingColor(MatchingScoreDataList[i, j]));
            }
        }
        heatmapTexture.Apply(); // 変更を適用
        HeatmapRenderer.material.mainTexture = heatmapTexture;

    }

    // 逆距離加重平均法による補間関数
    float InverseDistanceWeighted(float x, float z, List<float> HostApproachedRateAll, List<Vector3> FallenPointAll)
    {
        List<float> distances = new List<float>();
        List<float> weights = new List<float>();
        List<float> closestApproachedRates = new List<float>();

        // 各 FallenPointAll との距離を計算
        for (int i = 0; i < FallenPointAll.Count; i++)
        {
            float distance = Vector2.Distance(new Vector2(FallenPointAll[i].x, FallenPointAll[i].z), new Vector2(x, z));
            distances.Add(distance);
        }

        // 距離が近い順に並べ替える
        List<int> sortedIndices = GetSortedIndices(distances);

        // 近傍のデータ点を選択（neighbors 個）
        for (int i = 0; i < neighbors; i++)
        {
            int idx = sortedIndices[i];
            float distance = distances[idx];
            float weight = 1f / Mathf.Pow(distance, power);
            weights.Add(weight);
            closestApproachedRates.Add(HostApproachedRateAll[idx]);
        }

        // 重み付き平均を計算
        float weightedSum = 0f;
        float weightSum = 0f;
        for (int i = 0; i < neighbors; i++)
        {
            weightedSum += weights[i] * closestApproachedRates[i];
            weightSum += weights[i];
        }

        // 重み付き平均を返す
        float result = weightedSum / weightSum;
        return Mathf.Min(result, 1f);  // 1を超えないように制限
    }

    // 距離が小さい順にインデックスを取得する関数
    List<int> GetSortedIndices(List<float> distances)
    {
        List<int> indices = new List<int>();
        for (int i = 0; i < distances.Count; i++)
        {
            indices.Add(i);
        }
        indices.Sort((a, b) => distances[a].CompareTo(distances[b]));
        return indices;
    }

    float CalculateMatchingScore(float HostApproachedRate, float ClientApproachedRate)
    {
        float d_0 = Mathf.Sqrt(HostApproachedRate * HostApproachedRate + ClientApproachedRate * ClientApproachedRate);
        float d_1 = Mathf.Sqrt((HostApproachedRate - 1) * (HostApproachedRate - 1) + (ClientApproachedRate - 1) * (ClientApproachedRate - 1));

        return d_0 < d_1 ? d_0 - 1 : 1 - d_1;

    }

    // -1から1までのスコアに基づいて色を返す関数
    public Color GetMatchingColor(float score)
    {
        // Scoreが-1から1の範囲外ならクランプする
        score = Mathf.Clamp(score, -1f, 1f);

        // -1 から 0 の場合は青から緑
        if (score < 0)
        {
            return Color.Lerp(Color.blue, Color.green, (score + 1f) / 1f);
        }
        // 0 から 1 の場合は緑から赤を黄色を挟んで変化させる
        else
        {
            return Color.Lerp(Color.green, Color.Lerp(Color.yellow, Color.red, score), score);
        }
    }
}
