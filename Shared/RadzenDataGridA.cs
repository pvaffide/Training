using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Training.Shared
{
    public class RadzenDataGridA<TItem> : RadzenDataGrid<TItem>
    {
        public override async Task SetParametersAsync(ParameterView parameterView)
        {
            ApplyFilterText = "Appliquer";
            ClearFilterText = "Retirer";
            ContainsText = "Contient";
            EmptyText = "";
            EndsWithText = "Termine par";
            EqualsText = "Égale";
            FilterText = "Filtre";
            GreaterThanText = "Plus grand";
            GreaterThanOrEqualsText = "Plus grand ou égal";
            LessThanText = "Plus petit";
            LessThanOrEqualsText = "Plus petit ou égal";
            NotEqualsText = "Différent";
            OrOperatorText = "Ou";
            StartsWithText = "Commence par";
            GroupPanelText = "Filtre groupe";
            DoesNotContainText = "Ne contient pas";
            AndOperatorText = "Et";
            FilterCaseSensitivity = Radzen.FilterCaseSensitivity.CaseInsensitive;
            await base.SetParametersAsync(parameterView);
        }
    }
}
