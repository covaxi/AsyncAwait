using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Library
{
    public class LongOperationResult
    {
        private LongOperation operation;

        public LongOperationResult(LongOperation operation)
        {
            this.operation = operation;
        }
        public Awaiter GetAwaiter() => new Awaiter(operation);
    }
    public class LongOperation
    {
        private volatile bool finished;
        public LongOperation()
        {
            this.finished = false;
        }

        /// <summary>
        /// Запуск длительной операции в асинхронном режиме (придумать, как доделать)
        /// </summary>
        /// <returns></returns>
        public LongOperationResult StartAsync()
        {
            Task.Run(() =>
            {
                Finished += LongOperation_Finished;
                Thread.Sleep(2000);
                Finished.Invoke();
            });
            return new LongOperationResult(this);
        }

        private void LongOperation_Finished()
        {
            finished = true;
            Finished -= LongOperation_Finished;
        }

        public event Action Finished;
       
        
        public bool IsFinished => finished;
        public Action<string> Log;
    }
}
