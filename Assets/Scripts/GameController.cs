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
  private int score = 0;
  private int availableSlices = -1;
  private int slicesLeft = -1;

  public void Start()
  {
    cake = FindObjectOfType<Cake>();
    knife = FindObjectOfType<Knife>();
    hudController = FindObjectOfType<HudController>();

    ResetSliceCounters();
    cake.ResetState(noAnimation: true);
    hudController.UpdateSlices(slicesLeft);

    cake.CakeSliced += () =>
    {
      slicesLeft--;
      if (slicesLeft == 0)
      {
        var scoreChange = cake.GetScoreChange(availableSlices);
        if (scoreChange > 95)
        {
          hudController.TriggerToast("perfect!");
        }
        
        score += scoreChange;
        hudController.UpdateScore(score);
        ResetSliceCounters();
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

  private void ResetSliceCounters()
  {
    availableSlices = slicesLeft = Random.Range(minSliceCount, maxSliceCount);
  }
}
