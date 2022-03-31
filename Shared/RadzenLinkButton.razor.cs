using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Collections.Generic;

namespace Training.Shared
{
    public partial class RadzenLinkButton
    {
        [Parameter]
        public string Href { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string Class { get; set; }

        [Parameter]
        public bool OpenInNewTab { get; set; }
        [Parameter]
        public bool ForceNavigation { get; set; }

        private EventCallback _click;
        [Parameter]
        public EventCallback Click
        {
            get => _click;
            set
            {
                ForceNavigation = true;
                _click = value;
            }
        }

        [Parameter]
        public bool Download { get; set; }
        [Parameter]
        public bool Disabled { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public IDictionary<string, object> AdditionalAttributes { get; set; }

        private void ButtonClick(MouseEventArgs _)
        {
            if (Disabled)
                return;
            if (Href != null)
                navigationManager.NavigateTo(Href, true);
            else
                Click.InvokeAsync();
        }
    }
}