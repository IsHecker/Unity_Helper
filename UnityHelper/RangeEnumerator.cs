using System;

namespace UnityHelper
{
    public ref struct RangeEnumerator
    {
        private int _current;
        private readonly int _end;

        public RangeEnumerator(Range range)
        {
            _current = range.Start.Value - 1;
            _end = range.End.Value;
        }

        public readonly int Current => _current;

        public bool MoveNext() => ++_current <= _end;

        public void Reset() => throw new NotImplementedException();
    }
}
