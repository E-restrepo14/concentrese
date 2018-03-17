using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiradorDeBloques : MonoBehaviour 
{
	public GameObject particula;

	void Start ()
	{
		//este script esta dentro de todos los tiles prefabs, 
		GameManager.Instance.cubosenEscena ++;
	}


	void OnMouseDown() 
	{
		//cada vez que detecta el onmouse down, consulta al gamemanager si el jugador puede girar o no
		if (GameManager.Instance.puedeGirar == true ) 
		{

			//este if se ejecutara si el usuario no ha girado ningun cubo.
			if (GameManager.Instance.yaGiro == false ) 
			{
				// la variable ya giro empieza siempre valiendo falso, 
				// pero cuando se ejecuta esta vale verdadero... para informar que ya hay un cubo seleccionado  
				GameManager.Instance.yaGiro = true;
				// el cubo se guarda en esta variable seleccionado1... que se encuentra almacenada en el game manager
				GameManager.Instance.seleccionado1 = gameObject;
				//se instancia una particula y se vuelve hija del cubo selecionado1
				Instantiate(particula,transform.position,Quaternion.identity).transform.SetParent(transform);
				// y desde esta se le ordena que inicie la coroutine voltear.
				StartCoroutine (GameManager.Instance.Voltear (GameManager.Instance.seleccionado1));

			} 

			//en caso de que ya se haya girado un cubo en la escena... se ejecutara este else
			else
			{
				// pero solo se ejecutara si tiene una posicion diferente al "seleccionado1"
				if (transform.position != GameManager.Instance.seleccionado1.transform.position)
				{			
				GameManager.Instance.yaGiro = false;
				GameManager.Instance.seleccionado2 = gameObject;
				Instantiate (particula, transform.position, Quaternion.identity).transform.SetParent (transform);
				StartCoroutine (GameManager.Instance.Voltear (GameManager.Instance.seleccionado2));
			
				//en el momento que ya se hayan seleccionado dos... se comparan los dos cubos
				StartCoroutine (GameManager.Instance.Comparar (GameManager.Instance.seleccionado1, GameManager.Instance.seleccionado2));
				}
			}
		}
	}
}