using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Classes.Static;
using Assets.Scripts.Classes.WorldSingleton;
using ShipInternals;
using NPBehave;
using Action = NPBehave.Action;

public partial class Planet: Static
{
	protected Root behaviorTree;
	private Blackboard blackboard;

	void PlanetBTSetup()
	{
		base.Start();


		behaviorTree = CreatePlanetBT();

		blackboard = behaviorTree.Blackboard;

		// attach the debugger component if executed in editor (helps to debug in the inspector) 
		#if UNITY_EDITOR
		Debugger debugger = (Debugger)this.gameObject.AddComponent(typeof(Debugger));
		debugger.BehaviorTree = behaviorTree;
		#endif
		behaviorTree.Start();
	}

	private Root CreatePlanetBT()
	{
		return new Root (
			new Sequence (
				new Wait (1f),
				new Action (() => {
					CalculateResourcesNeeded ();
				}){ Label = "Calculate the resources needed for the market" },
				new Action (() => {
					CalculateUnwantedResource ();
				}){ Label = "Calculate the resources that doesn't need" },
				new Action (() => {
					UpdateMarketSellingOrder ();
				}){ Label = "Update the market selling order" },
				new Action (() => {
					UpdateMarketBuyingingOrder ();
				}){ Label = "Update the market buying order" },
				new Action (() => {
					SendDeliveryShips ();
				}){ Label = "Send delivery ships for sold resources if ships are available" }
			

			)
		);
	}

	private void CalculateResourcesNeeded()
	{
	}

	private void CalculateUnwantedResource()
	{
	}

	private void UpdateMarketSellingOrder()
	{
	}

	private void UpdateMarketBuyingingOrder()
	{
	}

	private void SendDeliveryShips()
	{
	}

}


