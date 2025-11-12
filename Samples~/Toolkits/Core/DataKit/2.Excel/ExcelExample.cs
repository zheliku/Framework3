// ------------------------------------------------------------
// @file       JsonExample.cs
// @brief
// @author     zheliku
// @Modified   2024-12-07 00:12:57
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.Core.DataKit.Example._1Json
{
    using Toolkits.DataKit;
    using UnityEngine;
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

    public class ExcelExample : MonoBehaviour
    {
        public string FileName  = "example";
        public string SheetName = "example";
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public ExcelSheet Sheet = new();

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
        #if UNITY_EDITOR
            ExcelSheet.OpenExcelSavePath();
        #endif
        }
    }
}