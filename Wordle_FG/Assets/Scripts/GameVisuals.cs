using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameVisuals : MonoBehaviour
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
    public void PlayPressEffect(bool reversed = false)
    {
        if (pressedEffect != null)
        {
            StopCoroutine(pressedEffect);
        }
        pressedEffect = StartCoroutine(PressEffect(reversed));
    }

    IEnumerator PressEffect(bool reversed = false)
    {
        Vector3 endScale = startingScale * pressedEffectEndScale;
        endScale.z = startingScale.z;
        if (reversed)
        {
            yield return PerformingScaleChange(endScale, startingScale, pressedEffectSpeed);
            yield return PerformingScaleChange(startingScale, endScale, pressedEffectSpeed);
        }
        else
        {
            yield return PerformingScaleChange(startingScale, endScale, pressedEffectSpeed);
            yield return PerformingScaleChange(endScale, startingScale, pressedEffectSpeed);
            
        }
        pressedEffect = null;
    }

    public void PlayIncreaseScale()
    {
        StopScaleCorotutines();
        onHoverEnter = StartCoroutine(IncreasingScale());
    }
    
    IEnumerator IncreasingScale()
    {
        Vector3 endScale = startingScale * hoverEffectEndScale;
        yield return PerformingScaleChange(startingScale, endScale, pressedEffectSpeed);
        onHoverEnter = null;
    }
    public void PlayDescreaseScale()
    {
        StopScaleCorotutines();
        onHoverExit = StartCoroutine(DecreasingScale());
    }
    
    IEnumerator DecreasingScale()
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

    public void StopScaleCorotutines()
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
        
        
        UpdateColor(newColor);
        actionInBetween?.Invoke();
        
        while (percent>0)
        {
            percent -= dissolvingSpeed * Time.deltaTime;
            PerformDissolveEffect(percent);
            yield return new WaitForEndOfFrame();
        }

        changingColor = null;
    }

    public void UpdateColor(Color newColor)
    {
        material.SetColor("_BaseColor", newColor);
    }

    public void PerformDissolveEffect(float percent,float minPercent = -1.5f, float maxPercent = 0.5f)
    {
        // float maxPercent = 0.5f;
        // float minPercent = -1.5f;
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
