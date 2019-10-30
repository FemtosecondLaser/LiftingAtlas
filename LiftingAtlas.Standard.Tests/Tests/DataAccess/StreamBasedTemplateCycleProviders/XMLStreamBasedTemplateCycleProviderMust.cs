using NUnit.Framework;
using System.IO;
using System.Text;

namespace LiftingAtlas.Standard.Tests
{
    [TestFixture]
    public class XMLStreamBasedTemplateCycleProviderMust
    {
        XMLStreamBasedTemplateCycleProvider streamBasedTemplateCycleProvider;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            streamBasedTemplateCycleProvider = new XMLStreamBasedTemplateCycleProvider();
        }

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
