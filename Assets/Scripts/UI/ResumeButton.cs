using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResumeButton : MonoBehaviour
{
    private Image buttonImage;

    [SerializeField] private Sprite buttonClickSpr;

    void Start()
    {
        buttonImage = gameObject.GetComponent<Image>();

        gameObject.GetComponent<Button>().onClick.AddListener(ClickButton);
    }

    private void ClickButton()
    {
        buttonImage.sprite = buttonClickSpr;
    }
}
