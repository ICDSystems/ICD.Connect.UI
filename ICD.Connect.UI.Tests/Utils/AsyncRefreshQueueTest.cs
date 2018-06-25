using System;
using ICD.Common.Utils;
using ICD.Connect.UI.Utils;
using NUnit.Framework;

namespace ICD.Connect.UI.Tests.Utils
{
	[TestFixture]
	public sealed class AsyncRefreshQueueTest
	{
		[Test]
		public void RefreshCallbackTest()
		{
			Action callback = () => { };
			AsyncRefreshQueue queue = new AsyncRefreshQueue {RefreshCallback = callback};

			Assert.AreEqual(callback, queue.RefreshCallback);
		}

		[Test]
		public void EnqueueRefreshTest()
		{
			int refreshCount = 0;

			Action callback = () =>
			                  {
				                  refreshCount++;
				                  ThreadingUtils.Sleep(1000);
			                  };

			AsyncRefreshQueue queue = new AsyncRefreshQueue {RefreshCallback = callback};

			// Enqueue 10 refreshes.
			for (int index = 0; index < 10; index++)
				queue.EnqueueRefresh();

			ThreadingUtils.Sleep(5000);

			// Should have only refreshed twice.
			Assert.AreEqual(2, refreshCount);
		}
	}
}
