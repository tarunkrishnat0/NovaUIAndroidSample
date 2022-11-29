using Nova;
using NovaSamples.Inventory;
using UnityEngine;

public class CardVisuals : ItemVisuals
{
    public UIBlock2D Image;
    public TextBlock Title;
    public UIBlock2D CountFillBar;

    public void Bind(InventoryItem data)
    {
        if(data.IsEmpty)
        {
            Image.SetImage((Texture2D)null);
            Title.Text = "";
            CountFillBar.Size.X.Percent = 1;
            return;
        }

        Title.Text = data.Item.Name;
        CountFillBar.Size.X.Percent = Mathf.Clamp01((float)data.Count / InventoryItem.MaxItemsPerSlot);
        Image.SetImage(data.Item.Image);
    }
}
