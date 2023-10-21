using UnityEngine;

public class Knife : MonoBehaviour
{
  public string sliceAnimationTriggerName = "KnifeSliceTrigger";
  public string sliceAnimationStateName = "KnifeSlice";
  public int sliceAnimationLayerIndex = 0;
  public bool leftHanded = false;

  private Animator animator;

  public void Start()
  {
    animator = gameObject.GetComponentInChildren<Animator>();

    if (leftHanded)
    {
      transform.Rotate(0f, 180f, 0f);
    }
  }

  public void TriggerSliceAnimation()
  {
    animator.ResetTrigger(sliceAnimationTriggerName);
    animator.SetTrigger(sliceAnimationTriggerName);
  }

  public bool IsSlicing()
  {
    return animator
      .GetCurrentAnimatorStateInfo(sliceAnimationLayerIndex)
      .IsName(sliceAnimationStateName);
  }
}
