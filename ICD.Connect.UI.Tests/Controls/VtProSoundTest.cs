using System;
using System.Linq;
using ICD.Connect.Panels.Mock;
using ICD.Connect.UI.Controls;
using NUnit.Framework;

namespace ICD.Connect.UI.Tests.Controls
{
    [TestFixture]
    class VtProSoundTest
    {

        /// <summary>
        /// Release resources.
        /// </summary>
        [Test]
        public void DisposeTest()
        {
            MockPanelDevice mockPanelDevice = new MockPanelDevice();
            var vtProSound = new VtProSound(mockPanelDevice);
            vtProSound.Dispose();

            Assert.Pass();
        }

        /// <summary>
        /// Starts the sound.
        /// </summary>
        [Test]
        public void PlayTest()
        {
            MockPanelDevice mockPanelDevice = new MockPanelDevice();
            var vtProSound = new VtProSound(mockPanelDevice);
            vtProSound.Play();

            Assert.AreEqual(1, mockPanelDevice.BooleanInput.Count());
        }

        /// <summary>
        /// Starts the sound.
        /// </summary>
        /// <param name="loopInterval">How often to loop in milliseconds, no loop if 0</param>
        [TestCase(500)]
        public void Play(long loopInterval)
        {
            MockPanelDevice mockPanelDevice = new MockPanelDevice();
            var vtProSound = new VtProSound(mockPanelDevice);
            vtProSound.Play(loopInterval);

            Assert.AreEqual(1, mockPanelDevice.BooleanInput.Count());
        }

        /// <summary>
        /// Stops the sound.
        /// </summary>
        [Test]
        public void Stop()
        {
            MockPanelDevice mockPanelDevice = new MockPanelDevice();
            var vtProSound = new VtProSound(mockPanelDevice);
            vtProSound.Stop();

            Assert.AreEqual(1, mockPanelDevice.BooleanInput.Count());
        }
    }
}