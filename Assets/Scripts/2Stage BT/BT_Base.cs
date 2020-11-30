using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
public abstract class Node
{
    public abstract bool Invoke();
}
public class CompositeNode : Node
{
    public override bool Invoke()
    {
        throw new NotImplementedException();
    }

    public void AddChild(Node node)
    {
        childrens.Push(node);
    }

    public Stack<Node> GetChildrens()
    {
        return childrens;
    }
    private Stack<Node> childrens = new Stack<Node>();
}

public class Selector : CompositeNode
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

public class Sequence : CompositeNode
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

public class IsDead : Node
{
    public EnemyMove Enemy
    {
        set { _Enemy = value; }
    }
    private EnemyMove _Enemy;
    public override bool Invoke()
    {
        return _Enemy.IsDead();
    }
}

public class Phase1to2 : Node
{
    public EnemyMove Enemy
    {
        set { _Enemy = value; }
    }
    private EnemyMove _Enemy;
    public override bool Invoke()
    {
        return _Enemy.Phase1to2();

    }
}
public class Phase2to3 : Node
{
    public EnemyMove Enemy
    {
        set { _Enemy = value; }
    }
    private EnemyMove _Enemy;
    public override bool Invoke()
    {
        return _Enemy.Phase2to3();

    }
}

public class IsCooltime : Node
{
    public EnemyMove Enemy
    {
        set { _Enemy = value; }
    }
    private EnemyMove _Enemy;
    public override bool Invoke()
    {
        return _Enemy.IsCooltime();
    }
}
public class MonsterRotation : Node
{
    public EnemyMove Enemy
    {
        set { _Enemy = value; }
    }
    private EnemyMove _Enemy;
    public override bool Invoke()
    {
        return _Enemy.MonsterRotation();
    }
}