
using System.Collections.Generic;
using System.Collections;
using UnityEngine;


public class BOX_spawn : MonoBehaviour
{
    public List<GameObject> beatPrefabs;  // List of prefabs to be generated on the beat
    public AudioClip audioClip;           // Audio clip to analyze
    public float threshold = 0.5f;        // Beat detection threshold
    public float spawnInterval = 0.2f;    // Time between beat spawns
    public float volumeMultiplier = 1f;   // Adjust this to control sensitivity
    public float beatForce = 50f;         // Force applied to the beatPrefab on each beat
    public float beatDestroyDelay = 2f;   // Time before the instantiated beat is destroyed

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.playOnAwake = false;
        audioSource.volume = 0.5f;
        audioSource.Play();

        InvokeRepeating("SpawnBeat", 0f, spawnInterval);
    }

    private void SpawnBeat()
    {
        float[] spectrum = new float[256];
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Hamming);

        float totalVolume = 0f;
        for (int i = 0; i < spectrum.Length; i++)
        {
            totalVolume += spectrum[i];
        }

        totalVolume *= volumeMultiplier;

        if (totalVolume > threshold)
        {
            // Randomly select a prefab from the list
            int randomIndex = Random.Range(0, beatPrefabs.Count);
            GameObject selectedPrefab = beatPrefabs[randomIndex];

            GameObject beatInstance = Instantiate(selectedPrefab, transform.position, Quaternion.identity);
            Rigidbody beatRigidbody = beatInstance.GetComponent<Rigidbody>();

            if (beatRigidbody == null)
            {
                beatRigidbody = beatInstance.AddComponent<Rigidbody>();
            }

            // Set the direction towards which the beatPrefab will move
            Vector3 moveDirection = Vector3.back; // Adjust this vector as needed

            // Apply force to move the beatPrefab in a specific direction
            beatRigidbody.AddForce(moveDirection * beatForce, ForceMode.Impulse);

            // Destroy the beatPrefab after a delay
            Destroy(beatInstance, beatDestroyDelay);
        }
    }
}
