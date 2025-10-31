namespace Inventory.UI
{
    public interface IInventoryView
    {
        void Show();
        void Hide();
        void Refresh();
        void On_SlotSelected(int index);
    }
}
