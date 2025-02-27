using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotationSpeed = 50f; //degree units for rotation 
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the coin constantly
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
