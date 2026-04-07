using UnityEngine;

public class PlayParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystemToPlay;

    public void Play()
    {
        if (particleSystemToPlay != null)
        {
            particleSystemToPlay.Play();
        }
    }
}
