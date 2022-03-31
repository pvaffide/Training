using Training.Helpers;
using Training.Models;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using System.Linq;
using System.Threading.Tasks;

namespace Training.Shared
{

    public partial class RadzenDataGridColumnA<TItem> : RadzenDataGridColumn<TItem>
    {
        public override async Task SetParametersAsync(ParameterView parameterView)
        {
            await base.SetParametersAsync(parameterView);
            if (Property != null)
            {
                if (Title == null)
                    Title = AttributeHelper.GetDisplayName<TItem>(Property);
                if (FormatString == null)
                    FormatString = AttributeHelper.GetDataFormatString<TItem>(Property);

                var type = typeof(TItem);
                foreach (var prop in Property.Split('.'))
                    type = type.GetProperty(prop).PropertyType;
                if (type.IsAssignableTo(typeof(INameable)))
                    Property = Property + "." + nameof(INameable.Nom);
                OnInitialized();
            }
        }
    }
}
