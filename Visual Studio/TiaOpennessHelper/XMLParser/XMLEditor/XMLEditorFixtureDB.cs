using System.Xml;

namespace TiaOpennessHelper
{
    public partial class OpennessHelper
    {
        /// <summary>
        /// Insert of the new networks inside the DB_ACTIONS
        /// </summary>
        /// <param name="originalXml"></param>
        /// <param name="newNetwork"></param>
        public static void InsertNewAction(XmlDocument originalXml, XmlDocument newNetwork)
        {
            XmlNodeList Network = originalXml.SelectNodes("/Document/SW.Blocks.GlobalDB/AttributeList/Interface");
            XmlNode networkToImport = newNetwork.FirstChild;
            XmlNode nodePointer = Network[0].FirstChild.FirstChild;
            XmlNode networkImported = originalXml.ImportNode(networkToImport, true);
            nodePointer.InsertAfter(networkImported, nodePointer.LastChild);
        }

        /// <summary>
        /// Renumbering Offset
        /// </summary>
        /// <param name="originalXml"></param>
        /// <returns></returns>
        public static void OffsetRenumbering(XmlDocument originalXml)
        {
            int i = 0;
            XmlNodeList Network = originalXml.SelectNodes("/Document/SW.Blocks.GlobalDB/AttributeList/Interface");
            XmlNode nodeAux = Network[0].FirstChild.FirstChild;

            foreach (XmlNode nodePointer in nodeAux)
            {
                nodePointer.FirstChild.FirstChild.InnerText = i.ToString();
                i++;
            }
        }
    }
}
