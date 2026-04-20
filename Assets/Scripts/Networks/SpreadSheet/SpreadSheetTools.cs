using Google.Apis.Sheets.v4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class SpreadSheetTools
{
    public int IndextoSSColumn(int indexNum)
    {
        return indexNum + (int)AllDirs.GetInstance().SpreadSheetStartCellPos.x;
    }

    public int IndextoSSRow(int indexNum)
    {
        return indexNum + (int)AllDirs.GetInstance().SpreadSheetStartCellPos.y;
    }
}
