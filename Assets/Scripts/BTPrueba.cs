using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Panda;
using UnityEngine.SceneManagement;

public class BTPrueba : MonoBehaviour
{
    public float velocidad = 5;
    private int colisionoCon = 0;

    public Transform objeto;
    public Transform arcoColision;
    public Transform pelota; // La transformada del enemigo
    public Transform arco; // La transformada del enemigo
    public Vector3 puntoRetorno = new Vector3(0f, 0f, 0f);
    public Vector3 puntoPenal = new Vector3(0f, 0f, 0f);
    public float distanciaUmbral = 10.0f; // Umbral de distancia para considerar que el enemigo está cerca
    public bool cerca = false;
    public Text scoreText;
    public int score = 0;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Pelota (1)")
            colisionoCon = 1;
        if (collision.collider.name == "ArcoColision")
            colisionoCon = 2;
    }

    private IEnumerator Score()
    {
        if (score == 50)
        {
            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene(2);
        }
    }

    private void FixedUpdate()
    {
        if (pelota != null)
        {
            float distanciaAlEnemigo = Vector3.Distance(objeto.position, pelota.position);
            if (distanciaAlEnemigo <= distanciaUmbral)
                cerca = true;
            else
                cerca = false;
        }
        Debug.Log("Score IA" + score);
        scoreText.text = score.ToString();
        StartCoroutine(Score());

    }
   [Task]
    private void PelotaCerca()
    {
        Debug.Log("PelotaCercaTask");
        if (cerca)
        {
            Debug.Log("PelotaCerca");
            Task.current.Succeed();
        }
        else
        {
            Debug.Log("La Pelota no esta Cerca");
            Task.current.Fail();
        }

    }


    [Task]
    private void IrPelota()
    {
        if (pelota != null)
        {
            transform.LookAt(pelota.position);
            transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
            Task.current.Succeed();
        }
        else
        {
            Task.current.Fail();
        }
    }

    [Task]
    private void ImpactarPelota()
    {
        if (colisionoCon == 1)
        {
            score = score + 10;
            colisionoCon = 0; // Reiniciar la colisión
            pelota.position = arco.position;
           /* pelota.LookAt(arco.position);
            pelota.Translate(Vector3.forward * velocidad * Time.deltaTime);*/
            Task.current.Succeed();
        }
        else
        {
            Task.current.Fail();
        }
    }

    [Task]
    private void Regresar()
    {
        transform.LookAt(puntoRetorno);
        transform.Translate(Vector3.forward * 3.5f * Time.deltaTime);

        float distanciaAlPunto = Vector3.Distance(transform.position, puntoRetorno);
        if (distanciaAlPunto < 0.1f) // Puedes ajustar el umbral de distancia
        {
            Task.current.Succeed();
        }
    }

    [Task]
    private void DevolverPelota()
    {
        pelota.LookAt(puntoPenal);
        pelota.Translate(Vector3.forward * velocidad * Time.deltaTime);

        float distanciaAlPunto = Vector3.Distance(pelota.position, puntoPenal);
        if (distanciaAlPunto < 0.1f)
        {
            Task.current.Succeed();
        }
    }

}