using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bakiye : MonoBehaviour
{
    public double bakiye;
    // Start is called before the first frame update
    [SerializeField]
    private TextMeshProUGUI _bakiye;
    void Start()
    {
        bakiye = 1000.00;
    }

    // Update is called once per frame
    void Update()
    {
        _bakiye.text = string.Format("{0:0.00}",bakiye);
    }
}
