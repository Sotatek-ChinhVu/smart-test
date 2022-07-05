using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    [Serializable()]
    public abstract class EmrCloneable<T>
    {
        /// <summary>
        /// Shallow copy <see cref="T"/> object.
        /// </summary>
        /// <returns>Shallow copy of <see cref="T"/></returns>
        public virtual T Clone()
        {
            return (T)this.MemberwiseClone();
        }

        /// <summary>
        /// Deep copy <see cref="T"/> object.
        /// Override this method to set another object properties
        /// </summary>
        /// <returns>Deep copy of <see cref="T"/></returns>
        public virtual T DeepClone()
        {
            T other = this.Clone();
            // Set some other properties then return
            return other;
        }
    }
}
