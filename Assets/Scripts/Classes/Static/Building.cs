using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ShipInternals;

public class Building
{
	[SerializeField]
	private List<Building> _buildings;
	private List<CargoItem> _cargos;

	private CargoHold resources;

	private float nextActionTime = 0.0f;
	public float period = 1.0f;

	string [] resourceType = new string[4];
	List<CargoItem> cargoList= new List<CargoItem>();

	void Start()
	{
		
		resourceType [0] = "Iron";
		resourceType [1] = "Steel";
		resourceType [2] = "Food";
		resourceType [3] = "Wool";


	}

	public void factoryProduce()
	{
		
		resources.addHoldType("Iron");

		_buildings.Add (cargoList);


	}



	void Update () {
		if (Time.time > nextActionTime ) {
			nextActionTime += period;
			cargoList.AddRange (resourceType);

		}
	}



}