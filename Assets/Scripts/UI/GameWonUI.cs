using UnityEngine;

public class GameWonUI : UserInterface
{
    // Start is called before the first frame update
    void Start()
    {
		GUI.gameWonUI = this;
        this.gameObject.SetActive(false);
    }

	/// <summary>
	/// Keep playing button event handler. Attached to the button in Unity Editor.
	/// Closes the game won UI and goes back to the game.
	/// </summary>
	public void KeepPlayingButtonOnClick()
	{
		GUI.HideGameWonUI();
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
