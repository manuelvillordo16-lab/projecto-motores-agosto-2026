using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Hackeable : MonoBehaviour
{
    private Renderer objectRenderer;
    private Color originalColor;
    private EnemyAI enemyAI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      objectRenderer = GetComponent<Renderer>();  
        originalColor = objectRenderer.material.color;

        enemyAI= GetComponent<EnemyAI>();
    }

   public void Hack()
    {
        Debug.Log("Hack activado en: " + gameObject.name);

        StartCoroutine(HackEffect());

    }
    private IEnumerator HackEffect()
    {
        objectRenderer.material.color = Color.green;
        if (enemyAI != null)
        {
            Debug.Log("EnemyAI encontrado. Aplicando stun. ");
            enemyAI.Stun(3f);
        }
        else
        {
            Debug.Log("Este objeto no tiene EnemyAI. ");
        }
        yield return new WaitForSeconds(3f);

        objectRenderer.material.color = originalColor;
    }
}
