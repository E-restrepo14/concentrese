using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour 
{

	// este script es para destruir los emptys que quedan instanciados en la escena... ya que se les sacaron sus transform
	// no sirven de nada, y su presencia en la escena hace que el juego sea mas pesado, por lo que se les ordena destruirse

	void Update ()
	{
		// el fire 1 es para evitar algun problema.
		if (Input.GetButton ("Fire1")) 
		{
			Destroy (gameObject,5);
		}
	}
}
