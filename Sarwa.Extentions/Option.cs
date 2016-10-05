using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sarwa.Common
{
    public class Option<T> : IEnumerable<T>
    {
        private T[] data;

        private Option(T[] data)
        {
            this.data = data;
        }

        public static Option<T> Create(T element) 
            => new Option<T>(new T[] { element });
        
        public static Option<T> CreateEmpty()
            => new Option<T>(new T[0]);
        
        public IEnumerator<T> GetEnumerator()
            => ((IEnumerable<T>)this.data).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();
    }
}
