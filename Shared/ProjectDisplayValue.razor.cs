using Training.Helpers;
using Microsoft.AspNetCore.Components;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Training.Shared
{
    public partial class ProjectDisplayValue
    {
        [Parameter]
        public Expression<Func<object>> For { get; set; }

        public Func<object> _for;
        [Parameter]
        public bool Raw { get; set; }

        private string FormattedValue => Display.For(For, _for());
        public override async Task SetParametersAsync(ParameterView parameterView)
        {
            bool forChanged = parameterView.DidParameterChange(nameof(For), For);
            await base.SetParametersAsync(parameterView);
            if (forChanged && For != null)
                _for = For.Compile();
        }

        [Parameter(CaptureUnmatchedValues = true)]
        public IDictionary<string, object> AdditionalAttributes { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
    }
}