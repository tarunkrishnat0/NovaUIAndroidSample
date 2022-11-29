using Nova;
using NovaSamples.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawnCtrl : MonoBehaviour
{
    [SerializeField] private ItemDatabase _inventoryDataBase = null;
    [SerializeField] private GridView _clickableGrid = null;
    [SerializeField] private GridView _nonClickableGrid = null;
    [SerializeField] private UIBlock2D _clickableButton;
    [SerializeField] private UIBlock2D _nonClickableButton;

    private List<InventoryItem> _clickableCardsInventory = null;
    private List<InventoryItem> _nonClickableCardsInventory = null;

    // Start is called before the first frame update
    void Start()
    {
        _clickableCardsInventory = _inventoryDataBase.GetRandomItems(60);
        _nonClickableCardsInventory = _inventoryDataBase.GetRandomItems(60);

        _clickableButton.Gradient.Enabled = false;
        _nonClickableButton.Gradient.Enabled = true;

        _clickableButton.AddGestureHandler<Gesture.OnClick>(HandleButtonPress);
        _nonClickableButton.AddGestureHandler<Gesture.OnClick>(HandleButtonPress);

        _clickableGrid.gameObject.SetActive(false);
        _nonClickableGrid.gameObject.SetActive(true);

        //_clickableGrid.AddDataBinder<InventoryItem, CardVisuals>(HandleBind);
        //_clickableGrid.AddGestureHandler<Gesture.OnClick, CardVisuals>(HandleClick);
        //_clickableGrid.SetDataSource(_clickableCardsInventory);

        _nonClickableGrid.AddDataBinder<InventoryItem, CardVisuals>(HandleBind);
        _nonClickableGrid.AddGestureHandler<Gesture.OnClick, CardVisuals>(HandleClick);
        _nonClickableGrid.SetDataSource(_nonClickableCardsInventory);
    }

    private void HandleButtonPress(Gesture.OnClick evt)
    {
        Debug.Log("HandleButtonPress : " + evt.Receiver.gameObject.name);
        if(evt.Receiver.gameObject.name == "Clickable")
        {
            _clickableButton.Gradient.Enabled = true;
            _nonClickableButton.Gradient.Enabled = false;

            _nonClickableGrid.gameObject.SetActive(false);
            _clickableGrid.gameObject.SetActive(true);
            if(_clickableGrid.DataSourceItemCount <= 0)
            {
                _clickableGrid.AddDataBinder<InventoryItem, CardVisuals>(HandleBind);
                _clickableGrid.AddGestureHandler<Gesture.OnClick, CardVisuals>(HandleClick);
                _clickableGrid.SetDataSource(_clickableCardsInventory);
            }
        } else
        {
            _clickableButton.Gradient.Enabled = false;
            _nonClickableButton.Gradient.Enabled = true;

            _clickableGrid.gameObject.SetActive(false);
            _nonClickableGrid.gameObject.SetActive(true);
        }
    }

    private void HandleBind(Data.OnBind<InventoryItem> evt, CardVisuals target, int index) => target.Bind(evt.UserData);

    private void HandleClick(Gesture.OnClick evt, CardVisuals target, int index)
    {
        Debug.Log("HandleClick : " + target.Title.Text);

        ScaleAnimations anim = new ScaleAnimations()
        {
            StartValue = 1f,
            EndValue = 1.2f,
            TransformToScale = evt.Receiver.gameObject.transform,
        };

        anim.Run(0.3f);
    }
}
