using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dest_GM : MonoBehaviour
{
    public ParticleSystem destroyEffect; // Reference to the particle system

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("TargetObject"))
        {
            // If the collided object is the targetObject, destroy the GameObject
            Destroy(collision.gameObject);

            // Activate the particle effect at the collision position
            ActivateParticleEffect(collision.contacts[0].point);

            Debug.Log("Dest");
        }
    }

    private void ActivateParticleEffect(Vector3 position)
    {
        // Check if the particle system reference is assigned
        if (destroyEffect != null)
        {
            // Instantiate the particle system at the specified position
            ParticleSystem newEffect = Instantiate(destroyEffect, position, Quaternion.identity);

            // Destroy the instantiated particle system after its duration
            Destroy(newEffect.gameObject, newEffect.main.duration);
        }
    }
}
