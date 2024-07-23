using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class BlackJackManager : MonoBehaviour
{
    [SerializeField] bool useSuit = false;
    [SerializeField] CardsList _cardslist;
    [SerializeField] int TimeLimit;
    [SerializeField] int ShowMyCardsTime = 5;
    [SerializeField] int ResultsTime = 5;
    [SerializeField] int WaitingTime = 3;
    [SerializeField] int BetTime = 4;
    [SerializeField] int NumberofSet = 10;
    [SerializeField] TextMeshProUGUI FinishUI;
    [SerializeField] BlackJackRecorder _blackJackRecorder;
    [SerializeField] TextMeshProUGUI MyScoreUI;
    [SerializeField] GameObject ClientUi;
    [SerializeField] GameObject BetUi;
    [SerializeField] GameObject CardListObject;
    [SerializeField] DecideHostorClient _decideHostorClient;
    [SerializeField] GameObject StartingUi;
    [SerializeField] GameObject StartingUi_button;
    [SerializeField] GameObject WaitforStartUi;
    [SerializeField] GameObject _SceneReloaderHost;
    [SerializeField] GameObject _SceneReloaderClient;
    [SerializeField] List<TextMeshProUGUI> BetUiChild;
    [SerializeField] GameObject TimeLimitObj;
    [SerializeField] GameObject TimeLimit_Bet;
    [SerializeField] GameObject TimeLimit_notBet;
    [SerializeField] GameObject AllTrialFinishedUI;
    [SerializeField] TextMeshProUGUI TimeLimitObj_str;
    [SerializeField] GameObject Ball;
    [SerializeField] Rigidbody Ball_rididbody;
    [SerializeField] GameObject HostPlayer;
    [SerializeField] GameObject ClientPlayer;
    [SerializeField] float AmountOfMove = 0.01f;
    Animator HostPlayerAnimator;
    Animator ClientPlayerAnimator;
    Vector3 fallpoint;
    float distance_host;
    float distance_client;
    //[SerializeField] TextMeshProUGUI YourScoreUI;
    public PracticeSet _PracticeSet { get; set; }
    private List<int> MaxScoreList = new List<int>();
    private List<int> ScoreList = new List<int>();
    private int NOTSELCETEDNUMBER = 101;

    public enum HostorClient
    {
        Host = 0,
        Client = 1
    }
    public HostorClient _hostorclient { get; set; }
    private enum HowShowCard
    {
        KeyBoard,
        Time
    }
    [SerializeField] HowShowCard _HowShowCard;
    int nowTrial = 0;
    float nowTime = 0;
    private int Score = 0;
    public bool hasPracticeSet { get; set; } = false;
    // Start is called before the first frame update
    void Start()
    {
        FinishUI.text = "";
        TimeLimitObj_str.text = "";
        HostPlayerAnimator = HostPlayer.GetComponent<Animator>();
        ClientPlayerAnimator = ClientPlayer.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasPracticeSet)
        {
            if (_hostorclient == HostorClient.Host)
            {
                if (_PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.BeforeStart)
                {
                    StartingGame();
                    if (_PracticeSet.HostPressed && _PracticeSet.ClientPressed)
                    {
                        PhotonMoveToWaitForNextTrial(nowTrial);
                        _PracticeSet.SetHostPressed(false);
                        _PracticeSet.SetClientPressed(false);
                    }
                    //if (Input.GetKeyDown(KeyCode.Space)) PhotonMoveToWaitForNextTrial(nowTrial);
                }
                else if (_PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.WaitForNextTrial)
                {
                    //if (Input.GetKeyDown(KeyCode.Space)) MoveToShowMyCards();
                    nowTime += Time.deltaTime;
                    _PracticeSet.SetTimeLeft(WaitingTime - nowTime);
                    if (nowTime > WaitingTime)
                    {
                        nowTime = 0;
                        PhotonMoveToShowMyCards();
                    }
                }
                else if (_PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.ShowMyCards)
                {
                    if (_HowShowCard == HowShowCard.KeyBoard)
                    {
                        if (Input.GetKeyDown(KeyCode.Space)) PhotonMoveToSelectCards();
                    }
                    else if (_HowShowCard == HowShowCard.Time)
                    {
                        nowTime += Time.deltaTime;
                        _PracticeSet.SetTimeLeft(ShowMyCardsTime - nowTime);
                        if (nowTime > ShowMyCardsTime)
                        {
                            nowTime = 0;
                            PhotonMoveToSelectCards();
                        }
                    }

                }
                else if (_PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.SelectCards)
                {
                    nowTime += Time.deltaTime;
                    _PracticeSet.SetTimeLeft(TimeLimit - nowTime);
                    BlackJacking();
                    if (nowTime > TimeLimit) PhotonMoveToSelectBet();
                }
                else if (_PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.SelectBet)
                {
                    nowTime += Time.deltaTime;
                    _PracticeSet.SetTimeLeft(BetTime - nowTime);
                    SelectBetting();
                    if (nowTime > BetTime) PhotonMoveToShowResult();
                }
                else if (_PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.ShowResult)
                {
                    //if (Input.GetKeyDown(KeyCode.Space)) MoveToWaitForNextTrial();
                    nowTime += Time.deltaTime;
                    _PracticeSet.SetTimeLeft(ResultsTime - nowTime);
                    if (nowTime > ResultsTime)
                    {
                        nowTime = 0;
                        PhotonMoveToWaitForNextTrial(nowTrial);
                    }
                }
                else if (_PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.Finished)
                {
                    if (_PracticeSet.HostPressed && _PracticeSet.ClientPressed)
                    {
                        PhotonRestart();
                    }
                }

            }
            else if (_hostorclient == HostorClient.Client && _PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.BeforeStart)
            {
                StartingGame();
            }
            else if (_hostorclient == HostorClient.Client && _PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.SelectCards)
            {
                nowTime += Time.deltaTime;
                BlackJacking();
            }
            else if (_hostorclient == HostorClient.Client && _PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.SelectBet)
            {
                SelectBetting();
            }
            else if (_hostorclient == HostorClient.Client)
            {
                nowTime = 0;
            }

            if (_PracticeSet.BlackJackState != PracticeSet.BlackJackStateList.BeforeStart) TimeLimitObj_str.text = "Time: " + Mathf.CeilToInt(_PracticeSet.TimeLeft).ToString();

            if (Input.GetKeyDown(KeyCode.F))
            {
                _PracticeSet.SetHostPlayerRunning(true);
            }
            else if (Input.GetKeyUp(KeyCode.F))
            {
                _PracticeSet.SetHostPlayerRunning(false);
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                _PracticeSet.SetClientPlayerRunning(true);
            }
            else if (Input.GetKeyUp(KeyCode.G))
            {
                _PracticeSet.SetClientPlayerRunning(false);
            }
        }
    }
    public void SetPracticeSet(PracticeSet _practiceset)
    {
        _PracticeSet = _practiceset;
        _cardslist.SetPracticeSet(_practiceset);
        hasPracticeSet = true;
    }


    public void UpdateParameter()
    {
        _PracticeSet.UpdateParameter();
    }
    public void ReUpdateParameter()
    {
        _PracticeSet.ReUpdateParameter();
    }
    public void InitializeCard()
    {
        _cardslist.InitializeCards();
    }
    public void ReInitializeCard()
    {
        _cardslist.ReInitializeCards();
    }
    void BlackJacking()
    {
        distance_host = Vector3.Magnitude(fallpoint - HostPlayer.transform.position);
        distance_client = Vector3.Magnitude(fallpoint - ClientPlayer.transform.position);
        // �}�E�X�{�^�����N���b�N���ꂽ���m�F
        if (Input.GetKey(KeyCode.F))
        {
            _PracticeSet.SetMySelectedTime(nowTime, nowTrial);
            if (distance_host < AmountOfMove)
            {
                _PracticeSet.SetHostPlayerPos(HostPlayer.transform.position + (fallpoint - HostPlayer.transform.position) / distance_host * AmountOfMove);
            }
        }

        if (Input.GetKey(KeyCode.J))
        {
            _PracticeSet.SetYourSelectedTime(nowTime, nowTrial);
            if (distance_client < AmountOfMove)
            {
                _PracticeSet.SetClientPlayerPos(ClientPlayer.transform.position + (fallpoint - ClientPlayer.transform.position) / distance_client * AmountOfMove);
            }
        }
        HostPlayer.transform.position = _PracticeSet.HostPlayerPos;
        ClientPlayer.transform.position = _PracticeSet.ClientPlayerPos;
        HostPlayerAnimator.SetBool("Running", _PracticeSet.HostPlayerRunning);
        ClientPlayerAnimator.SetBool("Running", _PracticeSet.ClientPlayerRunning);
    }
    void SelectBetting()
    {
        // �}�E�X�{�^�����N���b�N���ꂽ���m�F
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 rayPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero);

            // ���C�L���X�g���g�p���ăI�u�W�F�N�g�����o
            if (hit && hit.collider.gameObject.CompareTag("Bet"))
            {
                TextMeshProUGUI textMesh;
                if (hit.collider.gameObject.TryGetComponent<TextMeshProUGUI>(out textMesh))
                {
                    string text = textMesh.text;

                    // 文字列から数字を抽出してint型に変換
                    int number;
                    if (int.TryParse(text, out number))
                    {
                        foreach (TextMeshProUGUI child in BetUiChild) child.color = Color.white;
                        textMesh.color = Color.yellow;
                        if (_hostorclient == HostorClient.Host)
                        {
                            _PracticeSet.SetMySelectedBet(number);
                        }
                        else if (_hostorclient == HostorClient.Client)
                        {
                            _PracticeSet.SetYourSelectedBet(number);
                        }
                    }
                    else
                    {
                        Debug.LogError("文字列に数字が含まれていません。");
                    }
                }
                else
                {
                    Debug.LogError("TextMeshProUGUIコンポーネントが見つかりませんでした。");
                }
            }
        }
    }
    public void GameStartUI()
    {
        StartingUi.SetActive(true);
        StartingUi_button.SetActive(true);
    }
    public void PhotonGameStartUI()
    {
        _PracticeSet.GameStartUi();
    }
    void StartingGame()
    {
        // �}�E�X�{�^�����N���b�N���ꂽ���m�F
        /*if (Input.GetMouseButtonDown(0))
        {
            Vector2 rayPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero);

            // ���C�L���X�g���g�p���ăI�u�W�F�N�g�����o
            if (hit && hit.collider.gameObject.CompareTag("Start"))
            {
                if (_hostorclient == HostorClient.Host)
                {
                    _PracticeSet.SetHostPressed(true);
                }
                else if (_hostorclient == HostorClient.Client)
                {
                    _PracticeSet.SetClientPressed(true);
                }
                WaitforStartUi.SetActive(true);
                StartingUi.SetActive(false);
                StartingUi_button.SetActive(false);
            }
        }*/
        if (Input.GetKeyDown(KeyCode.Space))
        {

                if (_hostorclient == HostorClient.Host)
                {
                    _PracticeSet.SetHostPressed(true);
                }
                else if (_hostorclient == HostorClient.Client)
                {
                    _PracticeSet.SetClientPressed(true);
                }
                WaitforStartUi.SetActive(true);
                StartingUi.SetActive(false);
                StartingUi_button.SetActive(false);
        }
    }

    public void MoveToShowMyCards(int hostorClient)
    {
        /*if (hostorClient == (int)HostorClient.Host)
        {
            _cardslist.MyCardsOpen();
        }
        else if (hostorClient == (int)HostorClient.Client)
        {
            _cardslist.YourCardsOpen();
        }*/
        _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.ShowMyCards;
        TimeLimitObj.transform.position = TimeLimit_notBet.transform.position;
    }
    public void PhotonMoveToShowMyCards()
    {
        _PracticeSet.MoveToShowMyCards((int)_hostorclient);
    }
    public void PhotonMoveToSelectBet()
    {
        _PracticeSet.MoveToSelectBet();
    }
    public void MoveToSelectBet()
    {
        HostPlayerAnimator.SetBool("Running", false);
        ClientPlayerAnimator.SetBool("Running", false);
        _PracticeSet.MySelectedBet = 0;
        _PracticeSet.YourSelectedBet = 0;
        //CardListObject.SetActive(false);
        //BetUi.SetActive(true);
        //foreach (TextMeshProUGUI child in BetUiChild) child.color = Color.white;
        nowTime = 0;
        _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.SelectBet;
        TimeLimitObj.transform.position = TimeLimit_Bet.transform.position;
    }
    public void MoveToSelectCards()
    {
        //_cardslist.AllOpen();
        Ball.transform.position = new Vector3(_PracticeSet.FieldCardsPracticeList[nowTrial][0], _PracticeSet.FieldCardsPracticeList[nowTrial][1], _PracticeSet.FieldCardsPracticeList[nowTrial][2]);
        Ball_rididbody.AddForce(new Vector3(_PracticeSet.FieldCardsPracticeList[nowTrial][3], _PracticeSet.FieldCardsPracticeList[nowTrial][4], _PracticeSet.FieldCardsPracticeList[nowTrial][5]));
        Ball_rididbody.AddTorque(new Vector3(_PracticeSet.FieldCardsPracticeList[nowTrial][6], _PracticeSet.FieldCardsPracticeList[nowTrial][7], _PracticeSet.FieldCardsPracticeList[nowTrial][8]));
        fallpoint = new Vector3(_PracticeSet.FieldCardsPracticeList[nowTrial][9], _PracticeSet.FieldCardsPracticeList[nowTrial][10], _PracticeSet.FieldCardsPracticeList[nowTrial][11]);
        _PracticeSet.SetHostPlayerPos(new Vector3(_PracticeSet.MyCardsPracticeList[nowTrial][0], _PracticeSet.MyCardsPracticeList[nowTrial][1], _PracticeSet.MyCardsPracticeList[nowTrial][2]));
        _PracticeSet.SetClientPlayerPos(new Vector3(_PracticeSet.YourCardsPracticeList[nowTrial][0], _PracticeSet.YourCardsPracticeList[nowTrial][1], _PracticeSet.YourCardsPracticeList[nowTrial][2]));
        _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.SelectCards;
        TimeLimitObj.transform.position = TimeLimit_notBet.transform.position;
    }
    public void PhotonMoveToSelectCards()
    {
        _PracticeSet.MoveToSelectCards();
    }
    public void MoveToShowResult()
    {
        //CardListObject.SetActive(true);
        //BetUi.SetActive(false);
        //if (_PracticeSet.MySelectedCard != NOTSELCETEDNUMBER) _cardslist.MyCardsList[_PracticeSet.MySelectedCard].Clicked();
        //if (_PracticeSet.YourSelectedCard != NOTSELCETEDNUMBER) _cardslist.YourCardsList[_PracticeSet.YourSelectedCard].Clicked();
        TimeLimitObj.transform.position = TimeLimit_notBet.transform.position;

        /*foreach (var card in _cardslist.MyCardsList_opponent)
        {
            if (card.Number == _PracticeSet.YourSelectedCard.Number) card.Clicked();
        }
        foreach (var card in _cardslist.YourCardsList_opponent)
        {
            if (card.Number == _PracticeSet.MySelectedCard.Number) card.Clicked();
        }*/
        Score = CalculateResult();
        //_blackJackRecorder.RecordResult((_PracticeSet.MySelectedCard == NOTSELCETEDNUMBER) ? 0 : _cardslist.MyCardsList[_PracticeSet.MySelectedCard].Number, (_PracticeSet.YourSelectedCard == NOTSELCETEDNUMBER) ? 0 : _cardslist.YourCardsList[_PracticeSet.YourSelectedCard].Number, (useSuit) ? CalculateSuitScore() : Score, _PracticeSet.MySelectedBet, _PracticeSet.YourSelectedBet);
        _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.ShowResult;
        MyScoreUI.text = "Score:" + ((useSuit) ? CalculateScorewithSuit() : Score.ToString());
        ScoreList.Add(Score);
        if (useSuit)
        {
            RecordMaxSuitScore();
        }
        else
        {
            RecordMaxScore();
        }
        //YourScoreUI.text = Score.ToString();
        nowTime = 0;
        nowTrial += 1;
        if (nowTrial == _PracticeSet.TrialAll)
        {
            _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.Finished;
            FinishUI.text = "Finished! \n ScoreAll:" + ReturnSum(ScoreList).ToString() + "/" + ReturnSum(MaxScoreList).ToString() + "\n" + "Trial: " + _blackJackRecorder.Trial.ToString() + "/" + NumberofSet.ToString();
            //_blackJackRecorder.WriteResult();
            _blackJackRecorder.ExportCsv();
            if (_blackJackRecorder.Trial == NumberofSet)
            {
                AllTrialFinishedUI.SetActive(true);
            }
            else
            {
                _SceneReloaderHost.SetActive(true);
            }

            TimeLimitObj_str.text = "";
        }
    }
    public void PhotonMoveToShowResult()
    {
        _PracticeSet.MoveToShowResult();
    }
    public void MoveToWaitForNextTrial(int _nowTrial)
    {
        WaitforStartUi.SetActive(false);
        //_cardslist.AllClose();
        _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.WaitForNextTrial;
        nowTrial = _nowTrial;
        //_cardslist.SetCards(_nowTrial);
        MyScoreUI.text = "";
        //YourScoreUI.text = "";
        //_PracticeSet.MySelectedCard = NOTSELCETEDNUMBER;
        //_PracticeSet.YourSelectedCard = NOTSELCETEDNUMBER;
        SetClientUI(false);
        TimeLimitObj.transform.position = TimeLimit_notBet.transform.position;
    }
    public void PhotonMoveToWaitForNextTrial(int _nowTrial)
    {
        _PracticeSet.MoveToWaitForNextTrial(_nowTrial);
    }
    private int CalculateResult()
    {
        return (distance_host < AmountOfMove * 2 || distance_host < AmountOfMove * 2) ? 1 : 0;
    }
    public void MakeReadyHost()
    {
        _decideHostorClient.HostReady = true;
    }
    public void MakeReadyClient()
    {
        _decideHostorClient.ClientReady = true;
    }
    public void PhotonMakeReadyHost()
    {
        _PracticeSet.MakeReadyHost();
    }
    public void PhotonMakeReadyClient()
    {
        _PracticeSet.MakeReadyClient();
    }
    private string CalculateScorewithSuit()
    {
        string result = Score.ToString();
        if (_cardslist.MyCardsList[_PracticeSet.MySelectedCard].suit.GetColor() == _cardslist.YourCardsList[_PracticeSet.YourSelectedCard].suit.GetColor())
        {
            if (_cardslist.MyCardsList[_PracticeSet.MySelectedCard].suit == _cardslist.YourCardsList[_PracticeSet.YourSelectedCard].suit)
            {
                result += " x 1.2 = " + Mathf.Ceil(Score * 1.2f).ToString();
            }
            else
            {
                result += " x 1.1 = " + Mathf.Ceil(Score * 1.1f).ToString();
            }
        }
        else
        {
            result += " x 1.0 = " + Score.ToString();
        }
        return result;
    }
    private int CalculateSuitScore()
    {
        if (_cardslist.MyCardsList[_PracticeSet.MySelectedCard].suit.GetColor() == _cardslist.YourCardsList[_PracticeSet.YourSelectedCard].suit.GetColor())
        {
            if (_cardslist.MyCardsList[_PracticeSet.MySelectedCard].suit == _cardslist.YourCardsList[_PracticeSet.YourSelectedCard].suit)
            {
                return (int)Mathf.Ceil(Score * 1.2f);
            }
            else
            {
                return (int)Mathf.Ceil(Score * 1.1f);
            }
        }
        else
        {
            return Score;
        }
    }
    private void RecordMaxScore()
    {
        /*int MaxScore = 0;
        for (int i = 0; i < _cardslist.MyCardsList.Count; i++)
        {
            for (int j = 0; j < _cardslist.YourCardsList.Count; j++)
            {
                int _score = (_cardslist.MyCardsList[i].Number + _cardslist.YourCardsList[j].Number + _PracticeSet.FieldCardsPracticeList[nowTrial][0] > 21) ? 0 : _cardslist.MyCardsList[i].Number + _cardslist.YourCardsList[j].Number + _PracticeSet.FieldCardsPracticeList[nowTrial][0];
                if (MaxScore < _score) MaxScore = _score;
            }
        }
        MaxScoreList.Add(MaxScore);*/
    }
    private void RecordMaxSuitScore()
    {
       /* int MaxScore = 0;
        for (int i = 0; i < _cardslist.MyCardsList.Count; i++)
        {
            for (int j = 0; j < _cardslist.YourCardsList.Count; j++)
            {
                int _score = (_cardslist.MyCardsList[i].Number + _cardslist.YourCardsList[j].Number + _PracticeSet.FieldCardsPracticeList[nowTrial][0] > 21) ? 0 : _cardslist.MyCardsList[i].Number + _cardslist.YourCardsList[j].Number + _PracticeSet.FieldCardsPracticeList[nowTrial][0];
                if (_cardslist.MyCardsList[i].suit.GetColor() == _cardslist.YourCardsList[j].suit.GetColor())
                {
                    if (_cardslist.MyCardsList[i].suit == _cardslist.YourCardsList[j].suit)
                    {
                        _score = (int)Mathf.Ceil(_score * 1.2f);
                    }
                    else
                    {
                        _score = (int)Mathf.Ceil(Score * 1.1f);
                    }
                }
                if (MaxScore < _score) MaxScore = _score;
            }
        }
        MaxScoreList.Add(MaxScore);*/
    }
    private int ReturnSum(List<int> _list)
    {
        int result = 0;
        foreach (var element in _list)
        {
            result += element;
        }
        return result;
    }
    public void SetClientUI(bool setactive)
    {
        ClientUi.SetActive(setactive);
    }
    public void PhotonRestart()
    {
        _PracticeSet.SetHostPressed(false);
        _PracticeSet.SetClientPressed(false);
        ReUpdateParameter();
        _PracticeSet.Restart();
    }
    public void Restart()
    {
        TimeLimitObj_str.text = "";
        MaxScoreList = new List<int>();
        _SceneReloaderClient.SetActive(false);
        _blackJackRecorder.Trial += 1;
        FinishUI.text = "";
        //_cardslist.AllClose();
        ScoreList = new List<int>();
        nowTrial = 0;
        nowTime = 0;
        _blackJackRecorder.Initialize();
        _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.BeforeStart;
        MyScoreUI.text = "";
        PhotonGameStartUI();
    }
    public void PressedReload()
    {
        _SceneReloaderHost.SetActive(false);
        if (_hostorclient == HostorClient.Host)
        {
            _PracticeSet.SetHostPressed(true);
            _SceneReloaderClient.SetActive(true);
        }
        else if (_hostorclient == HostorClient.Client)
        {
            _PracticeSet.SetClientPressed(true);
            _SceneReloaderClient.SetActive(true);
        }
    }
}
