using UnityEngine;

public class PauseMenuUI : UserInterface
{
    // Start is called before the first frame update
    void Start()
    {
		GUI.pauseMenuUI = this;
        this.gameObject.SetActive(false);
    }

	/// <summary>
	/// Back button event handler. Attached to the button in Unity Editor.
	/// Closes the pause UI and goes back to the game.
	/// </summary>
	public void BackButtonOnClick()
	{
		GUI.HidePauseMenu();
	}

	/// <summary>
	/// Exit button event handler. Attached to the button in Unity Editor.
	/// Closes the game application and eventually exists from play mode.
	/// </summary>
	public void ExitButtonOnClick()
	{
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}
}
