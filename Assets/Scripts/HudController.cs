using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class HudController : MonoBehaviour
{
  // set in Unity UI
  public TMP_Text textElapsedTime;
  public TMP_Text textScore;
  public TMP_Text textSlices;
  public TMP_Text textLives;
  public TMP_Text textToast;

  public void Start()
  {
    textElapsedTime.text = string.Empty;
    textScore.text = string.Empty;
    textSlices.text = string.Empty;
    textLives.text = string.Empty;
    textToast.text = string.Empty;
  }

  public void UpdateElapsedTimeText(float elapsedTime)
  {
    var formatted = TimeSpan.FromSeconds(elapsedTime).ToString(@"hh\:mm\:ss\.ff");
    textElapsedTime.text = formatted;
  }

  public void UpdateScoreText(int score)
  {
    textScore.text = score.ToString();
  }

  public void UpdateScoreText(string altText)
  {
    textScore.text = altText;
  }

  public void UpdateSlicesText(int score)
  {
    textSlices.text = score.ToString();
  }

  public void UpdateLivesText(int lives)
  {
    textLives.text = new string('\u2665', lives);
  }

  public void TriggerToast(string message)
  {
    StopCoroutine(nameof(ToastCoroutine));
    textToast.text = message;
    StartCoroutine(nameof(ToastCoroutine));
  }

  private IEnumerator ToastCoroutine()
  {
    const float animationTime = 0.5f;
    var yIncrement = Screen.height / 256f;
    textToast.alpha = 1f;

    textToast.rectTransform.transform.SetPositionAndRotation(
      new Vector3(Screen.width / 2f, Screen.height / 2f, 0f),
      Quaternion.identity);

    for (int i = 1; i <= 32; i++)
    {
      yield return new WaitForSeconds(animationTime / 32f);
      textToast.rectTransform.Translate(0f, yIncrement, 0f);
      textToast.alpha = 1f - i * 1f / 32f;
    }
  }
}
