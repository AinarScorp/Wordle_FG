using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class LetterBlock : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 5.0f;

    [SerializeField] TextMeshPro displayLetter;

    [SerializeField] MeshRenderer meshRenderer;
    Material material;
    char chosenLetter;
    int thisRow, letterPosition;
    Coroutine spinningCoroutine;

    Color updatedColor;
    public char ChosenLetter => chosenLetter;

    public int LetterPosition => letterPosition;

    void Awake()
    {
        AssignMaterial();
        if (displayLetter == null)
        {
            displayLetter = GetComponentInChildren<TextMeshPro>();
        }
    }

    void AssignMaterial()
    {
        if (meshRenderer == null)
        {
            meshRenderer = GetComponentInChildren<MeshRenderer>();
        }

        if (material == null)
        {
            material = meshRenderer.material;
        }

        meshRenderer.material = material;
    }

    public void GetUpdatedColor(Color newColor)
    {
        updatedColor = newColor;
    }
    void UpdateColor()
    {
        //material.color = updatedColor;
        material.SetColor("_BaseColor", updatedColor);

    }
    public void UpdateLetter(char newLetter)
    {
        chosenLetter = Char.ToUpper(newLetter);
        displayLetter.text = chosenLetter.ToString();
    }

    void UpdateLetterOrientation()
    {
        float dotProduct = Vector3.Dot(transform.forward, Vector3.forward);
        Vector3 localScale = displayLetter.transform.localScale;
        float startingSign =Mathf.Sign(localScale.y);
        float sign = Mathf.Sign(dotProduct);
        localScale.y = sign;

        PerformDissolveEffect(sign, dotProduct);
        displayLetter.transform.localScale = localScale;

        //Update sign when flipped
        if ((int)startingSign !=(int)sign)
        {
            UpdateColor();
        }
    }

    void PerformDissolveEffect(float sign, float dotProduct)
    {
        float maxPercent = 0.5f;
        float minPercent = -1.5f;
        float percentSlider = sign > 0 ? Mathf.Lerp(minPercent, maxPercent, 1 - dotProduct) : Mathf.Lerp(maxPercent, minPercent, -dotProduct);
        material.SetFloat("_Percent", percentSlider);
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
