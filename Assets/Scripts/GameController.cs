using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
  // set in Unity UI
  public int minSliceCount = 3;
  public int maxSliceCount = 7;
  public float sliceAtAngle = 0f;

  private Cake cake;
  private Knife knife;
  private HudController hudController;

  // gameplay state
  private float elapsedTime = 0f;
  private int score = 0;
  private int lives = 3;
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

    hudController.UpdateScoreText("slice cakes!");
    hudController.UpdateElapsedTimeText(elapsedTime);
    hudController.UpdateSlicesText(slicesLeft);
    hudController.UpdateLivesText(lives);
  }

  public void Update()
  {
    elapsedTime += Time.deltaTime;
    hudController.UpdateElapsedTimeText(elapsedTime);

    if (Input.GetKeyDown("s"))
    {
      if (!knife.IsSlicing() && !cake.ResetAnimationInProgress)
      {
        knife.TriggerSliceAnimation();
      }
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
      hudController.UpdateScoreText(score);
      ResetSliceCounters();
      cake.ResetState();
    }

    hudController.UpdateSlicesText(slicesLeft);
  }

  private void OnToppingSmashed()
  {
    lives--;
    if (lives > 0)
    {
      hudController.UpdateLivesText(lives);
      hudController.TriggerToast("smash!");
    }
    else
    {
      SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }
  }

  private void OnKnifeDown()
  {
    cake.Slice();
  }
}
