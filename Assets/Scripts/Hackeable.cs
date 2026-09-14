using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Hackeable : MonoBehaviour
{
    private Renderer objectRenderer;
    private Color originalColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      objectRenderer = GetComponent<Renderer>();  
        originalColor = objectRenderer.material.color;
    }

   public void Hack()
    {
        StartCoroutine(HackEffect());

    }
    private IEnumerator HackEffect()
    {
        objectRenderer.material.color = Color.green;
        yield return new WaitForSeconds(3f);
        objectRenderer.material.color = originalColor;
    }
}
