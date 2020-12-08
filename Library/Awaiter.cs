using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace Library
{
    using Awaiter = Library.Awaiter<int>;
    public class Awaiter<T> : INotifyCompletion
    {
        readonly LongOperation<T> awaitable;
        T result;
        public Awaiter(LongOperation<T> awaitable)
        {
            this.awaitable = awaitable;
            Log("Awaiter created");
            if (IsCompleted)
            {
                SetResult(awaitable.Result);
            }
        }
        public bool IsCompleted => awaitable.IsFinished;

        public T GetResult()
        {
            if (!IsCompleted)
            {
                var wait = new SpinWait();
                while (!awaitable.IsFinished)
                    wait.SpinOnce();
            }
            Log("Returning result");
            return result;
        }
        private void ContinueOn(Action continuation, SynchronizationContext capturedContext)
        {

            SetResult(awaitable.Result);
            if (capturedContext != null)
            {
                capturedContext.Post(_ =>
                {
                    Log("Operation finished");
                    continuation();
                }, null);
            }
            else
            {
                Log("Operation finished");
                continuation();
            }
        }
        public void OnCompleted(Action continuation)
        {
            if (IsCompleted)
            {
                ContinueOn(continuation, SynchronizationContext.Current);
                return;
            }
            awaitable.Finished += (s ,e) =>
            {
                SetResult(awaitable.Result);
                ContinueOn(continuation, SynchronizationContext.Current);
            };
        }

        private void SetResult(T t)
        {
            result = t;
        }

        void Log(string text)
        {
            awaitable?.Log(text);
        }
    }
}
