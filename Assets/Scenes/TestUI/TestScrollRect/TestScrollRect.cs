#region namespace
using MP.UI;
using System.Collections.Generic;
using UnityEngine;
#endregion

public struct ContactInfo
{
    public string Name;
    public string Gender;
    public string id;
}

public class TestScrollRect : MonoBehaviour, IRecyclableScrollRectDataSource
{

    [SerializeField]
    RecyclableScrollRect _recyclableScrollRect;

    [SerializeField]
    private int _dataLength;

    //Dummy data List
    private List<ContactInfo> _contactList = new List<ContactInfo>();

    private void Awake()
    {
        InitData();
        _recyclableScrollRect.DataSource = this;
    }

    private void InitData()
    {
        if (_contactList != null) _contactList.Clear();

        string[] genders = { "Male", "Female" };
        for (int i = 0; i < _dataLength; i++)
        {
            ContactInfo obj = new ContactInfo();
            obj.Name = i + "_Name";
            obj.Gender = genders[Random.Range(0, 2)];
            obj.id = "item : " + i;
            _contactList.Add(obj);
        }
    }

    public int GetItemCount()
    {
        return _contactList.Count;
    }

    public void SetCell(ICell cell, int index)
    {
        DemoGridCell item = cell as DemoGridCell;
        DemoGridCell item2 = cell as DemoGridCell;
        item2.ConfigureCell(_contactList[index], index);
        // if(null == item)
        // {
        //     DemoGridCell item2 = cell as DemoGridCell;
        //     item2.ConfigureCell(_contactList[index], index);
        // }
        // else
        // {
        //     item.ConfigureCell(_contactList[index], index);
        // }
       
    }

    public void OnUPdateDatas()
    {
        _dataLength++;
        InitData();
        _recyclableScrollRect.ReloadData();
    }


    public void OnPressTheRoomInfoBtn(int index, object obj = null)
    {
        //throw new System.NotImplementedException();
        Debug.Log("TestScrollRect :: OnPressTheRoomInfoBtn");
    }
}
