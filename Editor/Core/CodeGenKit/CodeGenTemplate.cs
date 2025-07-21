// ------------------------------------------------------------
// @file       CodeGenTemplate.cs$
// @brief
// @author     zheliku
// @Modified   2025-07-20 04:09:16
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.CodeGenKit.Editor
{
    public class CodeGenTemplate
    {
        public static readonly string VIEW_CODE = @"// ------------------------------------------------------------
// @file       CodeTemplate.cs
// @brief
// @author     zheliku
// @Modified   __modifiedTime__
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------

namespace __Game__
{
    using Framework.Core;

    public class CodeTemplate : AbstractView
    {
        protected override IArchitecture _Architecture { get => null; }
    }
}";

        public static readonly string ARCHITECTURE_CODE = @"// ------------------------------------------------------------
// @file       CodeTemplate.cs
// @brief
// @author     zheliku
// @Modified   __modifiedTime__
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------

namespace __Game__
{
    using Framework.Core;

    public class ArchitectureTemplate : AbstractArchitecture<ArchitectureTemplate>
    {
        protected override void Init()
        {
            
        }
    }
}";
    }
}