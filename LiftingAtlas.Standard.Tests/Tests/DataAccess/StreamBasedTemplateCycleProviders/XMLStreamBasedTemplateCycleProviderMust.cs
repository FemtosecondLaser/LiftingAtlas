using NUnit.Framework;
using System.IO;
using System.Text;

namespace LiftingAtlas.Standard.Tests
{
    [TestFixture]
    public class XMLStreamBasedTemplateCycleProviderMust
    {
        /// <summary>
        /// XML stream-based template cycle provider.
        /// </summary>
        XMLStreamBasedTemplateCycleProvider streamBasedTemplateCycleProvider;

        /// <summary>
        /// One time set up.
        /// </summary>
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            streamBasedTemplateCycleProvider = new XMLStreamBasedTemplateCycleProvider();
        }

        /// <summary>
        /// Tests ability of <see cref="XMLStreamBasedTemplateCycleProvider"/>
        /// to provide cycle template name and lift of
        /// cycle as supplied via XML.
        /// </summary>
        [Test]
        public void ProvideTemplateCycleNameAndLiftAsSuppliedViaXML()
        {
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(REFERENCECYCLESB.XML())))
                Assert.That(
                    streamBasedTemplateCycleProvider.CycleTemplateNameAndLift(memoryStream),
                    Is.EqualTo(REFERENCECYCLESB.CycleTemplateNameAndLift()),
                    "Template cycle name and lift, provided by the system under test, " +
                    "must match reference template cycle name and lift."
                    );
        }

        /// <summary>
        /// Tests ability of <see cref="XMLStreamBasedTemplateCycleProvider"/>
        /// to provide <see cref="TemplateCycle{T1, T2}"/> version of
        /// cycle as supplied via XML.
        /// </summary>
        [Test]
        public void ProvideTemplateCycleAsSuppliedViaXML()
        {
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(REFERENCECYCLESB.XML())))
                Assert.That(
                    streamBasedTemplateCycleProvider.TemplateCycle(memoryStream),
                    Is.EqualTo(REFERENCECYCLESB.TemplateCycle()),
                    "Template cycle, provided by the system under test, must match reference template cycle."
                    );
        }
    }
}
