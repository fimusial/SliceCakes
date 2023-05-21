using UnityEngine;

public class Knife : MonoBehaviour
{
  public string sliceAnimationTriggerName = "KnifeSliceTrigger";
  public string sliceAnimationStateName = "KnifeSlice";
  public string idleAnimationStateName = "KnifeIdle";
  public int sliceAnimationLayerIndex = 0;

  private Animator animator;

  public void Start()
  {
    animator = gameObject.GetComponent<Animator>();
  }

  public void TriggerSliceAnimation()
  {
    animator.ResetTrigger(sliceAnimationTriggerName);
    animator.SetTrigger(sliceAnimationTriggerName);
  }

  public bool IsSlicingOrInTransition()
  {
    var inTransition = animator
      .GetAnimatorTransitionInfo(sliceAnimationLayerIndex)
      .IsName($"{idleAnimationStateName} -> {sliceAnimationStateName}");

    var slicing = animator
      .GetCurrentAnimatorStateInfo(sliceAnimationLayerIndex)
      .IsName(sliceAnimationStateName);

    return inTransition || slicing;
  }
}
