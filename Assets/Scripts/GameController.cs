using UnityEngine;

public class GameController : MonoBehaviour
{
  private Cake cake;
  private Knife knife;

  public void Start()
  {
    cake = FindObjectOfType<Cake>();
    knife = FindObjectOfType<Knife>();

    // debug
    cake.CakeReset += () => Debug.Log(nameof(cake.CakeReset));
    cake.CakeSliced += () => Debug.Log(nameof(cake.CakeSliced));
    cake.ToppingSmashed += () => Debug.Log(nameof(cake.ToppingSmashed));
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
