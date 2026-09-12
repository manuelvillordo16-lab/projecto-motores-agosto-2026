using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Hackeable : MonoBehaviour
{
    private Renderer objectRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      objectRenderer = GetComponent<Renderer>();  
    }

   public void Hack()
    {
        objectRenderer.material.color = Color.green;
    }
}
