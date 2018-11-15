using System.Collections.Generic;
using System.Linq;
using ICD.Connect.UI.Utils;
using NUnit.Framework;

namespace ICD.Connect.UI.Tests.Utils
{
	[TestFixture]
	public sealed class KeypadStringBuilderTest
	{
		[TestCase("test")]
		public void StringChangedFeedbackTest(string data)
		{
			List<string> feedback = new List<string>();

			KeypadStringBuilder builder = new KeypadStringBuilder();
			builder.OnStringChanged += (sender, args) => feedback.Add(args.Data);

			builder.SetString(data);

			Assert.IsTrue(feedback.SequenceEqual(new[] {data}));
		}

		[TestCase("test")]
		public void SetStringTest(string data)
		{
			KeypadStringBuilder builder = new KeypadStringBuilder();
			builder.SetString(data);

			Assert.AreEqual(data, builder.ToString());
		}

		[Test]
		public void AppendCharacterTest()
		{
			Assert.Inconclusive();
		}

		[Test]
		public void BackspaceTest()
		{
			Assert.Inconclusive();
		}

		[Test]
		public void ClearTest()
		{
			Assert.Inconclusive();
		}

		[Test]
		public void PopTest()
		{
			Assert.Inconclusive();
		}

		[Test]
		public void ToStringTest()
		{
			Assert.Inconclusive();
		}
	}
}
