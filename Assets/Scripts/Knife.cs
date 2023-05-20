using UnityEngine;

public class Knife : MonoBehaviour
{
  private const string SliceAnimationTriggerName = "SliceTrigger";
  private Animator animator;

  void Start()
  {
    animator = gameObject.GetComponent<Animator>();
  }

  void Update()
  {
    if (Input.GetKeyDown("k"))
    {
      animator.ResetTrigger(SliceAnimationTriggerName);
      animator.SetTrigger(SliceAnimationTriggerName);
    }
  }
}
