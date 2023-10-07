using UnityEngine;

public class GameController : MonoBehaviour
{
  public int minSliceCount = 3;
  public int maxSliceCount = 7;

  private Cake cake;
  private Knife knife;
  private HudController hudController;

  // gameplay state
  private int slicesLeft = -1;

  public void Start()
  {
    cake = FindObjectOfType<Cake>();
    knife = FindObjectOfType<Knife>();
    hudController = FindObjectOfType<HudController>();

    ResetSlicesLeft();
    cake.ResetSlicesAndToppings();
    hudController.UpdateScore(slicesLeft);

    cake.CakeSliced += () =>
    {
      slicesLeft--;
      if (slicesLeft == 0)
      {
        ResetSlicesLeft();
        cake.TriggerResetTransition();
      }
      hudController.UpdateScore(slicesLeft);
    };

    cake.ToppingSmashed += () =>
    {
      hudController.TriggerToast("smash!");
    };
  }

  public void Update()
  {
    if (Input.GetKeyDown("s"))
    {
      if (!knife.IsSlicing() && !cake.ResetTransitionInProgress)
      {
        knife.TriggerSliceAnimation();
        cake.Slice();
      };
    }
  }

  private void ResetSlicesLeft()
  {
    slicesLeft = Random.Range(minSliceCount, maxSliceCount);
  }
}
