using Training.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Training.Shared
{
    public partial class ProjectLabelVisu
    {
        private Expression<Func<object>> _for;

        [Parameter]
        public Expression<Func<object>> For
        {
            get => _for;
            set
            {
                _for = value;
                if (Required == null)
                    Required = AttributeHelper.IsRequired(For);
            }
        }

        [Parameter]
        public bool? Required { get; set; } = null;

        [Parameter(CaptureUnmatchedValues = true)]
        public IDictionary<string, object> AdditionalAttributes { get; set; }


        private string labelVisu => GetDisplayName();
        private string GetDisplayName()
        {
            return AttributeHelper.GetDisplayName(For);
        }
    }
}