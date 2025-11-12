// ------------------------------------------------------------
// @file       QueryExampleApp.cs
// @brief
// @author     zheliku
// @Modified   2024-10-13 14:10:37
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Core.Example._1.QueryExample.Scripts
{
    using Model;

    public class QueryExampleApp : AbstractArchitecture<QueryExampleApp>
    {
        protected override void Init()
        {
            RegisterModel(new StudentModel());
            RegisterModel(new TeacherModel());
        }
    }
}