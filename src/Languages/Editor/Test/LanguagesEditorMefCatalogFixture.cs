﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.Common.Core.Extensions;
using Microsoft.Common.Core.Shell;
using Microsoft.Common.Core.Test.Fakes.Shell;
using Microsoft.Common.Core.Test.Fixtures;
using Microsoft.Common.Core.Test.StubBuilders;
using Microsoft.Languages.Editor.Shell;
using Microsoft.Languages.Editor.Test.Fakes.Shell;
using Microsoft.R.Components.Settings;
using Microsoft.UnitTests.Core.Mef;
using Microsoft.UnitTests.Core.XUnit;
using Xunit.Sdk;

namespace Microsoft.Languages.Editor.Test {
    // Fixture doesn't import itself. Use AssemblyFixtureImportAttribute
    [ExcludeFromCodeCoverage]
    public class LanguagesEditorMefCatalogFixture : AssemblyMefCatalogFixture {
        protected override IEnumerable<string> GetAssemblies() {
            return new[] {
                "Microsoft.VisualStudio.CoreUtility.dll",
                "Microsoft.VisualStudio.Text.Data.dll",
                "Microsoft.VisualStudio.Text.Internal.dll",
                "Microsoft.VisualStudio.Text.Logic.dll",
                "Microsoft.VisualStudio.Text.UI.dll",
                "Microsoft.VisualStudio.Text.UI.Wpf.dll",
                "Microsoft.Languages.Editor.dll",
                "Microsoft.R.Host.Client.dll",
                "Microsoft.R.Common.Core.dll",
                "Microsoft.R.Common.Core.Test.dll",
                "Microsoft.R.Components.dll",
                "Microsoft.R.Components.Test.dll",
                "Microsoft.VisualStudio.Editor.dll",
                "Microsoft.VisualStudio.Language.Intellisense.dll",
                "Microsoft.VisualStudio.Platform.VSEditor.dll"
            };
        }

        protected override void AddValues(CompositionContainer container, string testName) {
            base.AddValues(container, testName);
            var editorShell = new TestEditorShell(container);
            var batch = new CompositionBatch()
                .AddValue(FileSystemStubFactory.CreateDefault())
                .AddValue<ICoreShell>(editorShell)
                .AddValue<IEditorShell>(editorShell)
                .AddValue(editorShell);
            container.Compose(batch);
        }
    }
}
