using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour 
{


	// aqui estaran todos los prefabs de cada dibujo
	public List<GameObject> tilesprefabs;

	public List<Transform> posicionesdeloscubosenescena;

	public GameObject empty;

	// este se le asignara segun un random range, uno del os tilesprefabs
	public GameObject cuboseleccionado;

	public bool cambiacubo = true;
	public Vector3 seccion;

	public float limiteMovimientos;
	public Text moveLimit;


	//=========================================================

	void Start ()
	{
		StartLevel ();
		// ya que startlevel, se llamara tanto como al iniciar el juego... como al pasar al siguiente nivel,
		// lo converti en un metodo, y lo llame desde un start
	}

	public void StartLevel ()
	{
		//antes intente crear esta variable en el game manager y llamarla desde esta... pero siempre sucedia algo que me impedia.. por lo que termine poniendolo asi
		limiteMovimientos = ((((GameManager.Instance.nivelNum * 2) + 2) * 2) * ((GameManager.Instance.nivelNum * 2) + 2));
		//si el numero de nivel es (1,2,3) los limites de movimientos son (32,72,128)... 

		// este al iniciar cada vez un nivel... mostrara al jugador cuantos movimientos maximos puede realizar para ganar.
		moveLimit.text = "limite de movimientos " + limiteMovimientos.ToString ();

		GameManager.Instance.StartCoroutine(GameManager.Instance. mostrarNextLevelSprite ());
		//cada vez que inicie un nivel nuevo... se llamara la funcion de mostrar este sprite, informando que el siguiente nivel se acerca.

		int boardSize = (GameManager.Instance. nivelNum * 2)+2;
		//dependiendo del numero del nivel en que se esté... cambiara el tamaño del tablero, y este valor se guardará en la variable boardsize.

		//como los tiles se empezaran a instanciar desde las posiciones 0 tanto en x y y... e iran aumentando... el tablero que estos formaran... 
		//será mas grande que los elementos decorativos en el escenario... por lo que tuve que convertirlos en hijos de un game object dueño de este script... y asi escalandolo y moviendolo... se ajustaran 
		// para que queden acomodados todos los tiles prefabs dentro del muro y las dos columnas decorativas
		transform.localScale = new Vector3(boardSize,boardSize,boardSize);
		transform.position = new Vector3 (GameManager.Instance.nivelNum+0.5f,GameManager.Instance.nivelNum+0.5f,0);

		// aqui utilizando dos for, se instanciaran en orden en cada posicion... unos emptys, y estos se guardaran en una lista... que almacena sus respectivas posiciones del transform
		// los emptys tienen un script que los destruye una vez el jugador empieza a seleccionar cubos... por lo que ya no necesitan ser destruidos despues.
		for (int x = 0; x < boardSize; x++) 
		{
			for (int y = 0; y < boardSize; y++) 
			{
				seccion = new Vector3 (x, y, 0);

				posicionesdeloscubosenescena.Add (Instantiate (empty,seccion,Quaternion.identity).transform);
			}
		}

		// luego de que se instanciaron los emptys en todas las posiciones del tablero... 
		//se procede a instanciar los tiles en sus posiciones al azar...
		Asignartag (boardSize);

	}

	//================================================================


	public void Asignartag (int boardSize)
	{
		//este for se repetira (la mitad de la todtalidad de cubos en escena) de veces. 
		for (int i =0 ; i<(boardSize*(boardSize/2)); i++)
		{
			//en una variable j de... las posiciones disponibles para instanciar un cubo
			// se ordena agarrar una posicion al azar y asignarla como argumento a la funcion agarrartransformalazar
			// y luego se ordena remover esta de la lista de posiciones disponibles, para evitar instanciar un tile, en otro tile existente.

			int j = UnityEngine.Random.Range (0, posicionesdeloscubosenescena.Count);
			Agarrartransformalazar (j);
			posicionesdeloscubosenescena.Remove (posicionesdeloscubosenescena[j]);

			//como no podia reasignarle un valor a j... tuve que crear otra variable llamada j2
			int j2 = UnityEngine.Random.Range (0, posicionesdeloscubosenescena.Count);
			Agarrartransformalazar (j2);
			posicionesdeloscubosenescena.Remove (posicionesdeloscubosenescena[j2]);

		}	
	}

	//esta funcion solamente instancia un tileprefab en la posicion del (empty almacenado en la listade posiciones disponibles)
	//Y utilizando un if una variable tipo bool... el tile prefab cambia por un random range... cada dos veces que se llama
	public void Agarrartransformalazar (int jota)
	{
		if (cambiacubo == true) 
		{
			int x = UnityEngine.Random.Range (0, tilesprefabs.Count);
			cuboseleccionado = tilesprefabs [x];
		}

		Instantiate (cuboseleccionado,(posicionesdeloscubosenescena[jota].position),Quaternion.identity);
		cambiacubo = !cambiacubo;
	}


}
	

