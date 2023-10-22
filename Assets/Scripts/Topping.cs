using UnityEngine;

public class Topping : MonoBehaviour
{
  public bool Smashed { get; set; }

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
