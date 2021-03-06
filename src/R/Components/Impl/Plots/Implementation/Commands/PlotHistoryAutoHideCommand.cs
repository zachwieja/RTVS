﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Threading.Tasks;
using Microsoft.R.Components.Controller;
using Microsoft.R.Components.InteractiveWorkflow;

namespace Microsoft.R.Components.Plots.Implementation.Commands {
    internal sealed class PlotHistoryAutoHideCommand : PlotHistoryCommand, IAsyncCommand {
        public PlotHistoryAutoHideCommand(IRInteractiveWorkflow interactiveWorkflow, IRPlotHistoryVisualComponent visualComponent) :
            base(interactiveWorkflow, visualComponent) {
        }

        public CommandStatus Status {
            get {
                if (VisualComponent.AutoHide) {
                    return CommandStatus.SupportedAndEnabled | CommandStatus.Latched;
                }

                return CommandStatus.SupportedAndEnabled;
            }
        }

        public Task<CommandResult> InvokeAsync() {
            VisualComponent.AutoHide = !VisualComponent.AutoHide;
            return Task.FromResult(CommandResult.Executed);
        }
    }
}
