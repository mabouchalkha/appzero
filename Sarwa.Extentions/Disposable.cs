using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sarwa.Common
{
    public static class Disposable
    {
        public static TResult Using<TDisposable, TResult>(Func<TDisposable> factory, Func<TDisposable, TResult> map)
          where TDisposable : IDisposable
        {
            using (var disposable = factory())
            {
                return map(disposable);
            }
        }
    }
}
