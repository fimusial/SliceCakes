using UnityEngine;

public class Knife : MonoBehaviour
{
  public string sliceAnimationTriggerName = "KnifeSliceTrigger";
  public string sliceAnimationStateName = "KnifeSlice";
  public int sliceAnimationLayerIndex = 0;

  private Animator animator;

  public float SliceAtAngle { get; set; } = 0f;

  public void Start()
  {
    animator = gameObject.GetComponentInChildren<Animator>();
  }

  public void Update()
  {
    transform.rotation = Quaternion.AngleAxis(SliceAtAngle, Vector3.up);
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
