using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    using Awaiter = Awaiter<int>;
    using LongOperation = LongOperation<int>;
    using LongOperationResult = LongOperationResult<int>;
    public class LongOperationResult<T>
    {
        private LongOperation<T> operation;

        internal LongOperationResult(LongOperation<T> operation)
        {
            this.operation = operation;
        }
        public Awaiter<T> GetAwaiter() => new Awaiter<T>(operation);
    }
}
