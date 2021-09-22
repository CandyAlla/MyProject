using MP.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoGridCell : MonoBehaviour, ICell
{

    public Text nameLabel;

    //Model
    private ContactInfo _contactInfo;
    private int _cellIndex;
    private void Start()
    {
        //Can also be done in the inspector
        GetComponent<Button>().onClick.AddListener(ButtonListener);
    }

    //This is called from the SetCell method in DataSource
    public void ConfigureCell(ContactInfo contactInfo, int cellIndex)
    {
        _cellIndex = cellIndex;
        _contactInfo = contactInfo;

        nameLabel.text = contactInfo.Name;

    }


    private void ButtonListener()
    {
        Debug.Log("Index : " + _cellIndex + ", Name : " + _contactInfo.Name + ", Gender : " + _contactInfo.Gender);
    }
}
