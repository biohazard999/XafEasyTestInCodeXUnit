using System;
using System.Xml;
using DevExpress.EasyTest.Framework;

namespace EasyTest.Tests.Utils
{
    public abstract class EasyTestFixtureBase : IDisposable
    {
        public abstract TestCommandAdapter CommandAdapter { get; }
        public abstract ICommandAdapter Adapter { get; }
        public abstract bool IsWeb { get; }
        public abstract void Dispose();

        protected static XmlAttribute CreateAttribute(XmlDocument doc, string attributeName, string attributeValue)
        {
            var entry = doc.CreateAttribute(attributeName);
            entry.Value = attributeValue;
            return entry;
        }

        protected static XmlAttribute CreateAttribute(XmlDocument doc, string attributeName, bool attributeValue)
            => CreateAttribute(doc, attributeName, attributeValue.ToString());
    }
}