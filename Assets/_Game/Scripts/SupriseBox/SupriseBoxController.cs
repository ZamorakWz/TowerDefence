using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupriseBoxController : MonoBehaviour
{
    private SupriseBoxManager supriseBoxManager;

    private void Awake()
    {
        supriseBoxManager = FindObjectOfType<SupriseBoxManager>();
    }

    public void OnClickedToSupriseBox()
    {
        Time.timeScale = 0.25f;

        //make indicator somehow?

        Destroy(gameObject);
    }
}