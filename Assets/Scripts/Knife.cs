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

  public bool SliceAnimationInProgress => animator?
    .GetCurrentAnimatorStateInfo(sliceAnimationLayerIndex)
    .IsName(sliceAnimationStateName)
    ?? false;

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

  private void OnKnifeDown()
  {
    KnifeDown?.Invoke();
  }
}
