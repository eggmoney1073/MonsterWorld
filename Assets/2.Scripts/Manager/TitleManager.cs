using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnums;

public class TitleManager : SingletonGameobject<TitleManager>
{
    [SerializeField] Canvas _saveCanvas;
    [SerializeField] UIMessage _messageUI;
    UISaveSlot _slot1;
    UISaveSlot _slot2;
    UISaveSlot _slot3;

    void Start()
    {
        _saveCanvas.gameObject.SetActive(false);

        _slot1 = _saveCanvas.transform.GetChild(1).GetChild(0).GetComponent<UISaveSlot>();
        _slot2 = _saveCanvas.transform.GetChild(1).GetChild(1).GetComponent<UISaveSlot>();
        _slot3 = _saveCanvas.transform.GetChild(1).GetChild(2).GetComponent<UISaveSlot>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Button_CloseWindow();
    }

    public void Button_CloseWindow()
    {
        _saveCanvas.gameObject.SetActive(false);
    }

    public void Button_Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Button_NewGame()
    {
        _saveCanvas.gameObject.SetActive(true);
        DataManager._Instance.LoadAllData();
        SetAllSlots(true);
    }

    public void Button_LoadGame()
    {
        _saveCanvas.gameObject.SetActive(true);
        DataManager._Instance.LoadAllData();
        SetAllSlots();
    }

    public void ClickEmptySlot()
    {
        _messageUI.ShowWindow();
    }

    void SetAllSlots(bool isNew = false)
    {
        _slot1.SetSlot(Slot.Slot1, isNew);
        _slot2.SetSlot(Slot.Slot2, isNew);
        _slot3.SetSlot(Slot.Slot3, isNew);
    }
}
