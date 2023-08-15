using UnityEngine;

public class GameController : MonoBehaviour
{
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
      if (!knife.IsSlicing())
      {
        knife.TriggerSliceAnimation();
        cake.Slice();
      };
    }

    if (Input.GetKeyDown("r"))
    {
      cake.Reset();
    }
  }
}
