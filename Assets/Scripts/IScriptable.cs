namespace DefaultNamespace
{
    public interface IScriptable
    {
        DelegateHelper.GetController getController { get; set; }
        DelegateHelper.SetController setController { get; set; }
    }
}