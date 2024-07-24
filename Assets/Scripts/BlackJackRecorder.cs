using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using TMPro;

public class BlackJackRecorder : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void DownloadFile(string filename, string content);

    [SerializeField] BlackJackManager _BlackJackManager;
    private PracticeSet _PracticeSet => _BlackJackManager._PracticeSet;
    //[SerializeField] CSVWriter _CSVWriter;
    public List<int> MyNumberList { get; set; } = new List<int>();
    public List<int> YourNumberList { get; set; } = new List<int>();
    public List<int> MySelectedBetList { get; set; } = new List<int>();
    public List<int> YourSelectedBetList { get; set; } = new List<int>();
    public List<int> ScoreList => _BlackJackManager.ScoreList;
    private List<List<float>> MyCardsPracticeList => _PracticeSet.MyCardsPracticeList;
    private List<List<float>> YourCardsPracticeList => _PracticeSet.YourCardsPracticeList;
    private List<List<float>> FieldCardsPracticeList => _PracticeSet.FieldCardsPracticeList;
    private int TrialAll => _PracticeSet.TrialAll;
    private List<float> MySelectedTime => _PracticeSet.MySelectedTime;
    private List<float> YourSelectedTime => _PracticeSet.YourSelectedTime;
    public int Trial = 1;


    private string _Title;
    private void Start()
    {
        _Title = "Day" + System.DateTime.Now.Day.ToString() + "_" + System.DateTime.Now.Hour.ToString() + "h_" + System.DateTime.Now.Minute.ToString() + "min_" + System.DateTime.Now.Second.ToString() + "sec";
    }
    string WriteContent()
    {
        string Content = "";
        for (int i = 0; i < FieldCardsPracticeList[0].Count; i++) Content += ",FieldCards" + (i + 1).ToString();
        for (int i = 0; i < MyCardsPracticeList[0].Count; i++) Content += ",MyCards" + (i + 1).ToString();
        for (int i = 0; i < YourCardsPracticeList[0].Count; i++) Content += ",YourCards" + (i + 1).ToString();
        Content += "MySelectedTime,YourSelectedTime,Score,Trial\n";
        for (int i = 0; i < TrialAll; i++)
        {
            Content = "";
            for (int j = 0; j < FieldCardsPracticeList[i].Count; j++) Content += "," + FieldCardsPracticeList[i][j].ToString();
            for (int j = 0; j < MyCardsPracticeList[i].Count; j++) Content += "," + MyCardsPracticeList[i][j].ToString();
            for (int j = 0; j < YourCardsPracticeList[i].Count; j++) Content += "," + YourCardsPracticeList[i][j].ToString();
            Content += "," + MySelectedTime[i].ToString() + "," + YourSelectedTime[i].ToString() + "," + ScoreList[i].ToString() + "," + Trial.ToString() + "\n";
        }
        return Content;
    }
    public void ExportCsv()
    {
        DownloadFile("result_flycatch_" + _Title + "_" + Trial.ToString() + ".csv", WriteContent());
    }

    /*public void WriteResult()
    {
        string Content = "";
        Content += "FieldNumber";
        for (int i = 0; i < MyCardsPracticeList[0].Count; i++) Content += ",MyCards" + (i + 1).ToString();
        for (int i = 0; i < YourCardsPracticeList[0].Count; i++) Content += ",YourCards" + (i + 1).ToString();
        Content += ",MyNumber,YourNumber,Score\n";
        for(int i = 0;i < TrialAll; i++)
        {
            Content += FieldCardsPracticeList[i].ToString();
            for (int j = 0; j < MyCardsPracticeList[i].Count; j++) Content += "," + MyCardsPracticeList[i][j].ToString();
            for (int j = 0; j < YourCardsPracticeList[i].Count; j++) Content += "," + YourCardsPracticeList[i][j].ToString();
            Content += "," + MyNumberList[i].ToString() + "," + YourNumberList[i].ToString() + "," + ScoreList[i].ToString() + "\n";
        }
        _CSVWriter.WriteCSV(Content);
    }*/
    public void Initialize()
    {
        MyNumberList = new List<int>();
        YourNumberList = new List<int>();
        MySelectedBetList = new List<int>();
        YourSelectedBetList = new List<int>();
        //ScoreList = new List<int>();
    }
}
