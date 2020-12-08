using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Library
{
    public class LongOperation<T>
    {
        private volatile bool finished;
        private Func<T> operation;

        public LongOperation(Func<T> operation = null)
        {
            this.finished = false;
            this.operation = operation == null ? (() => default(T)) : operation;
        }

        /// <summary>
        /// Запуск длительной операции в асинхронном режиме (придумать, как доделать)
        /// </summary>
        /// <returns></returns>
        public LongOperationResult<T> StartAsync()
        {
            Task.Run(() =>
            {
                Finished += LongOperation_Finished;
                Result = operation();
                Finished.Invoke(this, EventArgs.Empty);
            });
            return new LongOperationResult<T>(this);
        }

        private void LongOperation_Finished(object sender, EventArgs eventArgs)
        {
            finished = true;
            Finished -= LongOperation_Finished;
        }

        public event EventHandler Finished;
        
        public bool IsFinished => finished;

        public virtual T Result { get; protected set; }

        public Action<string> Log;
    }
}
