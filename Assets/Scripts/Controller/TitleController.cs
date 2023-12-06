using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{
    [SerializeField] Button startButton = null;
    [SerializeField] GameObject map = null;

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
        map.transform.Rotate(0, 5 * Time.deltaTime, 0);
    }
}
