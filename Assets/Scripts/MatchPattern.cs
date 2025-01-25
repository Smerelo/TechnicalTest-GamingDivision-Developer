using System;
using System.Collections.Generic;

[Serializable]
public class BoolRow
{
    public List<bool> Row;

    public BoolRow(int size)
    {
        Row = new List<bool>(new bool[size]);
    }

    public BoolRow(List<bool> row)
    {
        Row = row;
    }
}

[Serializable]
public class MatchPattern
{
    public List<BoolRow> Pattern; // Use a wrapper for the inner list

    public MatchPattern(List<List<bool>> patternData)
    {
        Pattern = new List<BoolRow>();
        foreach (var row in patternData)
        {
            Pattern.Add(new BoolRow(row)); // Wrap each inner list
        }
    }
}

[Serializable]
public class MatchPatternCollection
{
    public List<MatchPattern> MatchPatterns = new List<MatchPattern>();
}