using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class GenerateInitialParameter : MonoBehaviour
{
    public string csvFileName = "difficulty.csv"; // CSVファイル名
    private string csvUrl;
    private List<DifficultyData> difficultyList = new List<DifficultyData>();
    [SerializeField] float standarddeviation = 0.5f;
    [SerializeField] BlackJackManager _blackJackManager;
    

    void Awake()
    {
        // StreamingAssetsフォルダ内のCSVファイルのURLを取得
        csvUrl = Path.Combine(Application.streamingAssetsPath, csvFileName);

        // CSVファイルが存在するか非同期で確認
        StartCoroutine(CheckAndLoadCSV());

    }

    public void setfallenpoints()
    {
        _blackJackManager.fallenpoints = new List<Vector3>();
        // 乱数生成と最も近いdifficulty値の探索
        for (int i = 0; i < 5; i++)
        {
            float randomValue = GenerateOneSidedNormalDistribution(0f, standarddeviation);
            DifficultyData closestData = FindClosestDifficulty(randomValue);
            _blackJackManager.fallenpoints.Add(new Vector3(closestData.x, 0, closestData.y));
        }
        _blackJackManager.issetfallenpoints = true;
    }

    IEnumerator CheckAndLoadCSV()
    {
        using (UnityWebRequest www = UnityWebRequest.Head(csvUrl))
        {
            yield return www.SendWebRequest();

            if (!www.isHttpError && !www.isNetworkError)
            {
                // CSVファイルが存在する場合はロード
                StartCoroutine(LoadCSV());
            }
            else
            {
                Debug.LogWarning("CSV file not found or failed to load: " + www.error);
            }
        }
    }

    IEnumerator LoadCSV()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(csvUrl))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                // 取得したCSVデータをテキストとして処理
                string csvData = www.downloadHandler.text;

                // CSVデータの解析処理
                ParseCSV(csvData);
                setfallenpoints();
            }
            else
            {
                Debug.LogError("Failed to load CSV file: " + www.error);
            }
        }
    }

    // CSVデータを解析するメソッド
    void ParseCSV(string csvData)
    {
        StringReader reader = new StringReader(csvData);
        bool isHeader = true;

        while (reader.Peek() != -1) // ファイルの終わりまでループ
        {
            string line = reader.ReadLine();
            if (isHeader)
            {
                // ヘッダー行はスキップ
                isHeader = false;
                continue;
            }

            string[] values = line.Split(','); // カンマ区切りで分割

            // x, y, difficultyを格納
            float x = float.Parse(values[0]);
            float y = float.Parse(values[1]);
            float difficulty = float.Parse(values[2]);

            // データをリストに追加
            difficultyList.Add(new DifficultyData(x, y, difficulty));
        }

        Debug.Log("CSV file successfully loaded and parsed.");
    }

    // 最も近いdifficultyを持つデータを探すメソッド
    DifficultyData FindClosestDifficulty(float targetValue)
    {
        DifficultyData closest = null;
        float minDifference = Mathf.Infinity;

        foreach (DifficultyData data in difficultyList)
        {
            float difference = Mathf.Abs(data.difficulty - targetValue);
            if (difference < minDifference)
            {
                minDifference = difference;
                closest = data;
            }
        }

        return closest;
    }

    // ボックス＝ミュラー法で正規分布乱数を生成し、片側にするメソッド
    float GenerateOneSidedNormalDistribution(float mean, float standardDeviation)
    {
        // ボックス＝ミュラー法で正規分布の乱数を生成
        float u1 = 1.0f - Random.value; // (0, 1] の乱数
        float u2 = 1.0f - Random.value; // (0, 1] の乱数
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);

        // 正規分布の乱数に標準偏差と平均を適用
        float randNormal = mean + standardDeviation * randStdNormal;

        // 片側にする（負の場合は正にする）
        return 1 - Mathf.Min(1,Mathf.Abs(randNormal));
    }

    // データを格納するクラス
    class DifficultyData
    {
        public float x;
        public float y;
        public float difficulty;

        public DifficultyData(float x, float y, float difficulty)
        {
            this.x = x;
            this.y = y;
            this.difficulty = difficulty;
        }
    }
}
