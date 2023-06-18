using UnityEngine;

public class Sliver : MonoBehaviour
{
  public Topping Topping { get; set; }

  public bool Active
  {
    get
    {
      return gameObject.activeSelf;
    }
    set
    {
      gameObject.SetActive(value);
    }
  }
}
