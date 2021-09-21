using System;
public class RunAction : Node
{
    private Action action;

    public RunAction(Action action)
    {
        this.action = action;
    }

    public void Cancel()
    {
        return; //Nothing
    }

    public void Run(ParentNode parent)
    {
        action.Invoke();
        parent.HandleChildComplete();
    }
}
