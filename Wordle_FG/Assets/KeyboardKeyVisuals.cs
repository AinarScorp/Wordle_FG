using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyboardKeyVisuals : MonoBehaviour
{
    [SerializeField] TextMeshPro letterDisplay;
    [SerializeField] float pressedEffectSpeed = 3.0f;
    [SerializeField] float pressedEffectEndScale = 0.7f;
    [SerializeField] float hoverEffectEndScale = 1.3f;
    [SerializeField] float dissolvingSpeed = 1.0f;

    Vector3 startingScale;
    
    //cached
    MeshRenderer meshRenderer;
    Material material;

    Coroutine pressedEffect;
    Coroutine onHoverEnter;
    Coroutine onHoverExit;
    Coroutine changingColor;


    void Awake()
    {
        letterDisplay = GetComponentInChildren<TextMeshPro>();
        AssignMaterial();
    }

    void Start()
    {
        startingScale = transform.localScale;
    }

    void AssignMaterial()
    {

        meshRenderer = GetComponentInChildren<MeshRenderer>();
        material = meshRenderer.material;
        meshRenderer.material = material;
    }
    public void PlayPressEffect()
    {
        if (pressedEffect != null)
        {
            StopCoroutine(pressedEffect);
        }
        pressedEffect = StartCoroutine(PressEffect());
    }
    
    IEnumerator PressEffect()
    {
        Vector3 endScale = startingScale * pressedEffectEndScale;
        endScale.z = startingScale.z;
        yield return PerformingScaleChange(startingScale, endScale, pressedEffectSpeed);
        yield return PerformingScaleChange(endScale, startingScale, pressedEffectSpeed);
        pressedEffect = null;
    }

    public void PlayOnHoverEnter()
    {
        StopHoverCorotutines();
        onHoverEnter = StartCoroutine(OnHoverEnter());
    }
    
    IEnumerator OnHoverEnter()
    {
        Vector3 endScale = startingScale * hoverEffectEndScale;
        yield return PerformingScaleChange(startingScale, endScale, pressedEffectSpeed);
        onHoverEnter = null;
    }
    public void PlayOnHoverExit()
    {
        StopHoverCorotutines();
        onHoverExit = StartCoroutine(OnHoverExit());
    }
    
    IEnumerator OnHoverExit()
    {
        Vector3 startScale = startingScale * hoverEffectEndScale;
        yield return PerformingScaleChange(startScale, startingScale, pressedEffectSpeed);
        onHoverExit = null;
    }
    IEnumerator PerformingScaleChange(Vector3 startScale, Vector3 endScale, float effectSpeed)
    {
        endScale.z = startingScale.z;
        float percent = 0;
        while (percent<1)
        {
            percent += Time.deltaTime*effectSpeed;
            transform.localScale = Vector3.Lerp(startScale,endScale,percent);
            yield return new WaitForEndOfFrame();
        }

    }

    public void StopHoverCorotutines()
    {
        if (onHoverEnter != null)
        {
            StopCoroutine(onHoverEnter);
        }
        if (onHoverExit != null)
        {
            StopCoroutine(onHoverExit);
        }
    }
    public void PlayUpdateColor(Color newColor,Action actionInBetween = null)
    {
        if (changingColor!=null)
        {
            StopCoroutine(changingColor);
        }
        changingColor=  StartCoroutine(ChangingColor(newColor,actionInBetween));
    }
    IEnumerator ChangingColor(Color newColor, Action actionInBetween)
    {
        float percent = 0f;
        while (percent<1)
        {
            percent += dissolvingSpeed * Time.deltaTime;
            PerformDissolveEffect(percent);
            yield return new WaitForEndOfFrame();
        }
        
        
        material.SetColor("_BaseColor", newColor);
        actionInBetween?.Invoke();
        
        while (percent>0)
        {
            percent -= dissolvingSpeed * Time.deltaTime;
            PerformDissolveEffect(percent);
            yield return new WaitForEndOfFrame();
        }

        changingColor = null;
    }
    
    void PerformDissolveEffect(float percent)
    {
        float maxPercent = 0.5f;
        float minPercent = -1.5f;
        float percentSlider = Mathf.Lerp(minPercent, maxPercent, percent);
        material.SetFloat("_Percent", percentSlider);
    }

    public void UpdateText(Color newColor, string newText)
    {
        letterDisplay.text = newText;
        letterDisplay.color =newColor;
    }
    public void UpdateText(Color newColor)
    {
        UpdateText(newColor, letterDisplay.text);
    }    
    public void UpdateText(string newText)
    {
        UpdateText(letterDisplay.color, newText);
    }
}
