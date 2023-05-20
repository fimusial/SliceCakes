using UnityEngine;

public class GameController : MonoBehaviour
{
  public float sliceThrottlerTimeSeconds = 0.2f;

  private Cake cake;
  private Knife knife;
  private Throttler sliceThrottler;

  public void Start()
  {
    cake = FindObjectOfType<Cake>();
    knife = FindObjectOfType<Knife>();
    sliceThrottler = new Throttler(sliceThrottlerTimeSeconds);
  }

  public void Update()
  {
    if (Input.GetKeyDown("s"))
    {
      sliceThrottler.Run(() =>
      {
        knife.TriggerSliceAnimation();
        cake.Slice();
      });
    }

    if (Input.GetKeyDown("r"))
    {
      cake.ResetSlices();
    }
  }
}
