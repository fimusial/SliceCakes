using UnityEngine;

public class GameController : MonoBehaviour
{
  private Cake cake;
  private Knife knife;
  private HudController hudController;

  public void Start()
  {
    cake = FindObjectOfType<Cake>();
    knife = FindObjectOfType<Knife>();
    hudController = FindObjectOfType<HudController>();

    // sample
    cake.CakeReset += () => hudController.UpdateScore((uint)Time.frameCount);
    cake.CakeSliced += () => hudController.TriggerToast("slice");
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
