using UnityEngine;

public class Statue : MonoBehaviour
{
    public AudioSource statueAudioSource;
    public AudioClip groundImpactSound;
    private bool hasPlayedSound = false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !hasPlayedSound)
        {
            if (statueAudioSource != null && groundImpactSound != null)
            {
                statueAudioSource.PlayOneShot(groundImpactSound);
                hasPlayedSound = true;  // Prevent sound from playing multiple times
            }
        }
    }
}
