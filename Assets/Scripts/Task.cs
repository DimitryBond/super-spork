using System;
using System.Collections.Generic;

[Serializable]
public class Task
{
    public int TaskId;
    public List<Symbol> Symbols;
    public string TaskDescription;
}