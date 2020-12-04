using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
public abstract class Node1
{
    public abstract bool Invoke();
}
public class CompositeNode1 : Node1
{
    public override bool Invoke()
    {
        throw new NotImplementedException();
    }

    public void AddChild(Node1 node)
    {
        childrens.Push(node);
    }

    public Stack<Node1> GetChildrens()
    {
        return childrens;
    }
    private Stack<Node1> childrens = new Stack<Node1>();
}

public class Selector1 : CompositeNode1
{
    public override bool Invoke()
    {
        foreach (var node in GetChildrens())
        {
            if (node.Invoke())
            {

                return true;
            }
        }
        return false;
    }
}

public class Sequence1 : CompositeNode1
{
    public override bool Invoke()
    {
        bool p = false;
        foreach (var node in GetChildrens())
        {
            if (node.Invoke() == false)
            {
                p = true;
            }
        }
        return !p;
    }
}

public class IsDead1 : Node1
{
    public EnemyMove1 Enemy
    {
        set { _Enemy = value; }
    }
    private EnemyMove1 _Enemy;
    public override bool Invoke()
    {
        return _Enemy.IsDead();
    }
}


public class IsCooltime1 : Node1
{
    public EnemyMove1 Enemy
    {
        set { _Enemy = value; }
    }
    private EnemyMove1 _Enemy;
    public override bool Invoke()
    {
        return _Enemy.IsCooltime();
    }
}

public class MonsterRotation1 : Node1
{
    public EnemyMove1 Enemy
    {
        set { _Enemy = value; }
    }
    private EnemyMove1 _Enemy;
    public override bool Invoke()
    {
        return _Enemy.MonsterRotation();
    }
}
