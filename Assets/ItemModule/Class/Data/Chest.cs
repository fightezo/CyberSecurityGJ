using ItemModule.Class;
using ItemModule.Class.Data;

public abstract class Chest : CitizenItem
{
    private string _password;
    private ItemState _currentState;
    

    public string GetPassword()
    {
        return _password;
    }
    public void SetPassword(string password)
    {
        _password = password;
    }
    
    public override ItemState GetItemState()
    {
        return _currentState;
    }

    public override void SetState(ItemState newState)
    {
        _currentState = newState;
    }
    
    public override ItemCategory GetItemCategory()
    {
        return ItemCategory.Placement;
    }
}
