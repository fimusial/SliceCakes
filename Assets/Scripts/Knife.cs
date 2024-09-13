using System;
using UnityEngine;

public class Knife : MonoBehaviour
{
  public string sliceAnimationTriggerName = "KnifeSliceTrigger";
  public string sliceAnimationStateName = "KnifeSlice";
  public int sliceAnimationLayerIndex = 0;

  public event Action KnifeDown;

  private Animator animator;
  private KnifeAnimationEvents knifeAnimationEvents;

  public float SliceAtAngle { get; set; } = 0f;

  public void Start()
  {
    animator = gameObject.GetComponentInChildren<Animator>();
    knifeAnimationEvents = gameObject.GetComponentInChildren<KnifeAnimationEvents>();

    knifeAnimationEvents.BindKnifeDownEvent(OnKnifeDown);
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

  private void OnKnifeDown()
  {
    KnifeDown?.Invoke();
  }
}
