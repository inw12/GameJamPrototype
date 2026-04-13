using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    #region BT
    protected enum BTState { Success, Failure, Running }
    protected List<Func<BTState>> BTNodes = new();
    BTState BTRoot() => Selector(BTNodes.ToArray());
    #endregion

    [Header("Debug")]
    protected List<(string, BTState)> DebugNodes = new();
    public bool ShowDebug = false;

    public virtual void Start()
    {

    }

    public virtual void Update()
    {
        BTRoot();
    }

    // Tree
    private BTState Selector(params Func<BTState>[] nodes)
    {
        foreach (var node in nodes)
            if (node() != BTState.Failure) return BTState.Success;
        return BTState.Failure;
    }

    protected Func<BTState> Sequence(params Func<BTState>[] nodes) => () =>
    {
        foreach (var node in nodes)
            if (node() != BTState.Success) return BTState.Failure;
        return BTState.Success;
    };

    void OnGUI()
    {
        if (!ShowDebug) return;

        GUILayout.BeginArea(new Rect(10, 10, 220, 300));
        GUILayout.Label("Behavior Tree");

        foreach (var d in DebugNodes)
        {
            DrawNode(d.Item1, d.Item2);
        }

        GUILayout.EndArea();
    }

    void DrawNode(string label, BTState state)
    {
        Color dot = state == BTState.Success ? Color.green
                  : state == BTState.Failure ? Color.red
                  : Color.gray;

        GUILayout.BeginHorizontal();
        GUI.color = dot;
        GUILayout.Label("●", GUILayout.Width(20));
        GUI.color = Color.white;
        GUILayout.Label(label);
        GUILayout.EndHorizontal();
    }
}