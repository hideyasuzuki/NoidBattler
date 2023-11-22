using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : SingletonMonoBehaviour<FadeManager>
{
    protected override bool dontDestroyOnLoad { get { return true; } }

    [SerializeField] Image fadeImage = null;   // �����x��ύX����p�l���̃C���[�W
    float fadeSpeed = 0.5f;             // �����x���ς��X�s�[�h���Ǘ�
    float red, green, blue, alpa;       // �p�l���̐F�A�s�����x���Ǘ�

    bool isFadeIn = true;      // FadeIn�ɂȂ������𔻒�
    public bool IsFadeIn
    {
        get { return isFadeIn; }
    }
    bool isFadeOut = false;     // FadeOut�ɂȂ������𔻒�
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

    // ���̃X�N���v�g��FadeOut�����
    public void FadeOut()
    {
        StartCoroutine(FadeOutColor());
    }

    // �J���[�̏�����
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

