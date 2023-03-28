using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private UnitsData unitsData;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        audioSource.Play();
        audioSource.loop = true;
    }
    private void Update()
    {
        audioSource.volume = unitsData.Vol / 100f;
        /*if (!unitsData.IsMute)
            audioSource.Play();
        else
            audioSource.Stop();*/

        //if (unitsData.EndGame && !unitsData.IsMute) audioSource.Stop();
    }
}
