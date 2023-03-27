
using UnityEngine;

public class AudioController : MonoBehaviour
{

    [SerializeField] private UnitsData unitsData;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        audioSource.loop = true;
    }
    private void Update()
    {
        audioSource.volume = unitsData.Vol / 100f;
    }
}
