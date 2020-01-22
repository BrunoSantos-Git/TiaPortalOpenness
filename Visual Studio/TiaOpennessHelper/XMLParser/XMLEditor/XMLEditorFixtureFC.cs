using System.Collections.Generic;
using System.Xml;
using TiaOpennessHelper.XMLParser;

namespace TiaOpennessHelper
{
    public partial class OpennessHelper
    {
        /// <summary>
        /// Insert New Access
        /// </summary>
        /// <param name="originalXml"></param>
        /// <param name="newNode"></param>
        /// <param name="workPath"></param>
        public static void InsertNewAccessNode(XmlDocument originalXml, XmlDocument newNode, string workPath)
        {
            XmlNodeList Node = originalXml.SelectNodes(workPath);
            XmlNode nodeToImport = newNode.FirstChild;
            XmlNode nodeAux = Node[0].FirstChild.FirstChild;
            XmlNode nodeImported = originalXml.ImportNode(nodeToImport, true);

            foreach (XmlNode nodePointer in nodeAux)
            {
                if (nodePointer.Name != "Access")
                {
                    nodeAux.InsertBefore(nodeImported, nodePointer);
                    break;
                }
            }
        }

        /// <summary>
        /// Insert New Wire
        /// </summary>
        /// <param name="originalXml"></param>
        /// <param name="newNode"></param>
        /// <param name="workPath"></param>
        public static void InsertNewWireNode(XmlDocument originalXml, XmlDocument newNode, string workPath)
        {
            XmlNodeList Node = originalXml.SelectNodes(workPath);
            XmlNode nodeToImport = newNode.FirstChild;
            XmlNode nodePointer = Node[0].FirstChild.LastChild;
            XmlNode nodeImported = originalXml.ImportNode(nodeToImport, true);
            nodePointer.InsertAfter(nodeImported, nodePointer.LastChild);
        }
    }
}
