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

    cake.CakeSliced += OnCakeSliced;
    cake.ToppingSmashed += OnToppingSmashed;
    knife.KnifeDown += OnKnifeDown;

    cake.ResetState(noAnimation: true);
    ResetSliceCounters();

    hudController.UpdateSlices(slicesLeft);
    hudController.UpdateScore("slice cakes!");
  }

  public void Update()
  {
    if (Input.GetKeyDown("s"))
    {
      if (!knife.IsSlicing() && !cake.ResetAnimationInProgress)
      {
        knife.TriggerSliceAnimation();
      };
    }

    cake.SliceAtAngle = sliceAtAngle;
    knife.SliceAtAngle = sliceAtAngle;
  }

  private void ResetSliceCounters()
  {
    availableSlices = slicesLeft = UnityEngine.Random.Range(minSliceCount, maxSliceCount);
  }

  private void OnCakeSliced()
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
  }

  private void OnToppingSmashed()
  {
    hudController.TriggerToast("smash!");
  }

  private void OnKnifeDown()
  {
    cake.Slice();
  }
}
