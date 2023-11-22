using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : SingletonMonoBehaviour<FadeManager>
{
    protected override bool dontDestroyOnLoad { get { return true; } }

    [SerializeField] Image fadeImage = null;   // 透明度を変更するパネルのイメージ
    float fadeSpeed = 0.5f;             // 透明度が変わるスピードを管理
    float red, green, blue, alpa;       // パネルの色、不透明度を管理

    bool isFadeIn = true;      // FadeInになったかを判定
    public bool IsFadeIn
    {
        get { return isFadeIn; }
    }
    bool isFadeOut = false;     // FadeOutになったかを判定
    public bool IsFadeOut
    {
        get { return isFadeOut; }
    }

    void Start()
    {
        red = fadeImage.color.r;
        green = fadeImage.color.g;
        blue = fadeImage.color.b;
        alpa = fadeImage.color.a;
    }

    void Update()
    {
        if (isFadeIn)
        {
            FadeIn();
        }
    }

    void FadeIn()
    {
        StartCoroutine(FadeInColor());
    }

    // 他のスクリプトでFadeOutする為
    public void FadeOut()
    {
        StartCoroutine(FadeOutColor());
    }

    // カラーの初期化
    void InitColor()
    {
        fadeImage.color = new Color(red, green, blue, alpa);
    }


    IEnumerator FadeInColor()
    {
        fadeImage.enabled = true;
        alpa -= fadeSpeed * Time.deltaTime;
        InitColor();
        if (alpa <= 0)
        {
            isFadeOut = true;
            isFadeIn = false;

            fadeImage.enabled = false;
            yield return null;
        }
    }

    IEnumerator FadeOutColor()
    {
        while (true)
        {
            fadeImage.enabled = true;
            alpa += fadeSpeed * Time.deltaTime;
            InitColor();
            if (alpa >= 1)
            {
                isFadeIn = true;
                isFadeOut = false;

                yield break;
            }
            yield return null;
        }
    }
}

