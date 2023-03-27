using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class UnitsData : ScriptableObject
{
    [SerializeField]
    private int _row;
    [SerializeField]
    private int _col;
    [SerializeField]
    private int _mine;
    [SerializeField]
    private float _vol;

    public int Row { get { return _row; } set { _row = value; } }
    public int Col { get { return _col; } set { _col = value; } }
    public int Mine { get { return _mine; } set { _mine = value; } }
    public float Vol { get { return _vol; } set { _vol = value; } }
}
