using System.Collections.Generic;
using UnityEngine;

public class Cake : MonoBehaviour
{
  private const int SLIVER_COUNT = 64;
  private readonly List<Transform> Slivers = new List<Transform>();

  // TODO: take defaults from settings
  public float RotationSpeedAngle { get; set; } = -1.5f; // + for clockwise
  public float SliceAtAngle { get; set; } = 0f; // 180 for left handed knife

  public void Start()
  {
    var firstSliver = gameObject.transform.GetChild(0);
    Slivers.Add(firstSliver);

    // start from 1 to skip the first sliver which already exists
    for (int i = 1; i < SLIVER_COUNT; i++)
    {
      var rotationAngle = AnySliverIndexToBoundAngle(i);
      
      var newSliver = Instantiate(
        firstSliver,
        firstSliver.position,
        Quaternion.AngleAxis(-rotationAngle, Vector3.up));

      newSliver.parent = gameObject.transform;
      Slivers.Add(newSliver);
    }
  }

  public void Update()
  {
    if (Input.GetKeyDown("k"))
    {
      SliceAt(SliceAtAngle);
    }

    if (Input.GetKeyDown("r"))
    {
      ResetSlices();
    }
  }

  public void FixedUpdate()
  {
    transform.Rotate(Vector3.up, RotationSpeedAngle);
  }

  public void SliceAt(float angle)
  {
    var sliceAtIndex = AnyAngleToBoundSliverIndex(transform.eulerAngles.y - angle);
    Slivers[sliceAtIndex].gameObject.SetActive(false);
  }

  public void ResetSlices()
  {
    Slivers.ForEach(x => x.gameObject.SetActive(true));
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

  private int Modulo(int x, int m) {
    return (x % m + m) % m;
  }
}
