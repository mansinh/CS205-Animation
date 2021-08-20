using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foot : MonoBehaviour
{
    float height; // Height of the foot in the previous frame
    [SerializeField] ParticleSystem sand; // Particle emitter for kicked up sand
    PlayerController controller;

    private void Start()
    {
        controller = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        // Check if the foot is close to sandy ground
        bool onGround = Physics.Raycast(ray, 0.1f, LayerMask.GetMask("Sand"));

      

        // Kick up sand when the foot hits the ground
        if (onGround && !sand.isPlaying) {
            if (controller.speed > 0 && transform.position.y > height)
            {
                sand.transform.localScale =  2*controller.speed * Vector3.one;
                // Emit burst of sand particles
                sand.Play();
            }
            if (controller.verticalSpeed>0 || controller.isLanding) {
                // Emit burst of sand particles
                sand.transform.localScale = 3 * Vector3.one;
                sand.Play();
            }
           
        }

        height = transform.position.y;
    }
}
