﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Diagnostics;
using System.Globalization;
using Microsoft.Common.Core;
using Microsoft.Common.Core.Logging;
using Microsoft.Common.Core.OS;
using Microsoft.Office.Interop.Outlook;
using Microsoft.VisualStudio.R.Package.Commands;
using Microsoft.VisualStudio.R.Package.Interop;

namespace Microsoft.VisualStudio.R.Package.Feedback {
    internal class SendMailCommand : PackageCommand {
        private readonly ILoggingPermissions _permissions;
        private readonly IProcessServices _pss;
        private readonly IActionLog _log;

        public SendMailCommand(Guid group, int id, ILoggingPermissions permissions, IProcessServices pss, IActionLog log) :
            base(group, id) {
            _permissions = permissions;
            _pss = pss;
            _log = log;
        }

        protected override void SetStatus() {
            Enabled = Visible = _permissions.IsFeedbackPermitted;
        }

        protected void SendMail(string body, string subject, string attachmentFile) {
            if (attachmentFile != null) {
                IntPtr pidl = IntPtr.Zero;
                try {
                    pidl = NativeMethods.ILCreateFromPath(attachmentFile);
                    if (pidl != IntPtr.Zero) {
                        NativeMethods.SHOpenFolderAndSelectItems(pidl, 0, IntPtr.Zero, 0);
                    }
                } finally {
                    if (pidl != IntPtr.Zero) {
                        NativeMethods.ILFree(pidl);
                    }
                }
            }

            Application outlookApp = null;
            try {
                outlookApp = new Application();
            } catch (System.Exception ex) {
                _log.WriteAsync(LogVerbosity.Normal, MessageCategory.Error, "Unable to start Outlook: " + ex.Message).DoNotWait();
            }

            if (outlookApp == null) {
                var fallbackWindow = new SendMailFallbackWindow {
                    MessageBody = body
                };
                fallbackWindow.Show();
                fallbackWindow.Activate();

                ProcessStartInfo psi = new ProcessStartInfo();
                psi.UseShellExecute = true;
                psi.FileName = string.Format(
                    CultureInfo.InvariantCulture,
                    "mailto:rtvsuserfeedback@microsoft.com?subject={0}&body={1}",
                    Uri.EscapeDataString(subject),
                    Uri.EscapeDataString(body));
                _pss.Start(psi);
            } else {
                try {
                    MailItem mail = outlookApp.CreateItem(OlItemType.olMailItem) as MailItem;
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.To = "rtvsuserfeedback@microsoft.com";
                    mail.Display(Modal: false);
                } catch (System.Exception ex) {
                    _log.WriteAsync(LogVerbosity.Normal, MessageCategory.Error, "Error composing Outlook e-mail: " + ex.Message).DoNotWait();
                }
            }
        }
    }
}
