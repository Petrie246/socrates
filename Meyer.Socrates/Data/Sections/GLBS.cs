﻿namespace Meyer.Socrates.Data.Sections
{
    using Meyer.Socrates.IO;
    using System;
    using System.Collections.Generic;

    [SectionKey("GLBS")]
    public sealed class GLBS: IndexableSection<int>
    {
        private void ValidateItem(int item)
        {
            if (item < UInt16.MinValue) throw new ArgumentOutOfRangeException($"item value less than {UInt16.MinValue}", "item");
            if (item > UInt16.MaxValue) throw new ArgumentOutOfRangeException($"item value greater than {UInt16.MaxValue}", "item");
        }

        protected override void SetItem(int index, int item)
        {
            ValidateItem(item);
            base.SetItem(index, item);
        }

        protected override void InsertItem(int index, int item)
        {
            ValidateItem(item);
            base.InsertItem(index, item);
        }

        protected override void Read(IDataReadContext c, IList<int> items)
        {
            MagicNumber = c.AssertAny(Ms3dmm.MAGIC_NUM_US, Ms3dmm.MAGIC_NUM_JP);
            c.Assert<UInt32>(2);
            var count = c.Read<UInt32>();
            for (int i = 0;i < count;i++)
                items.Add(c.Read<Int16>());
        }

        protected override void Write(IDataWriteContext c, IList<int> items)
        {
            c.Write(MagicNumber);
            c.Write<UInt32>(2);
            c.Write((UInt32)items.Count);
            for (int i = 0;i < items.Count;i++)
                c.Write((UInt16)items[i]);
        }

        /// <summary>
        /// Gets the body set for the body part specified.
        /// </summary>
        /// <param name="bodyPart">The index to the body part.</param>
        /// <returns>The body set.</returns>
        public int GetBodySet(int bodyPart)
        {
            if (bodyPart < 0 || bodyPart >= Count)
                throw new ArgumentOutOfRangeException("bodyPart");

            return this[bodyPart];
        }
    }
}
