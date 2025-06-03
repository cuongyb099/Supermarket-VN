using Core.Interact;

public class ShelfTier : InteractObject
{
    protected override void OnInteract(Interactor source)
    {
        var currentItem = source.CurrentObjectInHand;
        
        if(!currentItem || currentItem is not Box box) return;
        
        
    }
}
