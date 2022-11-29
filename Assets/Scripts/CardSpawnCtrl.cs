using Nova;
using NovaSamples.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawnCtrl : MonoBehaviour
{
    [SerializeField] private ItemDatabase _inventoryDataBase = null;
    [SerializeField] private GridView _grid = null;

    private List<InventoryItem> _cardsInventory = null;

    // Start is called before the first frame update
    void Start()
    {
        _cardsInventory = _inventoryDataBase.GetRandomItems(30);
        _grid.AddDataBinder<InventoryItem, CardVisuals>(HandleBind);
        _grid.AddGestureHandler<Gesture.OnClick, CardVisuals>(HandleClick);
        _grid.SetDataSource(_cardsInventory);
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
