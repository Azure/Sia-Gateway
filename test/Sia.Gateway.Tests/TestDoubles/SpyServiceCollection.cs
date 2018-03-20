using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Sia.Gateway.Tests.TestDoubles
{
    public class SpyServiceCollection : IServiceCollection
    {
        public List<ServiceDescriptor> BackingList { get; set; }
            = new List<ServiceDescriptor>();
        public ServiceDescriptor this[int index] { get => BackingList[index]; set => BackingList[index] = value; }

        public int Count => BackingList.Count;

        public bool IsReadOnly => false;

        public void Add(ServiceDescriptor item) => BackingList.Add(item);
        public void Clear() => BackingList = new List<ServiceDescriptor>();
        public bool Contains(ServiceDescriptor item) => BackingList.Contains(item);
        public void CopyTo(ServiceDescriptor[] array, int arrayIndex) => BackingList.CopyTo(array, arrayIndex);
        public IEnumerator<ServiceDescriptor> GetEnumerator() => BackingList.GetEnumerator();
        public int IndexOf(ServiceDescriptor item) => BackingList.IndexOf(item);
        public void Insert(int index, ServiceDescriptor item) => BackingList.Insert(index, item);
        public bool Remove(ServiceDescriptor item) => BackingList.Remove(item);
        public void RemoveAt(int index) => BackingList.RemoveAt(index);
        IEnumerator IEnumerable.GetEnumerator() => BackingList.GetEnumerator();
    }
}
