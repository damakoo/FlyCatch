using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Text.RegularExpressions;
using System.Linq;


public class PracticeSet: MonoBehaviourPunCallbacks
{
    BlackJackManager _BlackJackManager;
    private PhotonView _PhotonView;
    public int MySelectedCard { get; set; }
    public int YourSelectedCard { get; set; }
    public List<float> MySelectedTime { get; set; }
    public List<float> YourSelectedTime { get; set; }
    public int MySelectedBet { get; set; }
    public int YourSelectedBet { get; set; }
    public float TimeLeft { get; set; } = 0;
    public bool HostPressed { get; set; } = false;
    public bool ClientPressed { get; set; } = false;
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
    public void SetHostPlayerPos(float _hostplayerpos_x, float _hostplayerpos_y, float _hostplayerpos_z)
    {
        HostPlayerPos = new Vector3(_hostplayerpos_x, _hostplayerpos_y, _hostplayerpos_z);
        _PhotonView.RPC("UpdateSetHostPlayerPos", RpcTarget.Others, _hostplayerpos_x, _hostplayerpos_y, _hostplayerpos_z);
    }
    [PunRPC]
    void UpdateSetHostPlayerPos(float _hostplayerpos_x, float _hostplayerpos_y, float _hostplayerpos_z)
    {
        // ここでカードデータを再構築
        HostPlayerPos = new Vector3(_hostplayerpos_x,_hostplayerpos_y,_hostplayerpos_z);
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
        _PhotonView.RPC("UpdateFieldCardsPracticeListOnAllClients", RpcTarget.Others, SerializeCardList(_FieldCardsPracticeList));
    }
    [PunRPC]
    void UpdateFieldCardsPracticeListOnAllClients(string serializeCards)
    {
        // ここでカードデータを再構築
        FieldCardsPracticeList = DeserializeFieldCardList(serializeCards);
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
        for(int i = 0; i<NumberofSet; i++)
        {
            List<float> Element = new List<float>();
            for(int j = 0; j < NumberofCards; j++)
            {
                Element.Add(numbers[i* NumberofCards + j]);
            }
            cardList.Add(Element);
        }
        return cardList;
    }
    private List<List<float>> DeserializeFieldCardList(string json)
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
            for (int j = 0; j < 12; j++)
            {
                Element.Add(numbers[i * 12 + j]);
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
            DecideDecidedCards(_order[i]-1);
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
            DecideDecidedCards(_order[i]-1);
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
        int result = Random.Range(0, 4);
        while(result == 1)
        {
            result = Random.Range(0, 4);
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
        MyCards = new List<float>() { -22, 0, 16 };
        YourCards = new List<float>() { 22, 0, 16 };
        FieldCards = new List<float>() { 0, 0, 0, 0, 100, 100, 0, 0, 100, 0, 0, 16 };
        /*
        MyCards = CardPattern.MyCardPattern[_order];
        YourCards = CardPattern.YourCardPattern[_order];
        MyCardsSuit = CardPattern.MyCardPatternSuit[_order];
        YourCardsSuit = CardPattern.YourCardPatternSuit[_order];
        //FieldCards = CardPattern.FieldCardPattern[_order];
        FieldCardsSuit = CardPattern.FieldCardPatternSuit[_order];*/
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
