using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class UnitsData : ScriptableObject
{
    [SerializeField]private int _row;
    [SerializeField]private int _col;
    [SerializeField]private int _mine;
    [SerializeField]private float _vol;
    [SerializeField]private bool _endGame;
    [SerializeField]private bool _winGame;
    [SerializeField]private float _time;
    [SerializeField]private bool _isTimeDown;
    [SerializeField] private bool _isMute;

    public int Row { get { return _row; } set { _row = value; } }
    public int Col { get { return _col; } set { _col = value; } }
    public int Mine { get { return _mine; } set { _mine = value; } }
    public float Vol { get { return _vol; } set { _vol = value; } }
    public bool EndGame { get { return _endGame; } set { _endGame = value; } }
    public bool WinGame { get { return _winGame; } set { _winGame = value; } }
    public bool IsTimeDown { get { return _isTimeDown; } set { _isTimeDown = value; } }
    public float Time { get { return _time; } set { _time = value; } }
    public bool IsMute
    {
        get { return _isMute; }
        set { _isMute = value; }
    }
}
