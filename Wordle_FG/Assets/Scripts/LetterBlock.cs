using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class LetterBlock : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 5.0f;
    [SerializeField] TextMeshPro displayLetter;

    char chosenLetter;
    int thisRow, letterPosition;
    Color updatedColor;


    Coroutine spinningCoroutine;
    GameVisuals gameVisuals;
    public char ChosenLetter => chosenLetter;
    public int LetterPosition => letterPosition;

    void Awake()
    {
        if (displayLetter == null)
        {
            displayLetter = GetComponentInChildren<TextMeshPro>();
        }

        gameVisuals = GetComponent<GameVisuals>();
    }
    

    public void GetUpdatedColor(Color newColor)
    {
        updatedColor = newColor;
    }

    public void UpdateLetter(char newLetter)
    {
        chosenLetter = Char.ToUpper(newLetter);
        displayLetter.text = chosenLetter.ToString();
    }

    void UpdateLetterOrientation()
    {
        float dotProduct = Vector3.Dot(transform.forward, Vector3.forward);
        float sign = Mathf.Sign(dotProduct);
        
        Vector3 localScale = displayLetter.transform.localScale;
        float startingSign =Mathf.Sign(localScale.y);
        localScale.y = sign;

        displayLetter.transform.localScale = localScale;
        PerformDissolveEffect(sign, dotProduct);
        
        //Update sign when flipped
        if ((int)startingSign !=(int)sign)
        {
            gameVisuals.UpdateColor(updatedColor);
        }
    }

    void PerformDissolveEffect(float sign, float dotProduct)
    {
        float percent = sign > 0 ? 1 - dotProduct : -dotProduct;
        float minPercent = sign > 0 ? -1.5f : 0.5f;
        float maxPercent = sign > 0 ? 0.5f : -1.5f;

        gameVisuals.PerformDissolveEffect(percent, minPercent, maxPercent);
    }

    public void PlayPressEffect()
    {
        gameVisuals.PlayPressEffect(false);
    }

    [ContextMenu("Spin")]
    public void Spin()
    {
        if (spinningCoroutine !=null)
        {
            return;
        }
        spinningCoroutine=  StartCoroutine(Spinning());
    }

    IEnumerator Spinning()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.AngleAxis(180,Vector3.right);

        float percent = 0f;
        while (percent<1)
        {
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, percent);
            percent += rotationSpeed * Time.deltaTime;

            UpdateLetterOrientation();
            yield return new WaitForEndOfFrame();
        }
        transform.rotation = endRotation;
        spinningCoroutine = null;
    }

    public void GetInfoLetter(int rowInfo, int letterPos)
    {
        thisRow = rowInfo;
        letterPosition = letterPos;
    }
}
