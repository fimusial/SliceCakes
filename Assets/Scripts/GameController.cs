using UnityEngine;

public class GameController : MonoBehaviour
{
  public float sliceThrottlerTimeSeconds = 0.2f;

  private Cake cake;
  private Knife knife;

  public void Start()
  {
    cake = FindObjectOfType<Cake>();
    knife = FindObjectOfType<Knife>();
  }

  public void Update()
  {
    if (Input.GetKeyDown("s"))
    {
      if (!knife.IsSlicingOrInTransition())
      {
        knife.TriggerSliceAnimation();
        cake.Slice();
      };
    }

    if (Input.GetKeyDown("r"))
    {
      cake.ResetSlices();
    }
  }
}
