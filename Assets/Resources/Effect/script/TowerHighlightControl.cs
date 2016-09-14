using UnityEngine;
using System.Collections;

public class TowerHighlightControl : MonoBehaviour {

    enum TowerUltiStatus
    {
        FADEIN,
        FADEOUT,
        QUIET,
        ULTI
    };

    public float fadeInTime = 5.0f;
    public float fadeoutTime = 5.0f;
    public float ultiStayTime = 5.0f;
    public float mainColorAdjustMin = 1.0f;
    public float mainColorAdjustMax = 2.0f;
    public float outlineColorAdjustMin = 1.0f;
    public float outlineColorAdjustMax = 1.5f;
    public float outlineAlphaMin = 0.0f;
    public float outlineAlphaMax = 1.0f;

    public Texture2D towerTexture;
    public Color outlineColor = Color.white;
    public float outlineWidth = 1.0f;

    public bool pressForUltiTest = false;

    float statusTime;
    TowerUltiStatus status;

    void Start()
    {
        status = TowerUltiStatus.QUIET;
        statusTime = 0;
        GetComponent<Renderer>().material.SetTexture("_MainTex", towerTexture);
        GetComponent<Renderer>().material.SetFloat("_IntensityAdjust", mainColorAdjustMin);
        GetComponent<Renderer>().material.SetFloat("_OutlineIntensityAdjust", outlineColorAdjustMin);
        GetComponent<Renderer>().material.SetFloat("_OutlineAlpha", outlineAlphaMin);
        GetComponent<Renderer>().material.SetColor("_OutlineColor", outlineColor);
        GetComponent<Renderer>().material.SetFloat("_OutlineWidth", outlineWidth);
    }

    void Ulti()
    {
        status = TowerUltiStatus.FADEIN;
        statusTime = 0;

        StartCoroutine(StayHigh(fadeInTime));
    }

    IEnumerator StayHigh(float fadeInTime)
    {
        yield return new WaitForSeconds(fadeInTime);

        status = TowerUltiStatus.ULTI;
        statusTime = 0;

        StartCoroutine(FadeOut(ultiStayTime));
    }

    IEnumerator FadeOut(float ultiStayTime)
    {
        yield return new WaitForSeconds(ultiStayTime);

        status = TowerUltiStatus.FADEOUT;
        statusTime = 0;

        StartCoroutine(Recover(fadeoutTime));
    }

    IEnumerator Recover(float fadeoutTime)
    {
        yield return new WaitForSeconds(fadeoutTime);

        status = TowerUltiStatus.QUIET;
        statusTime = 0;

        GetComponent<Renderer>().material.SetTexture("_MainTex", towerTexture);
        GetComponent<Renderer>().material.SetFloat("_IntensityAdjust", mainColorAdjustMin);
        GetComponent<Renderer>().material.SetFloat("_OutlineIntensityAdjust", outlineColorAdjustMin);
        GetComponent<Renderer>().material.SetFloat("_OutlineAlpha", outlineAlphaMin);
        GetComponent<Renderer>().material.SetColor("_OutlineColor", outlineColor);
        GetComponent<Renderer>().material.SetFloat("_OutlineWidth", outlineWidth);
    }

    // Update is called once per frame
    void Update ()
    {
        statusTime += Time.deltaTime;

        if (pressForUltiTest)
        {
            pressForUltiTest = false;
            Ulti();
        }

        if (status == TowerUltiStatus.FADEIN)
        {
            AdjustMaterial(statusTime, fadeInTime, "_IntensityAdjust", mainColorAdjustMin, mainColorAdjustMax);
            AdjustMaterial(statusTime, fadeInTime, "_OutlineIntensityAdjust", outlineColorAdjustMin, outlineColorAdjustMax);
            AdjustMaterial(statusTime, fadeInTime, "_OutlineAlpha", outlineAlphaMin, outlineAlphaMax);
        }
        else if (status == TowerUltiStatus.FADEOUT)
        {
            AdjustMaterial(statusTime, fadeoutTime, "_IntensityAdjust", mainColorAdjustMax, mainColorAdjustMin);
            AdjustMaterial(statusTime, fadeoutTime, "_OutlineIntensityAdjust", outlineColorAdjustMax, outlineColorAdjustMin);
            AdjustMaterial(statusTime, fadeoutTime, "_OutlineAlpha", outlineAlphaMax, outlineAlphaMin);
        }
	}

    void AdjustMaterial(float time, float period, string materialProperty, float beginValue, float endValue)
    {
        GetComponent<Renderer>().material.SetFloat(materialProperty, time / period * (endValue - beginValue) + beginValue);
    }
}
