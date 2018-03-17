using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{

	public static GameManager Instance;
	public LevelManager levelmanager;

	//este es un singleton que se encarga de proveer variables a todo script que las necesite acceder.

	private void Awake()
	{
		if (Instance == null) 
		{
			Instance = this;
		} 
		else 
		{
			Destroy (this);
		}

		yaGiro = false;
	
		levelmanager = GetComponent<LevelManager> ();
	}


	//======================================================
	//estas son las variables que otros codigos necesitar acceder

	public int numeroTurnos = 0 ;
	public int cubosenEscena = 0 ;
	public bool yaGiro;
	public bool puedeGirar = true;
	public GameObject seleccionado1;
	public GameObject seleccionado2;
	public GameObject camara;
	public Image ganoimage;
	public Image perdioimage;
	public Image nextlvlimage;
	public Text movimientosHechos;

	public int nivelNum =1;


	// a medida que se sube de nivel, los tiles se buelven mas pequeños para caber en el muro... 
	// por lo que hay que acercar la camara para que el usuario pueda ver los dibujos con claridad
	public IEnumerator acercarcamara ()
	{
		int contador = 0;

		while (contador < 36) 
		{
			// durante 36 veces que se ejecute el while, la camara avanzará 0.109 en z
			camara.transform.Translate (0, 0, 0.109f);
			// como las animaciones se veian como muy lageadas... les puse un return de 0.0 segundos
			yield return new WaitForSeconds (0.0f);
			contador++;
		}
	}
	//============================================================================================================================================

	public IEnumerator mostrarWinSprite()
	{
		//en el momento que se ejecute alguna de estas 3 ordenes, debido a la variable puedegirar = false...
		//el jugador no podra realizar ninguna accion hasta que terminen y la variable valga true nuevamente.

		//tanto la funcion de mostrar el sprite de ganar o perder... tienen la instruccion de reiniciar el nivel automaticamente

		puedeGirar = false;
		ganoimage.enabled = true;
		yield return new WaitForSeconds (4);
		SceneManager.LoadScene ("mainbackup");
		ganoimage.enabled = false;
		puedeGirar = true;
	}

	public IEnumerator mostrarloseSprite()
	{
		puedeGirar = false;
		perdioimage.enabled = true;
		yield return new WaitForSeconds (4);
		SceneManager.LoadScene ("mainbackup");
		perdioimage.enabled = false;
		puedeGirar = true;
	}

	public IEnumerator mostrarNextLevelSprite()
	{
		puedeGirar = false;
		nextlvlimage.enabled = true;
		yield return new WaitForSeconds (2);
		nextlvlimage.enabled = false;
		puedeGirar = true;
	}
	//==========================================================================================================================================

	// cuando se invoque esta funcion, se tomara como argumento... el mismo gameobject dueño del script... que llamo esta funcion
	public IEnumerator Voltear (GameObject cubo)
	{
		int contador = 0;

		//durante 15 ciclos el cubo rotará 12 grados en Y, total seran 180
		while (contador < 15) 
		{
			cubo.transform.Rotate (0, 12, 0);
			yield return new WaitForSeconds (0.0f);
			contador++;
		}
	}
		
	// esta funcion toma los valores de las variables seleccionado1 y seleccionado2, (de las cuales se encargan otros scripts de asignarle el valor)
	public IEnumerator Comparar(GameObject seleccionado1,GameObject seleccionado2)
	{
		numeroTurnos++;
		//cada vez que se comparen dos cubos... se contara el movimiento, se almacenará
		// y se mostrará en este text, para que el usuario vea en todo momento cuantos movimientos ha hecho
		movimientosHechos.text = "movimientos realizados " + numeroTurnos.ToString ();

		//mientras se comparen los cubos el jugador no podra seleccionar otros cubos
		puedeGirar = false;
		// este wait forseconds es para que el jugador pueda ver si los dos cubos que selecciono... son iguales o diferentes
		yield return new WaitForSeconds (1.0f);

		//si el jugador selecciona un cubo... y lo selecciona de nuevo... al ser del mismo tag, el juego lo aceptaria como una pareja
		// y desapareceria el cubo solo... este if es para evitar que eso suceda
		if (seleccionado1.transform.position != seleccionado2.transform.position) 
		{
			
			if (seleccionado1.tag == seleccionado2.tag) 
			{
				//si los dos cubos tienen el mismo tag, se destruirán. (los tiles tienen en su propio script una orden de aumentar en 1 esta variable cubosenescena. desde el momento que aparecen)
				Destroy (seleccionado1);
				Destroy (seleccionado2);
				cubosenEscena = cubosenEscena - 2;


				if (cubosenEscena < 1) 
				{
					nivelNum++;

					// cada vez que se destruyan dos cubos... se comprobara si no quedan mas por destruir... y de ser asi. se aumentará 
					//el numero del nivel y se llamará la funcion de iniciarnivel, guardada en este otro script levelmanager.

					if (nivelNum >= 4) 
					{
						StartCoroutine (mostrarWinSprite ());
					} 
					else 
					{
						StartCoroutine ("acercarcamara");
						levelmanager.StartLevel ();

					}	
				}
			}


			//al no ser iguales los cubos, solo se voltearan nuevamente los cubos a su posicion original y se destruiran las particulas hijas para que no se instancien nuevamente unas sobre otras
			else {
				Destroy (seleccionado1.transform.GetChild (0).gameObject);
				Destroy (seleccionado2.transform.GetChild (0).gameObject);
				StartCoroutine (Voltear (GameManager.Instance.seleccionado1));
				StartCoroutine (Voltear (GameManager.Instance.seleccionado2));
			}
		} 
		else 
		{
			// en caso de que el jugador seleccione dos veces el mismo cuadro... igualmente se necesita destruir las dos particulas hijas
			Destroy (seleccionado1.transform.GetChild (0).gameObject);
			Destroy (seleccionado2.transform.GetChild (0).gameObject);
		}

		//luego se debe de permitir al jugador seleccionar mas cubos
		puedeGirar = true;

		//necesitaba que de un numero 1... me diera un 32
		//                      de un 2... me diera un 72
		//                      de un 3... me diera un 128
		// por eso meti este if tan raro, si el numero de turnos es mayor a estos(32,72,128)... segun el nivel (1,2,3)
		// se active la coroutine de que perdio, por utilizar demaciados movimientos
		if(numeroTurnos> ((((nivelNum*2)+2) * 2)*((nivelNum*2)+2)))
		{
			StartCoroutine (mostrarloseSprite());
		}


	}

}
