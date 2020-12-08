using System;
using System.Threading.Tasks;

namespace Library
{
    public class LongOperation
    {
        private volatile bool finished;
        public LongOperation()
        {
            this.finished = false;
        }

        public void Finish()
        {
            if (!finished)
            {
                finished = true;
                Finished?.Invoke();
            }
        }

        public event Action Finished;
       
        public Awaiter GetAwaiter() => new Awaiter(this);
        public bool IsFinished => finished;
        public Action<string> Log;
    }
}
