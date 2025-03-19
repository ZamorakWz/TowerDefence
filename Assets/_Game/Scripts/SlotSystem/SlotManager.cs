using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    private RewardMethod rewardMethod;
    private SlotRewards slotRewards;
    public int boardHeight, boardWidth;
    public GameObject[] gamePieces;
    private GameObject _board;
    public GameObject[,] _gameBoard;
    [SerializeField]
    private int bet;
    public Vector3 _offset = new Vector3(0, 0, 1);
    public List<string> _matchingObjects;
    public List<string> _paycondition;
    void Start()
    {
        rewardMethod = gameObject.GetComponent<RewardMethod>();
        slotRewards = gameObject.GetComponent<SlotRewards>();
        _gameBoard = new GameObject[boardHeight, boardWidth];
        _board = GameObject.Find("GameBoard");
        _matchingObjects = new List<string>();
    }
    
    public void Spin()
    {
        if(GoldManager.Instance.GetCurrentGold() < bet)
        {
            Debug.Log("Yetersiz Bakiye");
        }
        else
        {
        _matchingObjects.Clear();
        _paycondition.Clear();
        GoldManager.Instance.RemoveGold(bet);       
        for (int i = 0; i< boardHeight; i++)
        {
            for (int j = 0; j < boardWidth; j++)
            {
                GameObject gridPosition = _board.transform.Find(i + " " + j).gameObject;
                if (gridPosition.transform.childCount > 0)
                {
                    GameObject destroyPiece = gridPosition.transform.GetChild(0).gameObject;
                    Destroy(destroyPiece);
                }
                GameObject pieceType = gamePieces[Random.Range(0, gamePieces.Length)];                
                GameObject thisPiece = Instantiate(pieceType, gridPosition.transform);
                thisPiece.name = pieceType.name;
                thisPiece.transform.parent = gridPosition.transform;
                _gameBoard[i, j] = thisPiece;
            }
        }
        rewardMethod.CheckForHorizontalMatches();
        slotRewards.rewardAssign();
        }
    }
}
