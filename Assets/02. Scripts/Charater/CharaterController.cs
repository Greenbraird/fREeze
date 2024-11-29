using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaterController : MonoBehaviour
{
    [SerializeField]
    private CharacterInputHandler charaterInputHanbler;
    [SerializeField]
    private CharaterMovement charaterMovement;

    bool touchable;
 
    void Start()
    {
        //charaterInputHanbler.OnRightSlide += HandleRightSlide;
        //charaterInputHanbler.OnLeftSlide += HandleLeftSlide;
        //charaterInputHanbler.OnUpSlide += HandleUpSlide;
        //charaterInputHanbler.OnDownSlide += HandleDownSlide;

        touchable = GameSystem.Instance.touchable;  
    }

    void HandleRightSlide()
    {
        if (GameSystem.Instance.touchable)
        {
            StartCoroutine(charaterMovement.MoveRight());
        }
    }

    void HandleLeftSlide()
    {
        if (GameSystem.Instance.touchable)
        {
            StartCoroutine(charaterMovement.MoveLeft());
        }
    }
    void HandleUpSlide()
    {
        if (GameSystem.Instance.touchable)
        {
            StartCoroutine(charaterMovement.Jumping());
        }
    }

    void HandleDownSlide()
    {
        if (GameSystem.Instance.touchable)
        {
            StartCoroutine(charaterMovement.Sliding());
        }
    }
}
