using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cake : MonoBehaviour
{
  private const int SLIVER_COUNT = 64;
  private readonly List<Sliver> slivers = new List<Sliver>();
  private Vector3 firstSliverInitialPosition;
  private Vector3 cakeInitialPosition;

  public event Action ToppingSmashed;
  public event Action CakeSliced;
  public event Action CakeReset;
  public float RotationSpeedAngle { get; set; } = -1.5f; // + for clockwise
  public int ToppingCount { get; set; } = 5;
  public int ToppingHitMargin { get; set; } = 3;
  public int ToppingPositionVariance { get; set; } = 5;
  public float SliceAtAngle { get; set; } = 0f;
  public bool ResetAnimationInProgress { get; private set; } = false;

  public void Start()
  {
    cakeInitialPosition = transform.position;

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
    transform.Rotate(Vector3.up, RotationSpeedAngle);
  }

  public void Slice()
  {
    if (ResetAnimationInProgress)
    {
      throw new InvalidOperationException($"Called {nameof(Slice)} while reset animation was in progress.");
    }

    var sliceAtIndex = AnyAngleToBoundSliverIndex(transform.eulerAngles.y - SliceAtAngle);
    slivers[sliceAtIndex].Active = false;

    RangeAround(sliceAtIndex, ToppingHitMargin)
    .ToList()
    .ForEach(unboundIndex =>
      {
        var topping = slivers[Modulo(unboundIndex, SLIVER_COUNT)].Topping;

        if (topping.Active)
        {
          topping.Smashed = true;
          ToppingSmashed?.Invoke();
        }

        topping.Active = false;
      });

    CakeSliced?.Invoke();
  }

  public int GetScoreChange(int slices)
  {
    var averageSlice = (SLIVER_COUNT - slices) / slices;
    var groups = GetSlicedSliverGroups();
    var accumulator = 0;

    foreach (var group in groups)
    {
      var diff = Math.Abs(averageSlice - group.Count);
      diff = diff <= 1 ? 0 : diff;
      accumulator += diff;
    }

    var smashedCount = slivers.Count(x => x.Topping.Smashed);
    var scoreChange = 100 - accumulator - (5 * smashedCount);
    return scoreChange;
  }

  public void ResetState(bool noAnimation = false)
  {
    if (ResetAnimationInProgress)
    {
      throw new InvalidOperationException($"Called {nameof(ResetState)} while reset animation was in progress.");
    }

    if (noAnimation)
    {
      ResetState();
      return;
    }

    ResetAnimationInProgress = true;
    StartCoroutine(nameof(ResetAnimationCoroutine));
  }

  private void ResetState()
  {
    slivers.ForEach(x =>
      {
        x.Active = true;
        x.Topping.Active = false;
        x.Topping.Smashed = false;
      });

    GetQuasiRandomToppingIndexes()
    .ToList()
    .ForEach(boundIndex =>
      {
        slivers[boundIndex].Topping.Active = true;
      });
  }

  private IEnumerator ResetAnimationCoroutine()
  {
    var groups = GetSlicedSliverGroups();

    // 256 frames in total over 1 second, first loop 192, second loop 64
    for (int frameIndex = 0; frameIndex < 192; frameIndex++)
    {
      var accel = frameIndex / 4096f;
      for (int groupIndex = 0; groupIndex < groups.Count; groupIndex++)
      {
        if (frameIndex >= groupIndex * (128 / groups.Count))
        {
          groups[groupIndex].ForEach(sliver => sliver.transform.Translate(0f, -accel, 0f));
        }
      }

      yield return new WaitForSeconds(1f / 256f);
    }

    slivers.ForEach(x => x.transform.position = firstSliverInitialPosition);
    ResetState();

    var cakeShiftedPosition = Quaternion.AngleAxis(SliceAtAngle + 180f, Vector3.up)
      * new Vector3(5f, cakeInitialPosition.y, cakeInitialPosition.z);

    for (int frameIndex = 0; frameIndex < 64; frameIndex++)
    {
      transform.position = Vector3.Lerp(cakeShiftedPosition, cakeInitialPosition, frameIndex / 64f);
      yield return new WaitForSeconds(1f / 256f);
    }

    transform.position = cakeInitialPosition;
    ResetAnimationInProgress = false;
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

    return groups.OrderByDescending(x => x.Count).ToList();
  }

  private IEnumerable<int> GetQuasiRandomToppingIndexes()
  {
    var spread = SLIVER_COUNT / ToppingCount;
    for (int i = 0; i < ToppingCount; i++)
    {
      var unboundIndex = (i * spread) + UnityEngine.Random.Range(-ToppingPositionVariance, ToppingPositionVariance);
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
