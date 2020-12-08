﻿using System;
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
        public void OnCompleted(Action continuation)
        {
            if (IsCompleted)
            {
                continuation();
                return;
            }
            var capturedContext = SynchronizationContext.Current;
            awaitable.Finished += () =>
            {
                Log("Operation finished");
                SetResult();
                if (capturedContext != null)
                    capturedContext.Post(_ => continuation(), null);
                else
                    continuation();
            };
        }



        private void SetResult()
        {
            result = new Random().Next();
        }

        void Log(string text) => awaitable?.Log(text);
    }
}
