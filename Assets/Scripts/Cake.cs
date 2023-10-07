using System;
using System.Collections;
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
  private Vector3 firstSliverInitialPosition;

  public bool ResetTransitionInProgress { get; private set; } = false;

  public void Start()
  {
    var firstSliver = gameObject.GetComponentInChildren<Sliver>();
    firstSliverInitialPosition = firstSliver.transform.position;

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
          ToppingSmashed?.Invoke();
        }

        topping.Active = false;
      });

    CakeSliced?.Invoke();
  }

  public void TriggerResetTransition()
  {
    if (ResetTransitionInProgress)
    {
      throw new InvalidOperationException($"Called {nameof(TriggerResetTransition)} while reset transition was in progress");
    }

    ResetTransitionInProgress = true;
    StartCoroutine(nameof(ResetTransitionCoroutine));
  }

  public void ResetSlicesAndToppings()
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
  }

  private IEnumerator ResetTransitionCoroutine()
  {
    var groups = GetSlicedSliverGroups();

    for (int frameIndex = 1; frameIndex <= 256; frameIndex++)
    {
      for (int groupIndex = 0; groupIndex < groups.Count; groupIndex++)
      {
        if (frameIndex >= groupIndex * (128 / groups.Count))
        {
          groups[groupIndex].ForEach(sliver => sliver.transform.Translate(0f, -1f / 64f, 0f));
        }
      }

      yield return new WaitForSeconds(1f / 256f);
    }

    slivers.ForEach(x => x.transform.position = firstSliverInitialPosition);
    
    ResetSlicesAndToppings();

    // slide the cake in from right/left
    ResetTransitionInProgress = false;
    CakeReset?.Invoke();
  }

  private List<List<Sliver>> GetSlicedSliverGroups()
  {
    var groups = new List<List<Sliver>>();
    var accumulator = new List<Sliver>();
    foreach (var sliver in slivers)
    {
      if (sliver.Active)
      {
        accumulator.Add(sliver);
      }
      else
      {
        groups.Add(accumulator);
        accumulator = new List<Sliver>();
      }
    }

    groups.Add(accumulator);
    groups.RemoveAll(x => !x.Any());

    if (slivers.First().Active && slivers.Last().Active)
    {
      var lastGroup = groups.Last();
      groups.First().AddRange(lastGroup);
      groups.Remove(lastGroup);
    }

    return groups.OrderBy(x => x.Count).ToList();
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
