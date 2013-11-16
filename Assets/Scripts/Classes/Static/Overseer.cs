using UnityEngine;
using System.Collections;
using EntityParts;

public class Overseer : Static
{
    float timeScaleOriginal;
    //float timeScale;
    void Start()
    {
        CargoItemTypes.AddCargoItemType(new CargoItem("Ice"));
        CargoItemTypes.AddCargoItemType(new CargoItem("Rock"));
        CargoItemTypes.AddCargoItemType(new CargoItem("Iron"));
        CargoItemTypes.AddCargoItemType(new CargoItem("Titanium"));
        CargoItemTypes.AddCargoItemType(new CargoItem("Gold"));

        timeScaleOriginal = Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Mobile.inTime)
        {
            Mobile.inTime = false;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Mobile.inTime = true;
        }
    }
}
