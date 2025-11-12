// ------------------------------------------------------------
// @file       JsonExample.cs
// @brief
// @author     zheliku
// @Modified   2024-12-07 00:12:57
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

using UnityEngine;

namespace Framework3.Toolkits.Core.DataKit.Example._1Json
{
    #if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
    using Toolkits.DataKit;

    public class ExcelExample : MonoBehaviour
    {
        #if ODIN_INSPECTOR
[ShowInInspector]
#endif
        public ExcelSheet Sheet = new ExcelSheet();

        public string FileName  = "example";
        public string SheetName = "example";

        public void SaveExcel()
        {
            Sheet[3, 2] = "hello world";
            Sheet.Save(FileName, SheetName);
        }
        
        public void LoadExcel()
        {
            Sheet.Load(FileName, SheetName);
            Debug.Log($"{Sheet.Start} -> {Sheet.End}");
        }
        
        public void SaveCsv()
        {
            Sheet[2, 4] = "hello world";
            Sheet.Save(FileName, SheetName, ExcelFormat.Csv);
        }
        
        public void LoadCsv()
        {
            Sheet.Load(FileName, SheetName, ExcelFormat.Csv);
            Debug.Log($"{Sheet.Start} -> {Sheet.End}");
        }
        
        public void OpenFolder()
        {
            ExcelSheet.OpenExcelSavePath();
        }
    }
}