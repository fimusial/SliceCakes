using System;
using System.Collections.Generic;
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

  private Dictionary<ToppingKind, GameObject> children = new();

  public void Start()
  {
    foreach (ToppingKind enumKey in Enum.GetValues(typeof(ToppingKind)))
    {
      var foundChild = transform.Find(enumKey.ToString());

      if (foundChild != null)
      {
        children.Add(enumKey, foundChild.gameObject);
      }
    }

    RandomizeKind();
  }

  private void RandomizeKind()
  {
    var chosenKind = (ToppingKind)UnityEngine.Random.Range(0, 2);
    foreach (var child in children)
    {
      child.Value.SetActive(child.Key == chosenKind);
    }
  }

  private enum ToppingKind : int
  {
    Candle = 0,
    Strawberry = 1,
  }
}
