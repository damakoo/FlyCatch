using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System;

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
    [SerializeField] GameObject TimeLimitObj;
    [SerializeField] GameObject TimeLimit_Bet;
    [SerializeField] GameObject TimeLimit_notBet;
    [SerializeField] GameObject AllTrialFinishedUI;
    [SerializeField] TextMeshProUGUI TimeLimitObj_str;
    [SerializeField] GameObject Ball;
    [SerializeField] GameObject FallenArea;
    private Material FallenAreaMaterial;
    public Rigidbody Ball_rigidbody;
    private MeshRenderer Ball_mesh;
    [SerializeField] GameObject HostPlayer;
    [SerializeField] GameObject ClientPlayer;
    public float RightAmountOfMove = 0.01f;
    public float LeftAmountOfMove = 0.01f;
    public float FlyAffordTime = 1.5f;
    [SerializeField] float RightDelayTime = 0.00f;
    [SerializeField] float LeftDelayTime = 0.00f;
    Animator HostPlayerAnimator;
    Animator ClientPlayerAnimator;
    Vector3 fallpoint;
    float distance_host;
    float distance_client;
    //[SerializeField] TextMeshProUGUI YourScoreUI;
    public PracticeSet _PracticeSet { get; set; }
    private List<int> MaxScoreList = new List<int>();
    public List<int> ScoreList { get; set; } = new List<int>();
    public List<float> floatScoreList { get; set; } = new List<float>();
    private int NOTSELCETEDNUMBER = 101;
    Vector3 currentAngularVelocity;
    Vector3 currentVelocity;
    Vector3 magnusForce;
    private Quaternion _defaultQuaternionHost;
    private Quaternion _defaultQuaternionClient;
    public bool issetfallenpoints { get; set; } = false;
    public List<Vector3> fallenpoints { get; set; } = new List<Vector3>();
    [SerializeField] GenerateInitialParameter _GenerateInitialParameter;
    public void setfallenpoints() => _GenerateInitialParameter.setfallenpoints();
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
    public bool hasPracticeSet { get; set; } = false;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        Ball_mesh = Ball.GetComponent<MeshRenderer>();
        FinishUI.text = "";
        TimeLimitObj_str.text = "";
        HostPlayerAnimator = HostPlayer.GetComponent<Animator>();
        ClientPlayerAnimator = ClientPlayer.GetComponent<Animator>();
        HostPlayerAnimator.applyRootMotion = false;
        ClientPlayerAnimator.applyRootMotion = false;
        _defaultQuaternionHost = HostPlayer.transform.localRotation;
        _defaultQuaternionClient = ClientPlayer.transform.localRotation;
        FallenAreaMaterial = FallenArea.GetComponent<Renderer>().material;
    }
    private void FixedUpdate()
    {
        if (hasPracticeSet)
        {
            if (_PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.SelectCards)
            {
                currentAngularVelocity = Ball_rigidbody.angularVelocity;
                currentVelocity = Ball_rigidbody.velocity;
                // Calculate Magnus effect
                magnusForce = Vector3.Cross(currentAngularVelocity, currentVelocity) * 0.1f; // 0.1f is a magnus effect coefficient
                Ball_rigidbody.velocity = currentVelocity + magnusForce * Time.fixedDeltaTime;
                //Console.WriteLine(magnusForce.x);
                //Console.WriteLine(currentVelocity.x);
            }
        }
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
                    else if (nowTrial != 0)
                    {
                        nowTime = 0;
                        PhotonMoveToShowMyCards();
                    }
                    else
                    {
                        PhotonSetCharacterPos();
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
                    //FallenArea.transform.position = ReturnFallenArea(new Vector3(_PracticeSet.FieldCardsPracticeList[nowTrial][9], _PracticeSet.FieldCardsPracticeList[nowTrial][10], _PracticeSet.FieldCardsPracticeList[nowTrial][11]), new Vector3(_PracticeSet.FieldCardsPracticeList[nowTrial][0], _PracticeSet.FieldCardsPracticeList[nowTrial][1], _PracticeSet.FieldCardsPracticeList[nowTrial][2]), 0.5f + 0.5f * nowTime / FlyAffordTime);
                   // FallenArea.transform.localScale = new Vector3(3 + 7 * (1 - nowTime / FlyAffordTime), 0.01f, 3 + 7 * (1 - nowTime / FlyAffordTime));

                    if (nowTime > 0.15f) BlackJacking();
                    if (nowTime > _PracticeSet.FieldCardsPracticeList[nowTrial][12]) PhotonMoveToSelectBet();
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
                    //Ball_rigidbody.velocity = Vector3.zero;
                    //Ball_rigidbody.angularVelocity = Vector3.zero;
                    //Ball_rigidbody.MovePosition(new Vector3(0, 0, -10f));
                    if (nowTime > ResultsTime)
                    {
                        nowTime = 0;
                        PhotonMoveToWaitForNextTrial(nowTrial);
                    }
                    else if (nowTime > ResultsTime / 3)
                    {
                        PhotonSetCharacterPos();
                    }
                }
                else if (_PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.Finished)
                {
                    if (Input.GetKeyDown(KeyCode.Space)) PressedReload();
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
                _PracticeSet.SetTimeLeft(TimeLimit - nowTime);
                //FallenArea.transform.position = ReturnFallenArea(new Vector3(_PracticeSet.FieldCardsPracticeList[nowTrial][9], _PracticeSet.FieldCardsPracticeList[nowTrial][10], _PracticeSet.FieldCardsPracticeList[nowTrial][11]), new Vector3(_PracticeSet.FieldCardsPracticeList[nowTrial][0], _PracticeSet.FieldCardsPracticeList[nowTrial][1], _PracticeSet.FieldCardsPracticeList[nowTrial][2]), 0.5f + 0.5f * nowTime / FlyAffordTime);
                //FallenArea.transform.localScale = new Vector3(3 + 7 * (1 - nowTime / FlyAffordTime), 0.01f, 3 + 7 * (1 - nowTime / FlyAffordTime));

                if (nowTime > 0.15f) BlackJacking();
            }
            else if (_hostorclient == HostorClient.Client && _PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.SelectBet)
            {
                SelectBetting();
            }
            else if (_hostorclient == HostorClient.Client && _PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.Finished)
            {
                if (Input.GetKeyDown(KeyCode.Space)) PressedReload();
                SelectBetting();
            }
            else if (_hostorclient == HostorClient.Client)
            {
                nowTime = 0;
            }

            if (_PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.WaitForNextTrial || _PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.ShowResult) TimeLimitObj_str.text = "Rest: " + Mathf.CeilToInt(_PracticeSet.TimeLeft).ToString();
            else TimeLimitObj_str.text = "";

            if (Input.GetKeyDown(KeyCode.F))
            {
                Invoke("HostStartRunning", LeftDelayTime);
            }
            else if (Input.GetKeyUp(KeyCode.F))
            {
                Invoke("HostStopRunning", LeftDelayTime);
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                Invoke("ClientStartRunning", RightDelayTime);
            }
            else if (Input.GetKeyUp(KeyCode.J))
            {
                Invoke("ClientStopRunning", RightDelayTime);
            }
        }
    }
    void HostStartRunning()
    {
        _PracticeSet.SetHostPlayerRunning(true);
    }
    void HostStopRunning()
    {
        _PracticeSet.SetHostPlayerRunning(false);
        _PracticeSet.SetHostPlayerRot(_defaultQuaternionHost);
    }
    void ClientStartRunning()
    {
        _PracticeSet.SetClientPlayerRunning(true);
    }
    void ClientStopRunning()
    {
        _PracticeSet.SetClientPlayerRunning(false);
        _PracticeSet.SetClientPlayerRot(_defaultQuaternionClient);
    }
    public void SetPracticeSet(PracticeSet _practiceset)
    {
        _PracticeSet = _practiceset;
        _cardslist.SetPracticeSet(_practiceset);
        hasPracticeSet = true;
    }
    void PhotonChangeFallenAreaColor(bool changeRed)
    {
        _PracticeSet.ChangeFallenAreaColor(changeRed);
    }
    public void ChangeFallenAreaColor(bool changeRed)
    {
        if (changeRed)
        {
            // 現在が赤なら透明に変更
            FallenAreaMaterial.color = new Color(0f, 0f, 0f, 0.5f); 
        }
        else
        {
            // 現在が透明なら赤に変更
            FallenAreaMaterial.color = new Color(0f, 0f, 0f, 0f);
        }
    }
    void PhotonChangeFallenAreaPos(float _x, float _y, float _z)
    {
        _PracticeSet.ChangeFallenAreaPos(_x, _y, _z);
    }
    public void ChangeFallenAreaPos(float _x, float _y, float _z)
    {
        FallenArea.transform.position = new Vector3(_x, _y, _z);
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
            if (distance_host <= LeftAmountOfMove * Time.deltaTime && distance_host != 0)
            {
                Invoke("SetPlayerHostFallpoint", LeftDelayTime);

            }
            else if (distance_host > LeftAmountOfMove * Time.deltaTime)
            {
                Invoke("ExecuteHostAfterDelay", LeftDelayTime);
            }
        }
        if (Input.GetKey(KeyCode.J))
        {
            if (distance_client <= RightAmountOfMove * Time.deltaTime && distance_client != 0)
            {
                Invoke("SetPlayerClientFallpoint", RightDelayTime);
            }
            else if (distance_client > RightAmountOfMove * Time.deltaTime)
            {
                Invoke("ExecuteClientAfterDelay", RightDelayTime);
            }
        }
        HostPlayer.transform.position = _PracticeSet.HostPlayerPos;
        ClientPlayer.transform.position = _PracticeSet.ClientPlayerPos;
        HostPlayer.transform.rotation = _PracticeSet.HostPlayerRot;
        ClientPlayer.transform.rotation = _PracticeSet.ClientPlayerRot;
        HostPlayerAnimator.SetBool("Running", _PracticeSet.HostPlayerRunning);
        ClientPlayerAnimator.SetBool("Running", _PracticeSet.ClientPlayerRunning);

        }
    private void SetPlayerHostFallpoint()
    {
        if (_PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.SelectCards)
        {
            _PracticeSet.SetHostPlayerPos(fallpoint.x, fallpoint.y, fallpoint.z);
            _PracticeSet.FixMySelectedTime(Vector3.Magnitude(fallpoint - new Vector3(_PracticeSet.MyCardsPracticeList[nowTrial][0], _PracticeSet.MyCardsPracticeList[nowTrial][1], _PracticeSet.MyCardsPracticeList[nowTrial][2])) / LeftAmountOfMove, nowTrial);
            _PracticeSet.SetMyApproachTime(nowTime - Time.deltaTime + distance_host / LeftAmountOfMove, nowTrial);
        }

    }
    private void SetPlayerClientFallpoint()
    {
        if (_PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.SelectCards)
        {
            _PracticeSet.SetClientPlayerPos(fallpoint.x, fallpoint.y, fallpoint.z);
            _PracticeSet.FixYourSelectedTime(Vector3.Magnitude(fallpoint - new Vector3(_PracticeSet.YourCardsPracticeList[nowTrial][0], _PracticeSet.YourCardsPracticeList[nowTrial][1], _PracticeSet.YourCardsPracticeList[nowTrial][2])) / RightAmountOfMove, nowTrial);
            _PracticeSet.SetYourApproachTime(nowTime - Time.deltaTime + distance_client / RightAmountOfMove, nowTrial);
        }

    }
    private void ExecuteHostAfterDelay()
    {
        if (_PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.SelectCards)
        {
            distance_host = Vector3.Magnitude(fallpoint - HostPlayer.transform.position);
            _PracticeSet.SetMySelectedTime(Time.deltaTime, nowTrial);
            if (distance_host < LeftAmountOfMove * Time.deltaTime)
            {
                _PracticeSet.SetHostPlayerPos(fallpoint.x, fallpoint.y, fallpoint.z);
            }
            else
            {
                Vector3 destination = HostPlayer.transform.position + (fallpoint - HostPlayer.transform.position) / distance_host * LeftAmountOfMove * Time.deltaTime;
                _PracticeSet.SetHostPlayerPos(destination.x, destination.y, destination.z);
                _PracticeSet.SetHostPlayerRot(Quaternion.LookRotation((fallpoint - HostPlayer.transform.position).normalized));
            }
            if (distance_host - LeftAmountOfMove * Time.deltaTime < LeftAmountOfMove * 1.5f * 0.1f)
            {
                _PracticeSet.SetMyApproachTime(nowTime, nowTrial);
            }
        }

    }
    private void ExecuteClientAfterDelay()
    {
        if (_PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.SelectCards)
        {
            distance_client = Vector3.Magnitude(fallpoint - ClientPlayer.transform.position);
            _PracticeSet.SetYourSelectedTime(Time.deltaTime, nowTrial);
            if (distance_client < RightAmountOfMove * Time.deltaTime)
            {
                _PracticeSet.SetClientPlayerPos(fallpoint.x, fallpoint.y, fallpoint.z);
            }
            else
            {
                Vector3 destination = ClientPlayer.transform.position + (fallpoint - ClientPlayer.transform.position) / distance_client * RightAmountOfMove * Time.deltaTime;
                _PracticeSet.SetClientPlayerPos(destination.x, destination.y, destination.z);
                _PracticeSet.SetClientPlayerRot(Quaternion.LookRotation((fallpoint - ClientPlayer.transform.position).normalized));
            }
            if (distance_client - RightAmountOfMove * Time.deltaTime < RightAmountOfMove * 1.5f * 0.1f)
            {
                _PracticeSet.SetYourApproachTime(nowTime, nowTrial);
            }
        }
    }


    void SelectBetting()
    {

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
        _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.ShowMyCards;
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
        //TimeLimitObj.transform.position = TimeLimit_Bet.transform.position;
    }
    public void MoveToSelectCards()
    {
        //_cardslist.AllOpen();
        Ball_rigidbody.useGravity = true;
        Ball_rigidbody.MovePosition(new Vector3(_PracticeSet.FieldCardsPracticeList[nowTrial][0], _PracticeSet.FieldCardsPracticeList[nowTrial][1], _PracticeSet.FieldCardsPracticeList[nowTrial][2]));
        Ball_rigidbody.velocity = new Vector3(_PracticeSet.FieldCardsPracticeList[nowTrial][3], _PracticeSet.FieldCardsPracticeList[nowTrial][4], _PracticeSet.FieldCardsPracticeList[nowTrial][5]);
        Ball_rigidbody.angularVelocity = new Vector3(_PracticeSet.FieldCardsPracticeList[nowTrial][6], _PracticeSet.FieldCardsPracticeList[nowTrial][7], _PracticeSet.FieldCardsPracticeList[nowTrial][8]);
        fallpoint = new Vector3(_PracticeSet.FieldCardsPracticeList[nowTrial][9], _PracticeSet.FieldCardsPracticeList[nowTrial][10], _PracticeSet.FieldCardsPracticeList[nowTrial][11]);
        _PracticeSet.SetHostPlayerPos(_PracticeSet.MyCardsPracticeList[nowTrial][0], _PracticeSet.MyCardsPracticeList[nowTrial][1], _PracticeSet.MyCardsPracticeList[nowTrial][2]);
        _PracticeSet.SetClientPlayerPos(_PracticeSet.YourCardsPracticeList[nowTrial][0], _PracticeSet.YourCardsPracticeList[nowTrial][1], _PracticeSet.YourCardsPracticeList[nowTrial][2]);
        _PracticeSet.SetHostPlayerRot(_defaultQuaternionHost);
        _PracticeSet.SetClientPlayerRot(_defaultQuaternionClient);
        HostPlayer.transform.position = _PracticeSet.HostPlayerPos;
        ClientPlayer.transform.position = _PracticeSet.ClientPlayerPos;
        HostPlayer.transform.rotation = _PracticeSet.HostPlayerRot;
        ClientPlayer.transform.rotation = _PracticeSet.ClientPlayerRot;
        _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.SelectCards;
        //TimeLimitObj.transform.position = TimeLimit_notBet.transform.position;
    }
    public void PhotonMoveToSelectCards()
    {
        _PracticeSet.MoveToSelectCards();
        PhotonChangeFallenAreaColor(true);
    }
    public void MoveToShowResult()
    {
        if (_PracticeSet.Score == 1) Ball_mesh.enabled = false;
        //_blackJackRecorder.RecordResult((_PracticeSet.MySelectedCard == NOTSELCETEDNUMBER) ? 0 : _cardslist.MyCardsList[_PracticeSet.MySelectedCard].Number, (_PracticeSet.YourSelectedCard == NOTSELCETEDNUMBER) ? 0 : _cardslist.YourCardsList[_PracticeSet.YourSelectedCard].Number, (useSuit) ? CalculateSuitScore() : Score, _PracticeSet.MySelectedBet, _PracticeSet.YourSelectedBet);
        _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.ShowResult;
        MyScoreUI.text = (_PracticeSet.Score == 1 ? "Succeed!" : "Failed!") + "Score:" + (MathF.Round(_PracticeSet.floatScore/10)*10).ToString("F1");
        //    + "\n Left Pressed:" + _PracticeSet.MySelectedTime[nowTrial].ToString("F1") + "s," + "Approached:" + (_PracticeSet.MyApproachedTime[nowTrial] < 90 ? _PracticeSet.MyApproachedTime[nowTrial].ToString("F1") : "NaN") + "s"
        //    + "\n Right Pressed:" + _PracticeSet.YourSelectedTime[nowTrial].ToString("F1") + "s," + "Approached:" + (_PracticeSet.YourApproachedTime[nowTrial] < 90 ? _PracticeSet.YourApproachedTime[nowTrial].ToString("F1") : "NaN") + "s";
        ScoreList.Add(_PracticeSet.Score);
        floatScoreList.Add(_PracticeSet.floatScore);
        nowTime = 0;
        nowTrial += 1;
        if (nowTrial == _PracticeSet.TrialAll)
        {
            _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.Finished;
            //FinishUI.text = "Finished! \n ScoreAll:" + ReturnSum(ScoreList).ToString() + "/" + ReturnSum(MaxScoreList).ToString() + "\n" + "Trial: " + _blackJackRecorder.Trial.ToString() + "/" + NumberofSet.ToString();
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
        else
        {

        }
    }
    public void SetCharacterPos()
    {
        HostPlayer.transform.position = _PracticeSet.HostPlayerPos;
        ClientPlayer.transform.position = _PracticeSet.ClientPlayerPos;
        HostPlayer.transform.rotation = _PracticeSet.HostPlayerRot;
        ClientPlayer.transform.rotation = _PracticeSet.ClientPlayerRot;
        Ball_rigidbody.MovePosition(new Vector3(_PracticeSet.FieldCardsPracticeList[nowTrial][0], _PracticeSet.FieldCardsPracticeList[nowTrial][1], _PracticeSet.FieldCardsPracticeList[nowTrial][2]));
        Ball_rigidbody.rotation = Quaternion.identity;
        Ball_mesh.enabled = true;
        Ball_rigidbody.useGravity = false;
        
    }
    private void PhotonSetCharacterPos()
    {
        _PracticeSet.SetHostPlayerPos(_PracticeSet.MyCardsPracticeList[nowTrial][0], _PracticeSet.MyCardsPracticeList[nowTrial][1], _PracticeSet.MyCardsPracticeList[nowTrial][2]);
        _PracticeSet.SetClientPlayerPos(_PracticeSet.YourCardsPracticeList[nowTrial][0], _PracticeSet.YourCardsPracticeList[nowTrial][1], _PracticeSet.YourCardsPracticeList[nowTrial][2]);
        _PracticeSet.SetHostPlayerRot(_defaultQuaternionHost);
        _PracticeSet.SetClientPlayerRot(_defaultQuaternionClient);
        _PracticeSet.SetCharacterPos();
        PhotonChangeFallenAreaPos(_PracticeSet.FieldCardsPracticeList[nowTrial][9], _PracticeSet.FieldCardsPracticeList[nowTrial][10], _PracticeSet.FieldCardsPracticeList[nowTrial][11]);
    }
    private Vector3 ReturnFallenArea(Vector3 _fallenpos, Vector3 _launchpos, float _rate)
    {
        Vector3 _pos = _launchpos + (_fallenpos - _launchpos) * _rate;
        return new Vector3(_pos.x, 0, _pos.z);

    }
    public void PhotonMoveToShowResult()
    {
        SetApproachRate();
        _PracticeSet.SetfloatScore(CalculatefloatScore_0805());
        _PracticeSet.SetScore(CalculateResult());
        _PracticeSet.MoveToShowResult();
        PhotonChangeFallenAreaColor(false);

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
        //TimeLimitObj.transform.position = TimeLimit_notBet.transform.position;
    }
    public void PhotonMoveToWaitForNextTrial(int _nowTrial)
    {
        _PracticeSet.MoveToWaitForNextTrial(_nowTrial);
    }
    private int CalculateResult()
    {
        distance_host = Vector3.Magnitude(fallpoint - HostPlayer.transform.position);
        distance_client = Vector3.Magnitude(fallpoint - ClientPlayer.transform.position);
        return (distance_host == 0 || distance_client == 0) ? 1 : 0;
    }
    private float CalculatefloatScore_0805()
    {

        distance_host = Vector3.Magnitude(fallpoint - HostPlayer.transform.position);
        distance_client = Vector3.Magnitude(fallpoint - ClientPlayer.transform.position);
        float _succeed = (distance_client == 0 || distance_host == 0) ? 1 : 0;//(distance_host < LeftAmountOfMove * 1.5f * 0.1f || distance_client < RightAmountOfMove * 1.5f * 0.1f) ? 1 : 0;
        float Mydistance = Vector3.Magnitude(fallpoint - new Vector3(_PracticeSet.MyCardsPracticeList[nowTrial][0], _PracticeSet.MyCardsPracticeList[nowTrial][1], _PracticeSet.MyCardsPracticeList[nowTrial][2]));
        float Yourdistance = Vector3.Magnitude(fallpoint - new Vector3(_PracticeSet.YourCardsPracticeList[nowTrial][0], _PracticeSet.YourCardsPracticeList[nowTrial][1], _PracticeSet.YourCardsPracticeList[nowTrial][2]));
        //return _succeed * Mathf.Abs(_PracticeSet.MySelectedTime[nowTrial] - _PracticeSet.YourSelectedTime[nowTrial]);// / (Mydistance - Yourdistance);
        //return _succeed
        //    * (10 - Mathf.Min(_PracticeSet.MyApproachedTime[nowTrial], _PracticeSet.YourApproachedTime[nowTrial]))
        //    * (10 - (_PracticeSet.MyApproachedTime[nowTrial] < _PracticeSet.YourApproachedTime[nowTrial] ? _PracticeSet.YourSelectedTime[nowTrial] : _PracticeSet.MySelectedTime[nowTrial]));
        return _succeed == 1 ?
                 MathF.Min(100, 100 * (Mathf.Min(Mydistance / LeftAmountOfMove, Yourdistance / RightAmountOfMove) / (_PracticeSet.MySelectedTime[nowTrial] + _PracticeSet.YourSelectedTime[nowTrial])))
            : 0;
        /*return _succeed == 1 ? Mathf.Min(
                 100 * Mathf.Min(Mydistance - LeftAmountOfMove * 1.5f * 0.1f, Yourdistance - RightAmountOfMove * 1.5f * 0.1f) / (_PracticeSet.MySelectedTime[nowTrial] * LeftAmountOfMove + _PracticeSet.YourSelectedTime[nowTrial] * RightAmountOfMove),
            100) : 0;
        return _succeed == 1 ? Mathf.Min(
                    (50 * Mathf.Min((Mydistance - LeftAmountOfMove * 1.5f * 0.1f) / LeftAmountOfMove + LeftDelayTime, (Yourdistance - RightAmountOfMove * 1.5f * 0.1f) / RightAmountOfMove + RightDelayTime) / (Mathf.Min(_PracticeSet.MyApproachedTime[nowTrial], _PracticeSet.YourApproachedTime[nowTrial]))
                     + 50 * Mathf.Min(Mydistance - LeftAmountOfMove * 1.5f * 0.1f, Yourdistance - RightAmountOfMove * 1.5f * 0.1f) / (_PracticeSet.MySelectedTime[nowTrial] * LeftAmountOfMove + _PracticeSet.YourSelectedTime[nowTrial] * RightAmountOfMove)),
                100) : 0;*/
    }
    private void SetApproachRate()
    {
        Console.WriteLine(_PracticeSet.YourSelectedTime[nowTrial] * RightAmountOfMove / Vector3.Magnitude(fallpoint - new Vector3(_PracticeSet.YourCardsPracticeList[nowTrial][0], _PracticeSet.YourCardsPracticeList[nowTrial][1], _PracticeSet.YourCardsPracticeList[nowTrial][2])));
        _PracticeSet.SetMyApproachRate(_PracticeSet.MySelectedTime[nowTrial] * LeftAmountOfMove / Vector3.Magnitude(fallpoint - new Vector3(_PracticeSet.MyCardsPracticeList[nowTrial][0], _PracticeSet.MyCardsPracticeList[nowTrial][1], _PracticeSet.MyCardsPracticeList[nowTrial][2])), nowTrial);
        _PracticeSet.SetYourApproachRate(_PracticeSet.YourSelectedTime[nowTrial] * RightAmountOfMove / Vector3.Magnitude(fallpoint - new Vector3(_PracticeSet.YourCardsPracticeList[nowTrial][0], _PracticeSet.YourCardsPracticeList[nowTrial][1], _PracticeSet.YourCardsPracticeList[nowTrial][2])), nowTrial);
    }
    public void MakeReadyHost()
    {
        _decideHostorClient.HostReady = true;
    }
    public void MakeReadyClient()
    {
        _decideHostorClient.ClientReady = true;
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
        PhotonMoveToWaitForNextTrial(nowTrial);
        _PracticeSet.SetHostPressed(false);
        _PracticeSet.SetClientPressed(false);
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
        floatScoreList = new List<float>();
        nowTrial = 0;
        nowTime = 0;
        _blackJackRecorder.Initialize();
        _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.BeforeStart;
        MyScoreUI.text = "";
        //PhotonGameStartUI();
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
