using UnityEngine;

public class StrawberryAnimator : MonoBehaviour
{
  // set in Unity UI
  public MeshRenderer strawberryMeshRenderer;

  private float timePhase;

  public void Start()
  {
    timePhase = UnityEngine.Random.Range(0f, Mathf.PI);
  }

  public void FixedUpdate()
  {
    var h = Mathf.Sin(Time.time * 0.5f + timePhase) * 0.5f + 0.5f;
    strawberryMeshRenderer.material.color = Color.HSVToRGB(h, 1.0f, 1.0f);
  }
}
