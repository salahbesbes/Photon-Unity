using System.Linq;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
	[SerializeField] private Menu[] menus;
	public static CanvasManager Instance;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
			Destroy(gameObject);
	}

	public T getMenu<T>(string name)
	{
		object menu = menus.FirstOrDefault(el => el.menuName == name && el is T);
		if (menu == null) Debug.Log($"no Menu with the name {name} exist");
		return (T)menu;
	}

	public void openMenu(string MenuName)
	{
		foreach (Menu menu in menus)
		{
			if (menu.menuName == MenuName)

				menu.Open();
			else
				menu.close();
		}
	}

	public void Open(Menu selectedMenu)
	{
		openMenu(selectedMenu.menuName);
	}

	public void Close(Menu selectedMenu)
	{
		selectedMenu.close();
	}

	public void showErrorMessage(string message)
	{
		ErrorMenu errorMenu = getMenu<ErrorMenu>("errorMenu");

		if (errorMenu == null) return;

		errorMenu.errorMenuGui.text = message;
		openMenu("errorMenu");
	}
}