using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace Library
{
    public class Awaiter : INotifyCompletion
    {
        readonly LongOperation awaitable;
        // int меняется на любой результат операции
        int result;
        public Awaiter(LongOperation awaitable)
        {
            this.awaitable = awaitable;
            Log("Awaiter created");
            if (IsCompleted)
            {
                SetResult();
            }
        }
        public bool IsCompleted => awaitable.IsFinished;

        public int GetResult()
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
            Log("Operation finished");
            SetResult();
            if (capturedContext != null)
                capturedContext.Post(_ => {continuation(), null);
            else
                continuation();
        }
        public void OnCompleted(Action continuation)
        {
            if (IsCompleted)
            {
                ContinueOn(continuation, SynchronizationContext.Current);
                return;
            }
            awaitable.Finished += () =>
            {
                SetResult();
                ContinueOn(continuation, SynchronizationContext.Current);
            };
        }



        private void SetResult()
        {
            result = new Random().Next();
        }

        void Log(string text) => awaitable?.Log(text);
    }
}
