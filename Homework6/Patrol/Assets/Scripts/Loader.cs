using UnityEngine;

public class Loader : MonoBehaviour
{
	public GameObject GuiManager;
	public GameObject GameManager;

	private void Awake()
	{
		if (Managers.GuiManager.Instance == null)
		{
			Instantiate(GuiManager);
		}

		if (Managers.GameManager.Instance == null)
		{
			Instantiate(GameManager);
		}
	}
}