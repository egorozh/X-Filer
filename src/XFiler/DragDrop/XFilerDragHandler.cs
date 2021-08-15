using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections;
using System.Linq;
using System.Windows;

namespace XFiler.DragDrop
{
    internal class XFilerDragHandler : IDragSource
    {   
        /// <inheritdoc />
        public virtual void StartDrag(IDragInfo dragInfo)
        {
            var items = dragInfo.SourceItems.Cast<object>().ToList();
          
            if (items.Count > 1)
            {
                dragInfo.Data = items;
            }
            else
            {
                var singleItem = items.FirstOrDefault();

                dragInfo.Data = singleItem is IEnumerable and not string ? items : singleItem;
            }

            dragInfo.Effects = dragInfo.Data != null
                ? DragDropEffects.Copy | DragDropEffects.Move | DragDropEffects.Link
                : DragDropEffects.None;
        }

        /// <inheritdoc />
        public virtual bool CanStartDrag(IDragInfo dragInfo)
        {
            return true;
        }

        /// <inheritdoc />
        public virtual void Dropped(IDropInfo dropInfo)
        {
        }

        /// <inheritdoc />
        public virtual void DragDropOperationFinished(DragDropEffects operationResult, IDragInfo dragInfo)
        {
            // nothing here
        }

        /// <inheritdoc />
        public virtual void DragCancelled()
        {
        }

        /// <inheritdoc />
        public virtual bool TryCatchOccurredException(Exception exception)
        {
            return false;
        }
    }
}