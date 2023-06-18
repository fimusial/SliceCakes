using System.Collections.Generic;
using UnityEngine;

public class Cake : MonoBehaviour
{
  public float rotationSpeedAngle = -1.5f; // + for clockwise
  public float sliceAtAngle = 0f; // 180 for left handed knife

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
    slivers[sliceAtIndex].Topping.Active = false;
  }

  public void Reset()
  {
    slivers.ForEach(x =>
    {
      x.Active = true;

      // todo: generate n random indexes and activate based on that
      // todo: pass n as parameter
      x.Topping.Active = Random.Range(0f, 1f) < 0.05f;
    });
  }

  // Math
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
