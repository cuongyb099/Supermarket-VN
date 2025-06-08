namespace KatJsonInventory.Item
{
    public interface IItemStack
    {
        public int Quantity { get; set; }
        public int MaxStack { get; protected set; }
    }
}
