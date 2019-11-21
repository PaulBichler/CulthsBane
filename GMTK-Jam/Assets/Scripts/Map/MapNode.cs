using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode
{
    private List<MapNode> children;
    private int level;
    private GameObject icon;
    private bool playable = true;

    public MapNode(int level = -1)
    {
        this.level = level;
        if (this.level == -1)
        {
            this.level = Random.Range(1, 3);
        }
        children = new List<MapNode>();
    }

    public void setChildren(MapNode child)
    {
        child.setPlayable(false);
        children.Add(child);
    }

    public int getLevel()
    {
        return level;
    }

    public List<MapNode> getChildren()
    {
        return children;
    }

    public void setGameObject(GameObject gameObject)
    {
        icon = gameObject;
    }

    public GameObject getGameObject()
    {
        return icon;
    }

    public void setPlayable(bool playable)
    {
        this.playable = playable;
    }

    public bool getPlayable()
    {
        return playable;
    }
}
