using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;  

public class UICountDown : MonoBehaviour
{
    [SerializeField]
    private GameObject CountPanel;
    [SerializeField]
    private GameObject CountTextObject;

    int countTime = 3;

    // Update is called once per frame
    void Start()
    {
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        TMP_Text CountText = CountTextObject.GetComponent<TMP_Text>();
        
        for (int i = 0; i < 3; i++)
        {
            CountText.text = countTime.ToString();
            CountTextObject.transform.DOScale(new Vector3(0, 0, 0), 1).From().SetEase(Ease.OutBounce);
            yield return new WaitForSeconds(1);
            countTime--;
        }
        GameSystem.Instance.IsGamestart = true;
        CountPanel.SetActive(false);
    }
}
