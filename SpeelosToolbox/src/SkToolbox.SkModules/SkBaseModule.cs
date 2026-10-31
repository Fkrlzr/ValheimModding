using System.Collections.Generic;
using SkToolbox.Utility;
using UnityEngine;

namespace SkToolbox.SkModules;

public class SkBaseModule : MonoBehaviour
{
	internal SkMenuController SkMC;

	public string ModuleName = "UNNAMED";

	public SkMenu MenuOptions { get; set; } = new SkMenu();


	public SkMenuItem CallerEntry { get; set; } = new SkMenuItem();


	public SkUtilities.Status ModuleStatus { get; set; }

	public void Awake()
	{
		SkMC = ((Component)this).GetComponent<SkMenuController>();
	}

	public List<SkMenuItem> FlushMenu()
	{
		return MenuOptions.FlushMenu();
	}

	public void RequestMenu()
	{
		SkMC.RequestSubMenu(MenuOptions.FlushMenu());
	}

	public void RequestMenu(SkMenu Menu)
	{
		SkMC.RequestSubMenu(Menu);
	}

	public void RemoveModule()
	{
		Object.Destroy((Object)(object)this);
	}

	public void Ready()
	{
		ModuleStatus = SkUtilities.Status.Ready;
	}

	public void Loading()
	{
		ModuleStatus = SkUtilities.Status.Loading;
	}

	public void Error()
	{
		ModuleStatus = SkUtilities.Status.Error;
	}

	public void Unload()
	{
		ModuleStatus = SkUtilities.Status.Unload;
	}
}
