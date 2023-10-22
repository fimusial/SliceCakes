using UnityEngine;

public class GameController : MonoBehaviour
{
  public int minSliceCount = 3;
  public int maxSliceCount = 7;
  public float sliceAtAngle = 0f;

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
    cake.ResetState(noAnimation: true);
    hudController.UpdateSlices(slicesLeft);

    cake.CakeSliced += () =>
    {
      slicesLeft--;
      if (slicesLeft == 0)
      {
        ResetSlicesLeft();
        cake.ResetState();
      }
      hudController.UpdateSlices(slicesLeft);
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
      if (!knife.IsSlicing() && !cake.ResetAnimationInProgress)
      {
        knife.TriggerSliceAnimation();
        cake.Slice();
      };
    }

    cake.SliceAtAngle = sliceAtAngle;
    knife.SliceAtAngle = sliceAtAngle;
  }

  private void ResetSlicesLeft()
  {
    slicesLeft = Random.Range(minSliceCount, maxSliceCount);
  }
}
