using UnityEngine;

public class PercImpactParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;

    public bool IsActive => gameObject.activeSelf;

    public void Play(Color color)
    {
        ParticleSystem.MainModule main = _particleSystem.main;
        main.startColor = color;
        gameObject.SetActive(true);
        _particleSystem.Play();
    }

    private void FixedUpdate()
    {
        transform.rotation = new Quaternion(-0.25f,0,0,0);
    }
}
