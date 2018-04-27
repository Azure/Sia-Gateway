using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.State.Processing.StateModels
{
    /// <summary>
    /// Interface for allowing deep copy of objects.
    /// Implementing this in a way that actually creates
    /// a shallow copy or a partial shallow copy can lead to major bugs!
    /// </summary>
    /// <typeparam name="T">Should be the type of the class implementing this interface, in most cases</typeparam>
    public interface IDeepCopyable<T>
    {
        /// <summary>
        /// Creates a copy, allocating new objects with equivalent values but with no shared references.
        /// Changes to the copy should not change the original
        /// The deep copy should be disposable without interfering with the original object.
        /// </summary>
        /// <returns>A copy of this object that shares no memory references with the original</returns>
        T GetDeepCopy();
    }
}
