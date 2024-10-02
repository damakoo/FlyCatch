using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class GenerateInitialParameter : MonoBehaviour
{
    public string csvFileName = "difficulty.csv"; // CSV�t�@�C����
    private string csvUrl;
    private List<DifficultyData> difficultyList = new List<DifficultyData>();
    [SerializeField] float standarddeviation = 0.5f;
    [SerializeField] BlackJackManager _blackJackManager;
    [SerializeField] Transform HostTransform;
    [SerializeField] Transform ClientTransform;
    public int gridResolution = 100;


    /*void Awake()
    {
        // StreamingAssets�t�H���_����CSV�t�@�C����URL���擾
        csvUrl = Path.Combine(Application.streamingAssetsPath, csvFileName);

        // CSV�t�@�C�������݂��邩�񓯊��Ŋm�F
        StartCoroutine(CheckAndLoadCSV());

    }*/
    private void Start()
    {
        CalculateDifficultyGrid();
        setfallenpoints();
    }

    public void setfallenpoints()
    {
        _blackJackManager.fallenpoints = new List<Vector3>();
        // ���������ƍł��߂�difficulty�l�̒T��
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
                // CSV�t�@�C�������݂���ꍇ�̓��[�h
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
                // �擾����CSV�f�[�^���e�L�X�g�Ƃ��ď���
                string csvData = www.downloadHandler.text;

                // CSV�f�[�^�̉�͏���
                ParseCSV(csvData);
                setfallenpoints();
            }
            else
            {
                Debug.LogError("Failed to load CSV file: " + www.error);
            }
        }
    }
    void CalculateDifficultyGrid()
    {
        // �O���b�h��������
        float[] gridX = GenerateGrid(HostTransform.position.x, ClientTransform.position.x, gridResolution);
        float[] gridY = GenerateGrid(HostTransform.position.z, ClientTransform.position.z, gridResolution);

        float minDifficulty = float.MaxValue;
        float maxDifficulty = float.MinValue;


        Vector2 Host2dPos = new Vector2(HostTransform.position.x, HostTransform.position.z);
        Vector2 Client2dPos = new Vector2(ClientTransform.position.x, ClientTransform.position.z);
        // �O���b�h��̊e�_�ɑ΂��Čv�Z
        for (int i = 0; i < gridX.Length; i++)
        {
            for (int j = 0; j < gridY.Length; j++)
            {
                Vector2 point = new Vector2(gridX[i], gridY[j]);

                // A�N��B�N�̓��B���Ԃ̌v�Z
                float aTime = Vector2.Distance(point, Host2dPos) / _blackJackManager.LeftAmountOfMove;
                float bTime = Vector2.Distance(point, Client2dPos) / _blackJackManager.RightAmountOfMove;

                // ���B���Ԃ̍������v�Z
                float difficulty = Mathf.Abs(aTime - bTime);

                // �f�[�^�����X�g�Ɋi�[
                difficultyList.Add(new DifficultyData(point.x, point.y, difficulty));

                // �ŏ��l�ƍő�l���g���b�N
                if (difficulty < minDifficulty) minDifficulty = difficulty;
                if (difficulty > maxDifficulty) maxDifficulty = difficulty;
            }
        }

        // ���K���imin-max�X�P�[�����O�j
        for (int i = 0; i < difficultyList.Count; i++)
        {
            DifficultyData data = difficultyList[i];
            float normalizedDifficulty = 1 - (data.difficulty - minDifficulty) / (maxDifficulty - minDifficulty);
            difficultyList[i] = new DifficultyData(data.x, data.y, normalizedDifficulty);
        }
    }

    float[] GenerateGrid(float start, float end, int resolution)
    {
        float[] grid = new float[resolution];
        float step = (end - start) / (resolution - 1);
        for (int i = 0; i < resolution; i++)
        {
            grid[i] = start + i * step;
        }
        return grid;
    }
// CSV�f�[�^����͂��郁�\�b�h
void ParseCSV(string csvData)
    {
        StringReader reader = new StringReader(csvData);
        bool isHeader = true;

        while (reader.Peek() != -1) // �t�@�C���̏I���܂Ń��[�v
        {
            string line = reader.ReadLine();
            if (isHeader)
            {
                // �w�b�_�[�s�̓X�L�b�v
                isHeader = false;
                continue;
            }

            string[] values = line.Split(','); // �J���}��؂�ŕ���

            // x, y, difficulty���i�[
            float x = float.Parse(values[0]);
            float y = float.Parse(values[1]);
            float difficulty = float.Parse(values[2]);

            // �f�[�^�����X�g�ɒǉ�
            difficultyList.Add(new DifficultyData(x, y, difficulty));
        }

        Debug.Log("CSV file successfully loaded and parsed.");
    }

    // �ł��߂�difficulty�����f�[�^��T�����\�b�h
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

    // �{�b�N�X���~�����[�@�Ő��K���z�����𐶐����A�Б��ɂ��郁�\�b�h
    float GenerateOneSidedNormalDistribution(float mean, float standardDeviation)
    {
        // �{�b�N�X���~�����[�@�Ő��K���z�̗����𐶐�
        float u1 = 1.0f - Random.value; // (0, 1] �̗���
        float u2 = 1.0f - Random.value; // (0, 1] �̗���
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);

        // ���K���z�̗����ɕW���΍��ƕ��ς�K�p
        float randNormal = mean + standardDeviation * randStdNormal;

        // �Б��ɂ���i���̏ꍇ�͐��ɂ���j
        return 1 - Mathf.Min(1,Mathf.Abs(randNormal));
    }

    // �f�[�^���i�[����N���X
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
