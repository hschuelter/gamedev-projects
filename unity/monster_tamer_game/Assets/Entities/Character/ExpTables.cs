using System.Collections.Generic;

public static class ExpTables
{
    public static readonly int[] ExpToNextLevel =
    {
        0,
        10, // 1 -> 2
        15,
        24,
        36,
        50,
        70,
        95,
        110,
        145,
        180, // 10
        220,
        260,
        300,
        360,
        420
    };

    public static int GetExpForLevel(int level)
    {
        if (level < 1 || level > ExpToNextLevel.Length)
            return 0;
        return ExpToNextLevel[level];
    }
}
