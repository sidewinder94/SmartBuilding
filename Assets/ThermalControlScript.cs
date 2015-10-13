using UnityEngine;
using System.Collections;
using System.Linq;

public class ThermalControlScript : MonoBehaviour
{


    private GameObject _hotter;
    private GameObject _colder;
    private TextMesh _temperature;

    private RoomScript _roomScript;

    void Start()
    {
        _roomScript = GameObject.FindGameObjectsWithTag(tag).First(n => n.name.Contains("Detector")).GetComponent<RoomScript>();

        foreach (var comp in GetComponentsInChildren<BoxCollider>())
        {
            if (comp.gameObject.name == "Hotter" && _hotter == null) _hotter = comp.gameObject;
            if (comp.gameObject.name == "Colder" && _colder == null) _colder = comp.gameObject;
        }

        _temperature = GetComponentInChildren<TextMesh>();

        _temperature.text = _roomScript.TargetTemperature.ToString();

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000))
            {
                if (hit.transform.gameObject.GetInstanceID().Equals(_hotter.GetInstanceID()) && _roomScript.TargetTemperature < 40d)
                {
                    _roomScript.TargetTemperature += 0.5d;
                    _temperature.text = _roomScript.TargetTemperature.ToString();
                }
                else if (hit.transform.gameObject.GetInstanceID().Equals(_colder.GetInstanceID()) && _roomScript.TargetTemperature > 5d)
                {
                    _roomScript.TargetTemperature -= 0.5d;
                    _temperature.text = _roomScript.TargetTemperature.ToString();
                }
            }
        }
    }
}
