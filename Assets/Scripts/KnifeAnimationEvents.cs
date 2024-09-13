using System;
using UnityEngine;

public class KnifeAnimationEvents : MonoBehaviour
{
  private Action onKnifeDown;

  public void BindKnifeDownEvent(Action onKnifeDown) {
    this.onKnifeDown = onKnifeDown;
  }

  private void KnifeDownEventAnimatorCallback(AnimationEvent animationEvent)
  {
    onKnifeDown?.Invoke();
  }
}
