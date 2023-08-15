using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cake : MonoBehaviour
{
  public float rotationSpeedAngle = -1.5f; // + for clockwise
  public float sliceAtAngle = 0f; // 180 for left handed knife
  
  public int toppingCount = 5;
  public int toppingHitMargin = 3;
  public int toppingPositionVariance = 5;

  public event Action ToppingSmashed;
  public event Action CakeSliced;
  public event Action CakeReset;

  private const int SLIVER_COUNT = 64;
  private readonly List<Sliver> slivers = new List<Sliver>();

  public void Start()
  {
    var firstSliver = gameObject.GetComponentInChildren<Sliver>();
    slivers.Add(firstSliver);

    // start from 1 to skip the first sliver, which is a constant part of the scene
    for (int i = 1; i < SLIVER_COUNT; i++)
    {
      var rotationAngle = AnySliverIndexToBoundAngle(i);

      var newSliver = Instantiate(
        firstSliver,
        firstSliver.transform.position,
        Quaternion.AngleAxis(-rotationAngle, Vector3.up));

      newSliver.transform.parent = gameObject.transform;
      slivers.Add(newSliver);
    }

    slivers.ForEach(x =>
      {
        x.Active = true;
        x.Topping = x.GetComponentInChildren<Topping>();
        x.Topping.Active = false;
      });
  }

  public void FixedUpdate()
  {
    transform.Rotate(Vector3.up, rotationSpeedAngle);
  }

  public void Slice()
  {
    var sliceAtIndex = AnyAngleToBoundSliverIndex(transform.eulerAngles.y - sliceAtAngle);
    slivers[sliceAtIndex].Active = false;

    RangeAround(sliceAtIndex, toppingHitMargin)
    .ToList()
    .ForEach(unboundIndex =>
      {
        var topping = slivers[Modulo(unboundIndex, SLIVER_COUNT)].Topping;
        
        if (topping.Active)
        {
          ToppingSmashed.Invoke();
        }

        topping.Active = false;
      });

    CakeSliced.Invoke();
  }

  public void Reset()
  {
    slivers.ForEach(x =>
      {
        x.Active = true;
        x.Topping.Active = false;
      });

    GetQuasiRandomToppingIndexes()
    .ToList()
    .ForEach(boundIndex =>
      {
        slivers[boundIndex].Topping.Active = true;
      });

    CakeReset.Invoke();
  }

  private IEnumerable<int> GetQuasiRandomToppingIndexes()
  {
    var spread = SLIVER_COUNT / toppingCount;
    for (int i = 0; i < toppingCount; i++)
    {
      var unboundIndex = (i * spread) + UnityEngine.Random.Range(-toppingPositionVariance, toppingPositionVariance);
      yield return Modulo(unboundIndex, SLIVER_COUNT);
    }
  }

  // Math
  private IEnumerable<int> RangeAround(int middle, int width)
  {
    return Enumerable.Range(middle - width / 2, width);
  }

  private float AnySliverIndexToBoundAngle(int index)
  {
    var boundIndex = Modulo(index, SLIVER_COUNT);
    return (float)boundIndex / (float)SLIVER_COUNT * 360f;
  }

  private int AnyAngleToBoundSliverIndex(float angle)
  {
    var unboundIndex = (int)((angle / 360f) * SLIVER_COUNT);
    return Modulo(unboundIndex, SLIVER_COUNT);
  }

  private int Modulo(int x, int m)
  {
    return (x % m + m) % m;
  }
}
