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

    public GameObject speedanimationobject;

    // Update is called once per frame
    void Start()
    {
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        TMP_Text CountText = CountTextObject.GetComponent<TMP_Text>();
        
        for (int i = 3; i> -1; i--)
        {
            if(i != 0){ 
                CountText.text = i.ToString();
                AudioManager.Instance.SFXPlay(gameObject, 3);
            }
            else {
                CountText.text = "Run!";
                AudioManager.Instance.SFXPlay(gameObject, 4);
                speedanimationobject.SetActive(true);
            }
            
            CountTextObject.transform.DOScale(new Vector3(0, 0, 0), 1).From().SetEase(Ease.OutBounce).OnComplete(()=>
            {
                //카운드가 거의 끝나고 GameSystem 값 초기화
                if (i == 0)
                {
                    GameSystem.Instance.touchable = true;
                    GameSystem.Instance.IsGamestart = true;
                }
            });
            yield return new WaitForSeconds(1);

        }
        CountPanel.SetActive(false);
    }
}
