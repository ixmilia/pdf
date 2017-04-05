// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections;
using System.Collections.Generic;

namespace IxMilia.Pdf
{
    public class PdfPathBuilder : IEnumerable<IPdfPathItem>
    {
        public IList<IPdfPathItem> Items { get; } = new List<IPdfPathItem>();

        public void Add(IPdfPathItem item)
        {
            Items.Add(item);
        }

        public IEnumerator<IPdfPathItem> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        public PdfPath ToPath()
        {
            var path = new PdfPath();
            foreach (var item in Items)
            {
                foreach (var command in item.GetCommands())
                {
                    path.Commands.Add(command);
                }
            }

            return path;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Items).GetEnumerator();
        }
    }
}
