using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agente : MonoBehaviour
{

    Rigidbody _rb = null;
    public float velocidad = 400;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float moverHorizontal = Input.GetAxis("Horizontal");
        float moverVertical = Input.GetAxis("Vertical");
        Vector3 movimiento = new Vector3(moverHorizontal, 0f, moverVertical);
        _rb.AddForce(movimiento * velocidad * Time.deltaTime);
        
    }
    private void OnCollisionEnter(Collision collision)
    {
       
        if (collision.collider.name == "Pelota")
            StartCoroutine(MoverDespuesDeEspera());
    }
    private IEnumerator MoverDespuesDeEspera()
    {
        yield return new WaitForSeconds(1.5f);
        transform.position = new Vector3(-1, 1, 0);
        
    }
}
