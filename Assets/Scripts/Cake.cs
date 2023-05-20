using System.Collections.Generic;
using UnityEngine;

public class Cake : MonoBehaviour
{
  public float rotationSpeedAngle = -1.5f; // + for clockwise
  public float sliceAtAngle = 0f; // 180 for left handed knife

  private const int SLIVER_COUNT = 64;
  private readonly List<Transform> slivers = new List<Transform>();

  public void Start()
  {
    var firstSliver = gameObject.transform.GetChild(0);
    slivers.Add(firstSliver);

    // start from 1 to skip the first sliver, which is a constant part of the scene
    for (int i = 1; i < SLIVER_COUNT; i++)
    {
      var rotationAngle = AnySliverIndexToBoundAngle(i);

      var newSliver = Instantiate(
        firstSliver,
        firstSliver.position,
        Quaternion.AngleAxis(-rotationAngle, Vector3.up));

      newSliver.parent = gameObject.transform;
      slivers.Add(newSliver);
    }
  }

  public void FixedUpdate()
  {
    transform.Rotate(Vector3.up, rotationSpeedAngle);
  }

  public void Slice()
  {
    var sliceAtIndex = AnyAngleToBoundSliverIndex(transform.eulerAngles.y - sliceAtAngle);
    slivers[sliceAtIndex].gameObject.SetActive(false);
  }

  public void ResetSlices()
  {
    slivers.ForEach(x => x.gameObject.SetActive(true));
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
