using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foot : MonoBehaviour
{
    float height;


    [SerializeField] ParticleSystem sand;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        bool onGround = Physics.Raycast(ray, 0.2f, LayerMask.GetMask("Sand"));
    
        if (onGround && !sand.isPlaying && Mathf.Abs(height-transform.position.y)>Time.deltaTime) {
            sand.transform.localScale = 2*Mathf.Pow(Mathf.Abs(height - transform.position.y)/Time.deltaTime,1f/3)*Vector3.one;
            sand.Play();
        }
        height = transform.position.y;
    }
}
