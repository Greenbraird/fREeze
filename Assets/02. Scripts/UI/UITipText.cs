using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITipText : MonoBehaviour
{

    public TMP_Text text;
    [SerializeField]
    private string[] tiptext = new string[5];
    void Start()
    {       
        text = GetComponent<TMP_Text>();

        text.text = "Tip! " + tiptext[Random.Range(0, tiptext.Length)];
    }
}
