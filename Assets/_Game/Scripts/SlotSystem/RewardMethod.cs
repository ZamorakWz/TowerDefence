using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class RewardMethod : MonoBehaviour
{
    private SlotManager slotmanager;
    public string matchingObject;
    public int matchLength;
    public Dictionary<string, int> resultSpin;
    void Start()
    {
        slotmanager = gameObject.GetComponent<SlotManager>();
    }

    public void CheckForHorizontalMatches()
    {
        resultSpin = new Dictionary<string, int>();
        //Horizontals
        for (int i = 0; i < slotmanager.boardHeight; i++)
        {

            for (int j = 0; j < slotmanager.boardWidth; j++)
            {
                matchingObject = slotmanager._gameBoard[i,j].name;
                slotmanager._matchingObjects.Add(matchingObject);
            }
        }
        foreach(string item in slotmanager._matchingObjects)
        {
            if(!resultSpin.ContainsKey(item))
            {
                resultSpin.Add(item,1);
            }
            else
            {
                matchLength = 0;
                resultSpin.TryGetValue(item, out matchLength);
                resultSpin.Remove(item);
                resultSpin.Add(item, matchLength+1);
            }
        }
        foreach(KeyValuePair<string, int> entry in resultSpin)
        {
            if(entry.Value >= 3)
            {
                slotmanager._paycondition.Add(entry.Key + " x " + entry.Value);
            }
        }
    }
}
