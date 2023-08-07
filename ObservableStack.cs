using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delegates.Observers
{
    public delegate void StackHandler(object obj);
    public class StackOperationsLogger
	{
        public StringBuilder Log = new StringBuilder();

        public void HandleEvent(object eventData)
        {
            Log.Append(eventData);
        }

		public void SubscribeOn<T>(ObservableStack<T> stack)
		{
			stack.Add(HandleEvent);
		}

		public string GetLog()
		{
			return Log.ToString();
		}
	}

	public class ObservableStack<T>
	{
        public event StackHandler NotifyEvent;

        private List<T> data = new List<T>();
        public void Add(Action<object> action)
		{
			NotifyEvent += action.Invoke;
		}

		public void Notify(object eventData)
		{
			NotifyEvent?.Invoke(eventData);
		}

		public void Remove(Action<object> action)
		{
            NotifyEvent -= action.Invoke;
        }

		public void Push(T obj)
		{
			data.Add(obj);
			Notify(new StackEventData<T> { IsPushed = true, Value = obj });
		}

		public T Pop()
		{
			if (data.Count == 0)
				throw new InvalidOperationException();
			var result = data.Last();
			Notify(new StackEventData<T> { IsPushed = false, Value = result });
			return result;

		}
	}
}
