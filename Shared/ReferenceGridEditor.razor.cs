using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using Training.Models;
using Training.Services;
using System.Threading.Tasks;

namespace Training.Shared
{
    public partial class ReferenceGridEditor<TItem> where TItem : class, new()
    {
        [Parameter]
        public IList<TItem> Items { get; set; }

        [Parameter]
        public RenderFragment Columns { get; set; }

        [Parameter]
        public TrainingDbContext DbContext { get; set; } = null!;

        [Parameter]
        public Func<TItem, bool> IsEditable { get; set; } = _ => true;

        [Parameter]
        public string Title { get; set; }


        RadzenDataGridA<TItem> Grid;

        private readonly bool _isSoftDeletable;
        public ReferenceGridEditor()
        {
            _isSoftDeletable = typeof(SoftDeletable).IsAssignableFrom(typeof(TItem));
        }

        [Parameter]
        public EventCallback<TItem> ItemUpdated { get; set; }

        TItem editedItem;
        async Task SaveRow(TItem item)
        {
            await ItemUpdated.InvokeAsync(item);
            if (!DbContext.ValidateContextWithNotification(notificationService) || !DbContext.SaveChangesWithNotification(notificationService))
                return;
            await Grid.UpdateRow(item);
            editedItem = null;
        }

        [Parameter]
        public EventCallback<TItem> ItemEdited { get; set; }
        void EditRow(TItem item)
        {
            if (editedItem != null)
                CancelEdit(editedItem);

            Grid.EditRow(item);
            ItemEdited.InvokeAsync(item);
            editedItem = item;
        }

        void CancelEdit(TItem item)
        {
            Grid.CancelEditRow(item);
            var entry = DbContext.Entry(item);
            if (entry.State == EntityState.Modified)
            {
                entry.CurrentValues.SetValues(entry.OriginalValues);
                entry.State = EntityState.Unchanged;
            }
            else if (entry.State == EntityState.Added)
            {
                Items.Remove(item);
                DbContext.Remove(item);
            }

            editedItem = null;
        }

        [Parameter]
        public EventCallback<TItem> ItemCreated { get; set; }

        async Task InsertRow()
        {
            if (editedItem != null)
                CancelEdit(editedItem);

            var item = new TItem();
            editedItem = item;
            await Grid.InsertRow(item);
            Items.Add(item);
            DbContext.Add(item);
            await ItemCreated.InvokeAsync(item);
            await ItemEdited.InvokeAsync(item);
        }
    }
}