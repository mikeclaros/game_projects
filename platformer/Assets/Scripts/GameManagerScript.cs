using UnityEngine;
using System.Collections;

public enum MenuTypes : byte{
	MainMain = 0,
	OptionsMenu = 1,
	PauseMenu = 2,
	GameOverMenu = 3,
}

public class GameManagerScript : MonoBehaviour {
	public GameManagerScript(){
		MenuFunctions = new GUI.WindowFunction[]{
			MainMenu, 		//0
			OptionsMenu,	//1
			PauseMenu,		//2
			GameOverMenu,	//3
		};
	}
	public AudioClip clickSound;
	private AudioSource m_soundSource;
	private Settings m_Settings = new Settings();
	public bool isMenuActive {get;set;}
	public MenuTypes ActiveMenu { get; set;}
	private readonly GUI.WindowFunction[] MenuFunctions = null;
	private readonly string[] MenuNames = new string[]{
		"Main Menu",		//0
		"Options Menu",		//1
		"Pause Menu",		//2
		"Game Over Menu",	//3
	};
	void Awake() {
		ActiveMenu = MenuTypes.MainMain;
		isMenuActive = true;
		Application.runInBackground = true;
		DontDestroyOnLoad(gameObject);
		m_soundSource = Camera.main.transform.FindChild ("Sound").GetComponent<AudioSource>();
		m_Settings.Load (
			Camera.main.transform.FindChild ("Music").GetComponent<AudioSource>(),
			m_soundSource
		);
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnGUI(){
		const int Width = 400;
		const int Height = 300;

		if(isMenuActive){
			Rect windowRect = new Rect( (Screen.width - Width) / 2,
			                           	(Screen.height - Height) / 2,
			                           	Width, Height);
			GUILayout.Window (0, windowRect, MenuFunctions[(byte)ActiveMenu], MenuNames[(byte)ActiveMenu]);
		}
	}

	private void MainMenu(int id){

		if(GUILayout.Button("Start Game")){
			m_soundSource.PlayOneShot(clickSound);
			isMenuActive = false;
		}
		if(GUILayout.Button("Options")){
			m_soundSource.PlayOneShot(clickSound);
//			m_sourceMenu = MenuType.MainMenu;
			ActiveMenu = MenuTypes.OptionsMenu;
		}
		if(!Application.isWebPlayer && !Application.isEditor){
			if(GUILayout.Button ("Exit")){
				m_soundSource.PlayOneShot(clickSound);
				Application.Quit();
			}
		}
	}

	private void OptionsMenu(int id){
		GUILayout.BeginHorizontal();
		GUILayout.Label("Music Volume: ", GUILayout.Width(90));
		m_Settings.musicVolume = GUILayout.HorizontalSlider(m_Settings.musicVolume, 0.0f, 1.0f);
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("Sound Volume: ", GUILayout.Width(90));
		m_Settings.soundVolume = GUILayout.HorizontalSlider(m_Settings.soundVolume, 0.0f, 1.0f);
		GUILayout.EndHorizontal();

		if(GUILayout.Button("Reset High Score")){
			m_soundSource.PlayOneShot(clickSound);
			m_Settings.HighScore = 0;
		}
		if(GUILayout.Button("Back")){
			m_soundSource.PlayOneShot(clickSound);
			m_Settings.Save();
			ActiveMenu = MenuTypes.MainMain; //m_sourceMenu;
		}
	}
	private void PauseMenu(int id){
	}
	private void GameOverMenu(int id){
	}


}