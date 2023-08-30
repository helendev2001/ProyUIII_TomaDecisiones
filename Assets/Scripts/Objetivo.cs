using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Objetivo : MonoBehaviour
{
    public Transform arco; // Referencia al transform del arco
    public Vector3 puntoPenal = new Vector3(-6.5f, 3.9f, 6f);
    public float velocidadMovimiento = 5.0f;
    public Text scoreText;
    public int score = 0;
    void FixedUpdate()
    {
        Debug.Log("Score" + score);
        scoreText.text = score.ToString();
        StartCoroutine(Score());

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Choque con player");
            MoverHaciaArco();
            score = score + 10;
        }
    }

    private void MoverHaciaArco()
    {
        StartCoroutine(MoverBalon(arco.position, puntoPenal));
    }

    private IEnumerator MoverBalon(Vector3 destino1, Vector3 destino2)
    {
        Vector3 startPosition = transform.position;
        float journeyLength = Vector3.Distance(startPosition, destino1);
        float startTime = Time.time;

        // Mover hacia el arco
        while (transform.position != destino1)
        {
            float distanceCovered = (Time.time - startTime) * velocidadMovimiento;
            float fractionOfJourney = distanceCovered / journeyLength;

            transform.position = Vector3.Lerp(startPosition, destino1, fractionOfJourney);

            yield return null;
        }

        // Esperar un poco en el arco
        yield return new WaitForSeconds(1.5f);

        // Mover de vuelta al punto penal
        journeyLength = Vector3.Distance(destino1, destino2);
        startTime = Time.time;
        startPosition = destino1;

        while (transform.position != destino2)
        {
            float distanceCovered = (Time.time - startTime) * velocidadMovimiento;
            float fractionOfJourney = distanceCovered / journeyLength;

            transform.position = destino2;

            yield return null;
        }
    }
    private IEnumerator Score()
    {
        if (score == 50)
        {
            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene(1);
        }
    }
}




