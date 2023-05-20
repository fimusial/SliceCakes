using System;
using UnityEngine;

public class Throttler
{
  private readonly float intervalSeconds;
  private float targetTime = 0;

  public Throttler(float intervalSeconds)
  {
    this.intervalSeconds = intervalSeconds;
  }

  public void Run(Action callback)
  {
    if (targetTime <= Time.time)
    {
      targetTime = Time.time + intervalSeconds;
      callback?.Invoke();
    }
  }

  public void ResetTimer()
  {
    targetTime = 0;
  }
}
