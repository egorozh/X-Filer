using System;
using System.Collections;
using System.Collections.Generic;

namespace XFiler.SDK
{
    internal class DirectoryHistory : IDirectoryHistory
    {
        #region Properties

        public bool CanMoveBack => Current.PreviousNode != null;
        public bool CanMoveForward => Current.NextNode != null;

        public DirectoryNode Current { get; private set; }

        #endregion

        #region Events

        public event EventHandler? HistoryChanged;

        #endregion

        #region Constructor

        public DirectoryHistory(XFilerUrl url)
        {   
            var head = new DirectoryNode(url);
            Current = head;
        }

        #endregion

        #region Public Methods

        public void MoveBack()
        {
            var prev = Current.PreviousNode;

            Current = prev;

            RaiseHistoryChanged();
        }

        public void MoveForward()
        {
            var next = Current.NextNode;

            Current = next;

            RaiseHistoryChanged();
        }

        public void Add(XFilerUrl url)
        {
            var node = new DirectoryNode(url);
                
            Current.NextNode = node;
            node.PreviousNode = Current;

            Current = node;

            RaiseHistoryChanged();
        }

        #endregion
        
        #region Private Methods

        private void RaiseHistoryChanged() => HistoryChanged?.Invoke(this, EventArgs.Empty);

        #endregion

        #region Enumerator

        public IEnumerator<DirectoryNode> GetEnumerator()
        {
            yield return Current;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}