namespace KatJsonInventory.Item
{
    public interface IInstanceItem
    {
        public void Init(ItemData itemData);
        public ItemData GetItemData();
    }
}
