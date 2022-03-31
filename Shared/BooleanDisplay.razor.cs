using Microsoft.AspNetCore.Components;

namespace Training.Shared
{
    public partial class BooleanDisplay
    {
        [Parameter]
        public bool Value { get; set; }
    }
}