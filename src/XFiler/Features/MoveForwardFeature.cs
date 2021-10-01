namespace XFiler.Features;

internal class MoveForwardFeature : CommandFeature<TabItemModel>
{
    public override string GetId() => "61e1ddf0-50db-41cc-a794-78f74f34cfbd";
        
    public override bool CanExecute(TabItemModel arg)
    {
        return base.CanExecute(arg);
    }

    public override void Execute(TabItemModel arg)
    {
            
    }
}