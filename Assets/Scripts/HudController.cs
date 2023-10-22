using System.Collections;
using TMPro;
using UnityEngine;

public class HudController : MonoBehaviour
{
    // set in Unity UI
    public TMP_Text textScore;
    public TMP_Text textSlices;
    public TMP_Text textToast;

    public void Start()
    {
        textScore.text = "SLICE CAKES!";
        textSlices.text = string.Empty;
        textToast.text = string.Empty;
    }

    public void UpdateScore(int score)
    {
        textScore.text = score.ToString();
    }

    public void UpdateSlices(int score)
    {
        textSlices.text = score.ToString();
    }

    public void TriggerToast(string message)
    {
        StopCoroutine(nameof(ToastCoroutine));
        textToast.text = message;
        StartCoroutine(nameof(ToastCoroutine));
    }

    private IEnumerator ToastCoroutine()
    {
        textToast.alpha = 1f;
        var yIncrement = Screen.height / 256f;

        textToast.rectTransform.transform.SetPositionAndRotation(
            new Vector3(Screen.width / 2f, Screen.height / 2f, 0f),
            Quaternion.identity);

        for (int i = 1; i <= 64; i++)
        {
            yield return new WaitForSeconds(1f / 64f);
            textToast.rectTransform.Translate(0f, yIncrement, 0f);
            textToast.alpha = 1f - i * 1f / 64f;
        }
    }
}
