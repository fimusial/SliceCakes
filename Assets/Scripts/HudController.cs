using System.Collections;
using TMPro;
using UnityEngine;

public class HudController : MonoBehaviour
{
    // set in Unity UI
    public TMP_Text textScore;

    public void Start()
    {
        textScore.text = 0.ToString();
        StartCoroutine(nameof(SampleCoroutine));
    }

    public void Update()
    {
    }

    private IEnumerator SampleCoroutine()
    {
        for (int i = 0; i < 500; i++)
        {
            yield return new WaitForSeconds(0.05f);
            textScore.text = i.ToString();
        }
    }
}
