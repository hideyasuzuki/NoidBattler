using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{
    [SerializeField] Button startButton = null;

    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(OnClickStartButton);
    }
    public void OnClickStartButton()
    {
        StartCoroutine(ClickStart());
    }

    IEnumerator ClickStart()
    {
        FadeManager.Instance.FadeOut();
        while (FadeManager.Instance.IsFadeOut)
        {
            yield return null;
        }
        SceneManager.LoadScene("GameScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
