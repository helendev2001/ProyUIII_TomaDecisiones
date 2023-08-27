using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

public class BTPrueba : MonoBehaviour
{
    public float velocidad = 5;
    public float energia;
    private int colisionoCon = 0;// 1-bateria, 2-A, 3-B

    //Enemigo
    public Transform objeto;
    public Transform enemigo; // La transformada del enemigo
    public Transform zonaSegura;
    public GameObject enemigoInstance;
    public float distanciaUmbral = 10.0f; // Umbral de distancia para considerar que el enemigo está cerca
    public bool cerca = false;
    public List<Transform> puntos;
    public int tempPunto=0;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Enemigo")
            colisionoCon = 1;
        if (collision.collider.name == "ZonaSegura")
            colisionoCon = 2;
        if (collision.collider.name == "PuntoB")
            colisionoCon = 3;
    }
    private void MoverObject()
     {
        Vector3 destino = (puntos[tempPunto].position-transform.position).normalized;
        transform.Translate(destino * velocidad * Time.deltaTime);


    }
    private void FixedUpdate()
    {
        Debug.Log("Energia"+energia);
        if (enemigo != null)
        {
            float distanciaAlEnemigo = Vector3.Distance(objeto.position, enemigo.position);
            if (distanciaAlEnemigo <= distanciaUmbral)
                cerca = true;
            else
                cerca = false;
        }

    }
    [Task]
    private void EnemigoCerca()
    {
        Debug.Log("EnemigoCercaTask");
        if (cerca)
        {
            Debug.Log("EnemigoCerca");
            Task.current.Succeed();
        }
        else
        {
            Debug.Log("El Enemigo no esta Cerca");
            Task.current.Fail();
        }
     


    }

    [Task]
    private void IrEnemigo()
    {
        Debug.Log("IrEnemigoTask");
        if (enemigoInstance == null)
        {
            Task.current.Fail();
            return;
        }
        if (enemigo != null)
        {
            transform.LookAt(enemigo);
            transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
            energia -= 0.07f;
            Debug.Log("Colision " + colisionoCon);
            Task.current.Succeed();
        }
    }

    [Task]
    private void Atacar()
    {
        Debug.Log("AtacarTask");
        Destroy(enemigoInstance); // Utiliza enemigo.gameObject para destruir el objeto enemigo
        energia -= 0.07f;
        if(enemigoInstance == null)
        {
            Task.current.Fail();
            return;
        }
        Task.current.Succeed();

    }

    [Task]
    private void Descanzar()
    {
        Debug.Log("DescanzarTask");
        if (energia<15)
        {
          
            transform.LookAt(zonaSegura);
            transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
            if(colisionoCon==2)
            {
                energia = 100;
            }
        }
        Task.current.Succeed();

    }

    [Task]
    private void RecorrerPerimetro()
    {
        Debug.Log("RecorrerPerimetrfoTask");
        if (enemigo !=null && Vector3.Distance(transform.position, enemigo.transform.position) < distanciaUmbral)
        {
            Task.current.Fail();
            return;

        }

        if (Vector3.Distance(transform.position, puntos[tempPunto].position) < 1f)
        {
            tempPunto = (tempPunto + 1) % puntos.Count;
        }

        MoverObject();
        Task.current.Succeed();
        energia -= 0.07f;

       // if (energia < 10)
        //    Task.current.Fail();
    }
}