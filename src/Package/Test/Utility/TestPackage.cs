﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.R.Package.Definitions;
using Microsoft.VisualStudio.R.Package.Repl;
using Microsoft.VisualStudio.R.Packages.R;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Mocks;

namespace Microsoft.VisualStudio.R.Package.Test.Utility
{
    [ExcludeFromCodeCoverage]
    internal sealed class TestRPackage : IRPackage
    {
        TestServiceProvider _serviceProvider = new TestServiceProvider();

        public RInteractiveWindowProvider InteractiveWindowProvider {
            get {
                throw new NotImplementedException();
            }
        }

        public T FindWindowPane<T>(Type t, int id, bool create) where T : ToolWindowPane {
            return new ToolWindowPaneMock(this) as T;
        }

        public DialogPage GetDialogPage(Type t) {
            throw new NotImplementedException();
        }

        public T GetPackageService<T>(Type t = null) where T : class {
            return _serviceProvider.GetService(t) as T;
        }

        public object GetService(Type t) {
            return _serviceProvider.GetService(t);
        }
    }
}
