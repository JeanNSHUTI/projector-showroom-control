using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropDownHandler : MonoBehaviour
{
    //Declare dropdown reference to get its value
    public Dropdown dropdownAutoDivideScreen;
    // Start is called before the first frame update
    void Start()
    {
        dropdownAutoDivideScreen.onValueChanged.AddListener(delegate {

            dropdownAutoDivideScreenValueChangeEvent(dropdownAutoDivideScreen);
	});
        
    }

    // Update is called once per frame
    void dropdownAutoDivideScreenValueChangeEvent(Dropdown sender)
    {
        //Print value selected in dropdown
        Debug.Log("User selected option: " + sender.value);

        //Print text of selected value
        Debug.Log("User selected option: " + sender.options[sender.value].text);

    }
}
