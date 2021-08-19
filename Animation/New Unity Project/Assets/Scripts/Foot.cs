using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foot : MonoBehaviour
{
    float height; // Height of the foot in the previous frame
    [SerializeField] ParticleSystem sand; // Particle emitter for kicked up sand
    
    void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        // Check if the foot is close to sandy ground
        bool onGround = Physics.Raycast(ray, 0.2f, LayerMask.GetMask("Sand"));

        // Vertical foot speed 
        float footSpeed = Mathf.Abs(height - transform.position.y)/ Time.deltaTime;

        // Kick up sand when the foot hits the ground
        if (onGround && !sand.isPlaying && footSpeed > 1) {
            // Scale the sand particle kicked proportional to the cuberoot of how fast the foot hits the ground
            sand.transform.localScale = 1.5f*Mathf.Pow(footSpeed,1f/3)*Vector3.one;
            // Emit burst of sand particles
            sand.Play();
        }

        height = transform.position.y;
    }
}
