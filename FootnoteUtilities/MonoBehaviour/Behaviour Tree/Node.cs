using System;

public interface Node
{
    void Run(ParentNode parent);
    void Cancel();
}
