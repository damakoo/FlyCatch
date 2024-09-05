using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Text.RegularExpressions;
using System.Linq;
using System;


public class PracticeSet : MonoBehaviourPunCallbacks
{
    BlackJackManager _BlackJackManager;
    private PhotonView _PhotonView;
    public int MySelectedCard { get; set; }
    public int YourSelectedCard { get; set; }
    public List<float> MySelectedTime { get; set; }
    public List<float> YourSelectedTime { get; set; }
    public List<float> MyApproachedTime { get; set; }
    public List<float> YourApproachedTime { get; set; }
    public int MySelectedBet { get; set; }
    public int YourSelectedBet { get; set; }
    public float TimeLeft { get; set; } = 0;
    public bool HostPressed { get; set; } = false;
    public bool ClientPressed { get; set; } = false;
    public void SetMyApproachTime(float _myapproachtime, int trial)
    {
        MyApproachedTime[trial] = _myapproachtime;
        _PhotonView.RPC("UpdateMyApproachedTimeOnAllClients", RpcTarget.Others, _myapproachtime, trial);
    }
    [PunRPC]
    void UpdateMyApproachedTimeOnAllClients(float _myapproachtime, int trial)
    {
        MyApproachedTime[trial] = _myapproachtime;
    }
    public void SetYourApproachTime(float _yourapproachtime, int trial)
    {
        YourApproachedTime[trial] = _yourapproachtime;
        _PhotonView.RPC("UpdateYourApproachedTimeOnAllClients", RpcTarget.Others, _yourapproachtime, trial);
    }
    [PunRPC]
    void UpdateYourApproachedTimeOnAllClients(float _yourapproachtime, int trial)
    {
        YourApproachedTime[trial] = _yourapproachtime;
    }
    public void SetHostPressed(bool _hostpressed)
    {
        HostPressed = _hostpressed;
        _PhotonView.RPC("UpdateHostPressedOnAllClients", RpcTarget.Others, _hostpressed);
    }
    [PunRPC]
    void UpdateHostPressedOnAllClients(bool _hostpressed)
    {
        HostPressed = _hostpressed;
    }
    public void SetClientPressed(bool _clientpressed)
    {
        ClientPressed = _clientpressed;
        _PhotonView.RPC("UpdateClientPressedOnAllClients", RpcTarget.Others, _clientpressed);
    }
    [PunRPC]
    void UpdateClientPressedOnAllClients(bool _clientpressed)
    {
        ClientPressed = _clientpressed;
    }
    public void SetTimeLeft(float _timeleft)
    {
        TimeLeft = _timeleft;
        _PhotonView.RPC("UpdateTimeLeftOnAllClients", RpcTarget.Others, _timeleft);
    }
    [PunRPC]
    void UpdateTimeLeftOnAllClients(float _timeleft)
    {
        // ここでカードデータを再構築
        TimeLeft = _timeleft;
    }
    public void SetMySelectedBet(int bet)
    {
        MySelectedBet = bet;
        _PhotonView.RPC("UpdateMySelectedBetOnAllClients", RpcTarget.Others, bet);
    }
    [PunRPC]
    void UpdateMySelectedBetOnAllClients(int bet)
    {
        // ここでカードデータを再構築
        MySelectedBet = bet;
    }
    public void SetYourSelectedBet(int bet)
    {
        YourSelectedBet = bet;
        _PhotonView.RPC("UpdateYourSelectedBetOnAllClients", RpcTarget.Others, bet);
    }
    [PunRPC]
    void UpdateYourSelectedBetOnAllClients(int bet)
    {
        // ここでカードデータを再構築
        YourSelectedBet = bet;
    }
    public void SetMySelectedTime(float time, int trial)
    {
        MySelectedTime[trial] += time;
        _PhotonView.RPC("UpdateMySelectedTimeOnAllClients", RpcTarget.Others, time, trial);
    }
    [PunRPC]
    void UpdateMySelectedTimeOnAllClients(float time, int trial)
    {
        // ここでカードデータを再構築
        MySelectedTime[trial] += time;
    }
    public void FixMySelectedTime(float time, int trial)
    {
        MySelectedTime[trial] = time;
        _PhotonView.RPC("UpdateFixMySelectedTimeOnAllClients", RpcTarget.Others, time, trial);
    }
    [PunRPC]
    void UpdateFixMySelectedTimeOnAllClients(float time, int trial)
    {
        // ここでカードデータを再構築
        MySelectedTime[trial] = time;
    }
    public void FixYourSelectedTime(float time, int trial)
    {
        YourSelectedTime[trial] = time;
        _PhotonView.RPC("UpdateFixYourSelectedTimeOnAllClients", RpcTarget.Others, time, trial);
    }
    [PunRPC]
    void UpdateFixYourSelectedTimeOnAllClients(float time, int trial)
    {
        // ここでカードデータを再構築
        YourSelectedTime[trial] = time;
    }
    public void SetYourSelectedTime(float time, int trial)
    {
        YourSelectedTime[trial] += time;
        _PhotonView.RPC("UpdateYourSelectedTimeOnAllClients", RpcTarget.Others, time, trial);
    }
    [PunRPC]
    void UpdateYourSelectedTimeOnAllClients(float time, int trial)
    {
        // ここでカードデータを再構築
        YourSelectedTime[trial] += time;
    }
    public void SetMySelectedCard(int card)
    {
        MySelectedCard = card;
        _PhotonView.RPC("UpdateMySelectedCardOnAllClients", RpcTarget.Others, card);
    }
    [PunRPC]
    void UpdateMySelectedCardOnAllClients(int _Number)
    {
        // ここでカードデータを再構築
        MySelectedCard = _Number;
    }
    public void SetYourSelectedCard(int card)
    {
        YourSelectedCard = card;
        _PhotonView.RPC("UpdateYourSelectedCardOnAllClients", RpcTarget.Others, card);
    }
    [PunRPC]
    void UpdateYourSelectedCardOnAllClients(int _Number)
    {
        // ここでカードデータを再構築
        YourSelectedCard = _Number;
    }
    public Vector3 HostPlayerPos;
    public Vector3 ClientPlayerPos;
    public Quaternion HostPlayerRot;
    public Quaternion ClientPlayerRot;
    public bool HostPlayerRunning;
    public bool ClientPlayerRunning;
    public void SetHostPlayerRunning(bool _HostPlayerRunning)
    {
        HostPlayerRunning = _HostPlayerRunning;
        _PhotonView.RPC("UpdateSetHostPlayerRunning", RpcTarget.Others, _HostPlayerRunning);
    }
    [PunRPC]
    void UpdateSetHostPlayerRunning(bool _HostPlayerRunning)
    {
        // ここでカードデータを再構築
        HostPlayerRunning = _HostPlayerRunning;
    }
    public void SetClientPlayerRunning(bool _ClientPlayerRunning)
    {
        ClientPlayerRunning = _ClientPlayerRunning;
        _PhotonView.RPC("UpdateSetHostPlayerRunning", RpcTarget.Others, _ClientPlayerRunning);
    }
    [PunRPC]
    void UpdateSetClientPlayerRunning(bool _ClientPlayerRunning)
    {
        // ここでカードデータを再構築
        ClientPlayerRunning = _ClientPlayerRunning;
    }
    public void SetHostPlayerRot(Quaternion _hostplayerrot)
    {
        HostPlayerRot = _hostplayerrot;
        _PhotonView.RPC("UpdateSetHostPlayerRot", RpcTarget.Others, _hostplayerrot.x, _hostplayerrot.y, _hostplayerrot.z, _hostplayerrot.w);
    }
    [PunRPC]
    void UpdateSetHostPlayerRot(float _hostplayerrot_x, float _hostplayerrot_y, float _hostplayerrot_z, float _hostplayerrot_w)
    {
        // ここでカードデータを再構築
        HostPlayerRot = new Quaternion(_hostplayerrot_x, _hostplayerrot_y, _hostplayerrot_z, _hostplayerrot_w);
    }
    public void SetClientPlayerRot(Quaternion _clientplayerrot)
    {
        ClientPlayerRot = _clientplayerrot;
        _PhotonView.RPC("UpdateSetClientPlayerRot", RpcTarget.Others, _clientplayerrot.x, _clientplayerrot.y, _clientplayerrot.z, _clientplayerrot.w);
    }
    [PunRPC]
    void UpdateSetClientPlayerRot(float _clientplayerrot_x, float _clientplayerrot_y, float _clientplayerrot_z, float _clientplayerrot_w)
    {
        // ここでカードデータを再構築
        ClientPlayerRot = new Quaternion(_clientplayerrot_x, _clientplayerrot_y, _clientplayerrot_z, _clientplayerrot_w);
    }
    public void SetHostPlayerPos(float _hostplayerpos_x, float _hostplayerpos_y, float _hostplayerpos_z)
    {
        HostPlayerPos = new Vector3(_hostplayerpos_x, _hostplayerpos_y, _hostplayerpos_z);
        _PhotonView.RPC("UpdateSetHostPlayerPos", RpcTarget.Others, _hostplayerpos_x, _hostplayerpos_y, _hostplayerpos_z);
    }
    [PunRPC]
    void UpdateSetHostPlayerPos(float _hostplayerpos_x, float _hostplayerpos_y, float _hostplayerpos_z)
    {
        // ここでカードデータを再構築
        HostPlayerPos = new Vector3(_hostplayerpos_x, _hostplayerpos_y, _hostplayerpos_z);
    }
    public void SetClientPlayerPos(float _clientplayerpos_x, float _clientplayerpos_y, float _clientplayerpos_z)
    {
        ClientPlayerPos = new Vector3(_clientplayerpos_x, _clientplayerpos_y, _clientplayerpos_z);
        _PhotonView.RPC("UpdateSetClientPlayerPos", RpcTarget.Others, _clientplayerpos_x, _clientplayerpos_y, _clientplayerpos_z);
    }
    [PunRPC]
    void UpdateSetClientPlayerPos(float _clientplayerpos_x, float _clientplayerpos_y, float _clientplayerpos_z)
    {
        // ここでカードデータを再構築
        ClientPlayerPos = new Vector3(_clientplayerpos_x, _clientplayerpos_y, _clientplayerpos_z);
    }
    public List<List<float>> MyCardsPracticeList { get; set; } = new List<List<float>>();
    public List<List<float>> YourCardsPracticeList { get; set; } = new List<List<float>>();
    public List<List<float>> FieldCardsPracticeList /*{ get; set; }*/ = new List<List<float>>();
    public void SetMyCardsPracticeList(List<List<float>> _MyCardsPracticeList)
    {
        List<List<float>> temp = _MyCardsPracticeList;
        MyCardsPracticeList = temp;
        _PhotonView.RPC("UpdateMyCardsPracticeListOnAllClients", RpcTarget.Others, SerializeCardList(_MyCardsPracticeList));
    }
    [PunRPC]
    void UpdateMyCardsPracticeListOnAllClients(string serializeCards)
    {
        // ここでカードデータを再構築
        MyCardsPracticeList = DeserializeCardList(serializeCards);
    }
    public void SetYourCardsPracticeList(List<List<float>> _YourCardsPracticeList)
    {
        List<List<float>> temp = _YourCardsPracticeList;
        YourCardsPracticeList = temp;
        _PhotonView.RPC("UpdateYourCardsPracticeListOnAllClients", RpcTarget.Others, SerializeCardList(_YourCardsPracticeList));
    }
    [PunRPC]
    void UpdateYourCardsPracticeListOnAllClients(string serializeCards)
    {
        // ここでカードデータを再構築
        YourCardsPracticeList = DeserializeCardList(serializeCards);
    }
    public void SetFieldCardsList(List<List<float>> _FieldCardsPracticeList)
    {
        List<List<float>> temp = FieldCardsPracticeList;
        FieldCardsPracticeList = temp;
        _PhotonView.RPC("UpdateFieldCardsPracticeListOnAllClients", RpcTarget.Others, SerializeCardList(_FieldCardsPracticeList), _FieldCardsPracticeList[0].Count);
    }
    [PunRPC]
    void UpdateFieldCardsPracticeListOnAllClients(string serializeCards, int _length)
    {
        // ここでカードデータを再構築
        FieldCardsPracticeList = DeserializeFieldCardList(serializeCards, _length);
    }

    private string SerializeCardList(List<List<float>> cards)
    {

        string cards_json = "[";
        for (int i = 0; i < cards.Count; i++)
        {
            cards_json += JsonHelper.ToJson(cards[i]) + ",";
        }
        cards_json = cards_json.Remove(cards_json.Length - 1);
        cards_json += "]";
        return cards_json;
    }

    private List<List<float>> DeserializeCardList(string json)
    {
        Regex regex = new Regex(@"-?\d+(\.\d+)?");

        List<float> numbers = new List<float>();
        foreach (Match match in regex.Matches(json))
        {
            numbers.Add(float.Parse(match.Value));
        }
        List<List<float>> cardList = new List<List<float>>();
        // JSON 文字列を int[] の配列に変換
        for (int i = 0; i < NumberofSet; i++)
        {
            List<float> Element = new List<float>();
            for (int j = 0; j < NumberofCards; j++)
            {
                Element.Add(numbers[i * NumberofCards + j]);
            }
            cardList.Add(Element);
        }
        return cardList;
    }
    private List<List<float>> DeserializeFieldCardList(string json, int _length)
    {
        Regex regex = new Regex(@"-?\d+(\.\d+)?");

        List<float> numbers = new List<float>();
        foreach (Match match in regex.Matches(json))
        {
            numbers.Add(float.Parse(match.Value));
        }
        List<List<float>> cardList = new List<List<float>>();
        // JSON 文字列を int[] の配列に変換
        for (int i = 0; i < NumberofSet; i++)
        {
            List<float> Element = new List<float>();
            for (int j = 0; j < _length; j++)
            {
                Element.Add(numbers[i * _length + j]);
            }
            cardList.Add(Element);
        }
        return cardList;
    }

    private string SerializeFieldCard(List<int> cards)
    {
        return JsonHelper.ToJson(cards);
    }

    private List<int> DeserializeFieldCard(string serializedCards)
    {
        Regex regex = new Regex(@"\d+");

        List<int> numbers = new List<int>();
        foreach (Match match in regex.Matches(serializedCards))
        {
            numbers.Add(int.Parse(match.Value));
        }
        return numbers;
    }

    [System.Serializable]
    private class SerializationWrapper<T>
    {
        public T data;

        public SerializationWrapper(T data)
        {
            this.data = data;
        }
    }

    public List<List<int>> MyCardsSuitPracticeList = new List<List<int>>();
    public List<List<int>> YourCardsSuitPracticeList = new List<List<int>>();
    public List<int> FieldCardsSuitPracticeList = new List<int>();
    public void SetMyCardsSuitPracticeList(List<List<int>> _MyCardsSuitPracticeList)
    {
        //List<List<int>> temp = _MyCardsSuitPracticeList;
        //MyCardsSuitPracticeList = temp;
        //_PhotonView.RPC("UpdateMyCardsSuitPracticeListOnAllClients", RpcTarget.Others, SerializeCardList(_MyCardsSuitPracticeList));
    }
    [PunRPC]
    void UpdateMyCardsSuitPracticeListOnAllClients(string serializeCards)
    {
        // ここでカードデータを再構築
        //MyCardsSuitPracticeList = DeserializeCardList(serializeCards);
    }
    public void SetYourCardsSuitPracticeList(List<List<int>> _YourCardsSuitPracticeList)
    {
        //List<List<int>> temp = _YourCardsSuitPracticeList;
        //YourCardsSuitPracticeList = temp;
        //_PhotonView.RPC("UpdateYourCardsSuitPracticeListOnAllClients", RpcTarget.Others, SerializeCardList(_YourCardsSuitPracticeList));
    }
    [PunRPC]
    void UpdateYourCardsSuitPracticeListOnAllClients(string serializeCards)
    {
        // ここでカードデータを再構築
        //YourCardsSuitPracticeList = DeserializeCardList(serializeCards);
    }
    public void SetFieldCardsSuitList(List<int> _FieldCardsSuitPracticeList)
    {
        //List<int> temp = FieldCardsSuitPracticeList;
        //FieldCardsSuitPracticeList = temp;
        //_PhotonView.RPC("UpdateFieldCardsSuitPracticeListOnAllClients", RpcTarget.Others, SerializeFieldCard(_FieldCardsSuitPracticeList));
    }
    [PunRPC]
    void UpdateFieldCardsSuitPracticeListOnAllClients(string serializeCards)
    {
        // ここでカードデータを再構築
        // FieldCardsSuitPracticeList = DeserializeFieldCard(serializeCards);
    }

    public enum BlackJackStateList
    {
        BeforeStart,
        WaitForNextTrial,
        ShowMyCards,
        SelectCards,
        SelectBet,
        ShowResult,
        Finished,
    }
    public BlackJackStateList BlackJackState = BlackJackStateList.BeforeStart;

    public void SetBlackJackState(BlackJackStateList _BlackJackState)
    {
        BlackJackState = _BlackJackState;
        _PhotonView.RPC("UpdateBlackJackStateListOnAllClients", RpcTarget.Others, SerializeBlackJackState(_BlackJackState));
    }
    [PunRPC]
    void UpdateBlackJackStateListOnAllClients(string serializeCards)
    {
        // ここでカードデータを再構築
        BlackJackState = DeserializeBlackJackState(serializeCards);
    }

    private string SerializeBlackJackState(BlackJackStateList _BlackJackState)
    {
        return JsonUtility.ToJson(new SerializationWrapper<BlackJackStateList>(_BlackJackState));
    }

    private BlackJackStateList DeserializeBlackJackState(string serializedCards)
    {
        return JsonUtility.FromJson<SerializationWrapper<BlackJackStateList>>(serializedCards).data;
    }

    public int TrialAll;
    public int NumberofCards { get; set; } = 3;


    public int NumberofSet { get; set; } = 5;
    List<float> FieldCards = new List<float>();

    List<float> MyCards;
    List<float> YourCards;
    private static List<int> MyCardsSuit;
    private static List<int> YourCardsSuit;
    private static int FieldCardsSuit = 0;
    private void Start()
    {
        _PhotonView = GetComponent<PhotonView>();
        _BlackJackManager = GameObject.FindWithTag("Manager").GetComponent<BlackJackManager>();
    }
    public void UpdateParameter()
    {
        List<int> _order = GenerateRandomList(1, CardPattern.FieldCardPattern.Count);
        for (int i = 0; i < NumberofSet; i++)
        {
            //DecidingCards(Random.Range(0, NumberofCards));
            //DecidingCards(RandomValue());
            DecideDecidedCards(_order[i] - 1);
            FieldCardsPracticeList.Add(FieldCards);
            MyCardsPracticeList.Add(MyCards);
            YourCardsPracticeList.Add(YourCards);
        }
        SetMyCardsPracticeList(MyCardsPracticeList);
        SetYourCardsPracticeList(YourCardsPracticeList);
        SetFieldCardsList(FieldCardsPracticeList);
        InitializeCard();
    }
    public void ReUpdateParameter()
    {
        FieldCardsPracticeList = new List<List<float>>();
        MyCardsPracticeList = new List<List<float>>();
        YourCardsPracticeList = new List<List<float>>();

        List<int> _order = GenerateRandomList(1, CardPattern.FieldCardPattern.Count);
        for (int i = 0; i < NumberofSet; i++)
        {
            //DecidingCards(Random.Range(0, NumberofCards));
            //DecidingCards(RandomValue());
            DecideDecidedCards(_order[i] - 1);
            FieldCardsPracticeList.Add(FieldCards);
            MyCardsPracticeList.Add(MyCards);
            YourCardsPracticeList.Add(YourCards);
        }
        SetMyCardsPracticeList(MyCardsPracticeList);
        SetYourCardsPracticeList(YourCardsPracticeList);
        SetFieldCardsList(FieldCardsPracticeList);
        ReInitializeCard();
    }
    private int RandomValue()
    {
        int result = UnityEngine.Random.Range(0, 4);
        while (result == 1)
        {
            result = UnityEngine.Random.Range(0, 4);
        }
        Debug.Log(result);
        return result;
    }
    public void InitializeCard()
    {
        _BlackJackManager.InitializeCard();
        _PhotonView.RPC("RPCInitializeCard", RpcTarget.Others);
    }
    [PunRPC]
    void RPCInitializeCard()
    {
        // ここでカードデータを再構築
        _BlackJackManager.InitializeCard();
    }
    public void ReInitializeCard()
    {
        _BlackJackManager.ReInitializeCard();
        _PhotonView.RPC("RPCReInitializeCard", RpcTarget.Others);
    }
    [PunRPC]
    void RPCReInitializeCard()
    {
        // ここでカードデータを再構築
        _BlackJackManager.ReInitializeCard();
    }
    void DecideDecidedCards(int _order)
    {
        //MyCards = new List<float>() { UnityEngine.Random.Range(15f, 25f), 0, UnityEngine.Random.Range(10f, 20f) };
        //YourCards = new List<float>() { UnityEngine.Random.Range(-25f, -15f), 0, UnityEngine.Random.Range(10f, 20f) };
        MyCards = new List<float>() { 30.5f, 0, -33.5f };
        YourCards = new List<float>() { -30.5f, 0, 33.5f };
        Vector3 fallpoint = new Vector3(UnityEngine.Random.Range(-30.5f, 30.5f), 0f, UnityEngine.Random.Range(-33.5f, 33.5f));
        Vector3 launchpoint = new Vector3(UnityEngine.Random.Range(-30.5f, 30.5f), UnityEngine.Random.Range(20f, 25f), UnityEngine.Random.Range(-97.5f, -50.5f));
        float Mydistance = Vector3.Magnitude(fallpoint - new Vector3(MyCards[0], MyCards[1], MyCards[2]));
        float Yourdistance = Vector3.Magnitude(fallpoint - new Vector3(YourCards[0], YourCards[1], YourCards[2]));
        float landingtime = Mathf.Min((Mydistance - _BlackJackManager.LeftAmountOfMove * 1.5f * 0.1f) / _BlackJackManager.LeftAmountOfMove, (Yourdistance - _BlackJackManager.RightAmountOfMove * 1.5f * 0.1f) / _BlackJackManager.RightAmountOfMove) + _BlackJackManager.FlyAffordTime;
        //float landingtime = _BlackJackManager.FlyAffordTime;
        Vector3 initialVelocity = GetInitialVelocityfromfallpoint(fallpoint, launchpoint, landingtime);

        //Vector3 initialVelocity = new Vector3(Mathf.Sign(UnityEngine.Random.Range(-1f, 1f)) * GetRandomFloatValue(0,1,8,10), -UnityEngine.Random.Range(12f, 17f), UnityEngine.Random.Range(27f, 30f));
        //Vector3 initialVelocity = new Vector3(Mathf.Sign(UnityEngine.Random.Range(-1f, 1f)) * UnityEngine.Random.Range(0.0f, 4f), UnityEngine.Random.Range(18f, 20f), UnityEngine.Random.Range(10f, 15f));
        //Vector3 initialAngularVelocity = CalculateInitialAngularVelocity(initialVelocity);
        //Vector3 initialAngularVelocity = (-1) * new Vector3(0, Mathf.Sign(initialVelocity.x) * GetRandomFloatValue(-0.2f, 0.5f, 1.5f, 1.8f), 0);
        //Vector3 initialAngularVelocity = (-1) * new Vector3(0, Mathf.Sign(initialVelocity.x) * GetRandomFloatValue(-200f, 200f, 3000f, 3500f), 0);
        Vector3 initialAngularVelocity = Vector3.zero;
        //Vector3 initialAngularVelocity = new Vector3(0, 0, 0);
        //Vector3 initialPos = new Vector3((-1) * Mathf.Sign(initialVelocity.x) * UnityEngine.Random.Range(-1f, 3f), 0, -10);
        //Vector3 initialPos = new Vector3((-1) * Mathf.Sign(initialVelocity.x) * UnityEngine.Random.Range(-1f, 5f), 18, -10);
        //List<float> landingpoint = PredictLandingPoint(new Vector3(0, 0, -10), initialVelocity, initialAngularVelocity);
        //List<float> landingpoint = PredictLandingPoint(initialPos, initialVelocity, initialAngularVelocity);
        //Debug.Log(landingpoint.x);
        //FieldCards = new List<float>() { initialPos.x, initialPos.y, initialPos.z, initialVelocity.x, initialVelocity.y, initialVelocity.z, initialAngularVelocity.x, initialAngularVelocity.y, initialAngularVelocity.z, landingpoint[0], landingpoint[1], landingpoint[2], landingpoint[3] };
        FieldCards = new List<float>() { launchpoint.x, launchpoint.y, launchpoint.z, initialVelocity.x, initialVelocity.y, initialVelocity.z, initialAngularVelocity.x, initialAngularVelocity.y, initialAngularVelocity.z, fallpoint.x, fallpoint.y, fallpoint.z, landingtime };
        /*
        MyCards = CardPattern.MyCardPattern[_order];
        YourCards = CardPattern.YourCardPattern[_order];
        MyCardsSuit = CardPattern.MyCardPatternSuit[_order];
        YourCardsSuit = CardPattern.YourCardPatternSuit[_order];
        //FieldCards = CardPattern.FieldCardPattern[_order];
        FieldCardsSuit = CardPattern.FieldCardPatternSuit[_order];*/
    }
    Vector3 GetInitialVelocityfromfallpoint(Vector3 _fallpoint, Vector3 _launchpoint, float _landingtime)
    {

        // 重力加速度（単位：m/s^2）
        float gravity = Physics.gravity.y;

        // 水平方向（XZ平面）の距離
        Vector3 horizontalDistance = new Vector3(_fallpoint.x - _launchpoint.x, 0, _fallpoint.z - _launchpoint.z);

        // 水平方向の初期速度
        Vector3 horizontalVelocity = horizontalDistance / _landingtime;

        // Y方向（垂直方向）の初期速度を計算する
        float verticalDistance = _fallpoint.y - _launchpoint.y;

        // 垂直方向の運動方程式を使って初期速度を計算（v0 = (y - 1/2 * g * t^2) / t）
        float verticalVelocity = (verticalDistance - 0.5f * gravity * Mathf.Pow(_landingtime, 2)) / _landingtime;

        // 最終的な初期速度ベクトル
        return new Vector3(horizontalVelocity.x, verticalVelocity, horizontalVelocity.z);

    }
    float GetRandomFloatValue(float _minmin, float _minmax, float _maxmin, float _maxmax)
    {
        float randomValue;

        // 0か1をランダムに選ぶ
        int rangeSelector = UnityEngine.Random.Range(0, 2);

        if (rangeSelector == 0)
        {
            // 1から2の範囲からランダムなfloat値を生成
            randomValue = UnityEngine.Random.Range(_minmin, _minmax);
        }
        else
        {
            // 3から4の範囲からランダムなfloat値を生成
            randomValue = UnityEngine.Random.Range(_maxmin, _maxmax);
        }

        return randomValue;
    }
    Vector3 CalculateInitialAngularVelocity(Vector3 velocity)
    {
        Vector3 angularVelocity = Vector3.zero;
        if (velocity.x > 0)
        {
            // x軸の負の方向に力を加えるために必要な角速度を設定
            angularVelocity = Vector3.up * velocity.magnitude;
        }
        else
        {
            // x軸の正の方向に力を加えるために必要な角速度を設定
            angularVelocity = Vector3.down * velocity.magnitude;
        }

        // さらにy方向の速度に基づいて調整
        if (velocity.y < 0)
        {
            angularVelocity += Vector3.right * velocity.magnitude;
        }
        else
        {
            angularVelocity += Vector3.left * velocity.magnitude;
        }

        return angularVelocity;
    }


    List<float> PredictLandingPoint(Vector3 initialPosition, Vector3 initialVelocity, Vector3 initialAngularVelocity)
    {

        Vector3 currentPosition = initialPosition;
        Vector3 currentVelocity = initialVelocity;
        Vector3 gravity = Physics.gravity;
        Vector3 currentAngularVelocity = initialAngularVelocity;
        float landingtime = 0;

        // Calculate Magnus effect
        Vector3 magnusForce = Vector3.Cross(currentAngularVelocity, currentVelocity) * 0.1f; // 0.1f is a magnus effect coefficient

        // Update velocity with gravity and Magnus effect
        currentVelocity += (gravity + magnusForce) * Time.fixedDeltaTime;
        //currentVelocity += gravity * timestep;
        currentPosition += currentVelocity * Time.fixedDeltaTime;
        landingtime += Time.fixedDeltaTime;
        while (currentPosition.y > 0)
        {
            // Calculate Magnus effect
            magnusForce = Vector3.Cross(currentAngularVelocity, currentVelocity) * 0.1f; // 0.1f is a magnus effect coefficient

            // Update velocity with gravity and Magnus effect
            currentVelocity += (gravity + magnusForce) * Time.fixedDeltaTime;
            //currentVelocity += gravity * timestep;
            currentPosition += currentVelocity * Time.fixedDeltaTime;
            landingtime += Time.fixedDeltaTime;
        }

        currentPosition.y = 0; // Ensure the y-coordinate is exactly 0

        return new List<float>() { currentPosition.x, currentPosition.y, currentPosition.z, landingtime };
    }

    void DecidingCards(int _j)
    {
        DecideRandomCards();
        while (CheckDoubleCard())
        {
            DecideRandomCards();
        }
    }

    void DecideCards(int _j)
    {
        /*MyCards = new List<float>();
        YourCards = new List<float>();
        MyCardsSuit = new List<float>();
        YourCardsSuit = new List<float>();
        //FieldCards = UnityEngine.Random.Range(1, 14);
        //FieldCardsSuit = UnityEngine.Random.Range(0, 4);
        int _targetSum = 10;
        if (_j > 0)
        {
            for (int i = 0; i < _j; i++)
            {
                int card = UnityEngine.Random.Range(1, 14);
                while (ValidityCheck(_targetSum, card, MyCards))
                {
                    card = UnityEngine.Random.Range(1, 14);
                }
                MyCards.Add(card);
                YourCards.Add(_targetSum - card);
                MyCardsSuit.Add(UnityEngine.Random.Range(0, 4));
                YourCardsSuit.Add(UnityEngine.Random.Range(0, 4));
            }
        }
        if (_j < NumberofCards)
        {
            for (int i = 0; i < NumberofCards - _j; i++)
            {
                int mycard = UnityEngine.Random.Range(1, 14);
                int yourcard = UnityEngine.Random.Range(1, 14);
                while (ValidityCheck_remaining(_targetSum, mycard, yourcard, MyCards, YourCards))
                {
                    mycard = UnityEngine.Random.Range(1, 14);
                    yourcard = UnityEngine.Random.Range(1, 14);
                }
                MyCards.Add(mycard);
                YourCards.Add(yourcard);
                MyCardsSuit.Add(UnityEngine.Random.Range(0, 4));
                YourCardsSuit.Add(UnityEngine.Random.Range(0, 4));
            }
        }
        ShuffleCards();*/
    }
    void DecideRandomCards()
    {
        /*MyCards = new List<int>();
        YourCards = new List<int>();
        MyCardsSuit = new List<int>();
        YourCardsSuit = new List<int>();
       // FieldCards = UnityEngine.Random.Range(1, 14);
        //FieldCards = 6;
        FieldCardsSuit = UnityEngine.Random.Range(0, 4);
        for (int i = 0; i < 5; i++)
        {
            MyCards.Add(UnityEngine.Random.Range(1, 14));
            YourCards.Add(UnityEngine.Random.Range(1, 14));
            //MyCards.Add(i + 6);
            //YourCards.Add(i + 5);
            MyCardsSuit.Add(UnityEngine.Random.Range(0, 4));
            YourCardsSuit.Add(UnityEngine.Random.Range(0, 4));
        }
        ShuffleCards();*/
    }
    private bool CheckDoubleCard()
    {
        /*var combinedList = new List<(int, int)>();

        // MyCards と MyCardsSuit の組み合わせを追加
        for (int i = 0; i < MyCards.Count; i++)
        {
            combinedList.Add((MyCards[i], MyCardsSuit[i]));
        }

        // YourCards と YourCardsSuit の組み合わせを追加
        for (int i = 0; i < YourCards.Count; i++)
        {
            combinedList.Add((YourCards[i], YourCardsSuit[i]));
        }

        // FieldCards と FieldCardsSuit の組み合わせを追加
        //combinedList.Add((FieldCards, FieldCardsSuit));

        // 重複があるかチェック
        return combinedList.GroupBy(x => x).Any(g => g.Count() > 1);*/
        return true;
    }
    bool CheckmorethanfourCards()
    {
        bool Result = false;
        for (int k = 1; k < 14; k++)
        {
            int number = 0;
            //if (FieldCards == k) number++;
            foreach (var i in MyCards) if (i == k) number++;
            foreach (var i in YourCards) if (i == k) number++;
            if (number > 4) Result = true;
        }
        return Result;
    }
    bool ValidityCheck(int _targetSum, int card, List<int> _MyCard)
    {
        bool Result = false;
        if (_targetSum <= card) Result = true;
        if (_targetSum - card > 13) Result = true;
        foreach (var eachcard in _MyCard) if (eachcard == card) Result = true;
        return Result;
    }
    bool ValidityCheck_remaining(int _targetSum, int mycard, int yourcard, List<int> _MyCard, List<int> _YourCard)
    {
        bool Result = false;
        if (mycard + yourcard == _targetSum) Result = true;
        //foreach (var eachcard in _MyCard) if (eachcard == mycard) Result = true;
        foreach (var eachcard in _MyCard) if (yourcard + eachcard == _targetSum) Result = true;
        return Result;
    }
    void ShuffleCards()
    {
        for (int i = 0; i < MyCards.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, MyCards.Count);
            float temp = MyCards[i];
            MyCards[i] = MyCards[randomIndex];
            MyCards[randomIndex] = temp;
        }
        for (int i = 0; i < YourCards.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, YourCards.Count);
            float temp = YourCards[i];
            YourCards[i] = YourCards[randomIndex];
            YourCards[randomIndex] = temp;
        }
    }

    public void MoveToWaitForNextTrial(int _nowTrial)
    {
        _BlackJackManager.MoveToWaitForNextTrial(_nowTrial);
        _PhotonView.RPC("RPCMoveToWaitForNextTrial", RpcTarget.Others, _nowTrial);
    }
    [PunRPC]
    void RPCMoveToWaitForNextTrial(int _nowTrial)
    {
        // ここでカードデータを再構築
        _BlackJackManager.MoveToWaitForNextTrial(_nowTrial);
    }

    public void MoveToShowMyCards(int hostorClient)
    {
        _BlackJackManager.MoveToShowMyCards(0);
        _PhotonView.RPC("RPCMoveToShowMyCards", RpcTarget.Others);
    }
    [PunRPC]
    void RPCMoveToShowMyCards()
    {
        // ここでカードデータを再構築
        _BlackJackManager.MoveToShowMyCards(1);
    }

    public void MoveToSelectCards()
    {
        _BlackJackManager.MoveToSelectCards();
        _PhotonView.RPC("RPCMoveToSelectCards", RpcTarget.Others);
    }
    [PunRPC]
    void RPCMoveToSelectCards()
    {
        // ここでカードデータを再構築
        _BlackJackManager.MoveToSelectCards();
    }
    public void MoveToSelectBet()
    {
        _BlackJackManager.MoveToSelectBet();
        _PhotonView.RPC("RPCMoveToSelectBet", RpcTarget.Others);
    }
    [PunRPC]
    void RPCMoveToSelectBet()
    {
        // ここでカードデータを再構築
        _BlackJackManager.MoveToSelectBet();
    }
    public void MoveToShowResult()
    {
        _BlackJackManager.MoveToShowResult();
        _PhotonView.RPC("RPCMoveToShowResult", RpcTarget.Others);
    }
    [PunRPC]
    void RPCMoveToShowResult()
    {
        // ここでカードデータを再構築
        _BlackJackManager.MoveToShowResult();
    }
    public void MakeReadyHost()
    {
        _BlackJackManager.MakeReadyHost();
        _PhotonView.RPC("RPCMakeReadyHost", RpcTarget.Others);
    }
    [PunRPC]
    void RPCMakeReadyHost()
    {
        // ここでカードデータを再構築
        _BlackJackManager.MakeReadyHost();
    }
    public void MakeReadyClient()
    {
        _BlackJackManager.MakeReadyClient();
        _PhotonView.RPC("RPCMakeReadyClient", RpcTarget.Others);
    }
    [PunRPC]
    void RPCMakeReadyClient()
    {
        // ここでカードデータを再構築
        _BlackJackManager.MakeReadyClient();
    }
    public void Restart()
    {
        _BlackJackManager.Restart();
        _PhotonView.RPC("RPCRestart", RpcTarget.Others);
    }
    [PunRPC]
    void RPCRestart()
    {
        // ここでカードデータを再構築
        _BlackJackManager.Restart();
    }
    public void GameStartUi()
    {
        _BlackJackManager.GameStartUI();
        _PhotonView.RPC("RPCGameStartUi", RpcTarget.Others);
    }
    [PunRPC]
    void RPCGameStartUi()
    {
        // ここでカードデータを再構築
        _BlackJackManager.GameStartUI();
    }
    public void SetCharacterPos()
    {
        _BlackJackManager.SetCharacterPos();
        _PhotonView.RPC("RPCSetCharacterPos", RpcTarget.Others);
    }
    [PunRPC]
    void RPCSetCharacterPos()
    {
        // ここでカードデータを再構築
        _BlackJackManager.SetCharacterPos();
    }
    List<int> GenerateRandomList(int min, int max)
    {
        List<int> result = new List<int>();
        for (int i = min; i <= max; i++)
        {
            result.Add(i);
        }

        // シャッフル
        int n = result.Count;
        System.Random rng = new System.Random();
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            int value = result[k];
            result[k] = result[n];
            result[n] = value;
        }

        return result;
    }
}
