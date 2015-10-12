using UnityEngine;
using System.Collections;

public class ThermalControlScript : MonoBehaviour {

	public GameObject room;
	public GameObject hotter;
	public GameObject colder;
	public TextMesh temperature;

	private RoomScript roomScript;

	void Start () {
		roomScript = room.GetComponent<RoomScript>();
		temperature.text = roomScript.TargetTemperature.ToString();
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetMouseButtonDown(0)){
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast (ray, out hit, 1000)) {
				if (hit.transform.gameObject.GetInstanceID().Equals(hotter.GetInstanceID()) && roomScript.TargetTemperature < 40d)
				{
					roomScript.TargetTemperature += 0.5d;
					temperature.text = roomScript.TargetTemperature.ToString();
				}
				else if (hit.transform.gameObject.GetInstanceID().Equals(colder.GetInstanceID())&& roomScript.TargetTemperature > 5d)
				{			
					roomScript.TargetTemperature -= 0.5d;
					temperature.text = roomScript.TargetTemperature.ToString();
				}
			}
		}
	}
}
