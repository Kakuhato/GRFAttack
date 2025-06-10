using UnityEngine;

public interface IVisitor
{
    void Visit<T>(T visitable) where T :  Component,Ivisitable;
}

public interface Ivisitable
{
    void Accept(IVisitor visitor);
}
