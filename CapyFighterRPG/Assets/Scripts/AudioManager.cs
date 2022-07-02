using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip _victorySound;
    [SerializeField] private AudioClip _lossSound;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayVictorySound()
    {
        if (_victorySound == null)
            Debug.Log("Audioclip Victory Sound not found!");

        _audioSource.clip = _victorySound;
        _audioSource.loop = false;
        _audioSource.Play();
    }

    public void PlayLossSound()
    {
        if (_lossSound == null)
            Debug.Log("Audioclip Loss Sound not found!");

        _audioSource.clip = _lossSound;
        _audioSource.loop = false;
        _audioSource.Play();
    }
}
