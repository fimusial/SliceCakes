using UnityEngine;

public class CandleAnimator : MonoBehaviour
{
  // set in Unity UI
  public MeshRenderer candlestickMeshRenderer;
  public Transform flameTransform;

  private float timePhase;

  public void Start()
  {
    timePhase = UnityEngine.Random.Range(0f, Mathf.PI);
  }

  public void FixedUpdate()
  {
    candlestickMeshRenderer.material.mainTextureOffset = new Vector2(0f, Mathf.Sin(Time.time + timePhase));

    flameTransform.Rotate(Vector3.up, 5f);

    flameTransform.position = new Vector3(
      flameTransform.position.x,
      flameTransform.position.y + (0.0005f * Mathf.Sin((Time.time + timePhase) * 5f)),
      flameTransform.position.z
    );
  }
}
