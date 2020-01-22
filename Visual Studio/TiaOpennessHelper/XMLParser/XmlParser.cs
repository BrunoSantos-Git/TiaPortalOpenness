using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Xml;
using System.Xml.Linq;
using TiaOpennessHelper.Enums;
using TiaOpennessHelper.Models;
using TiaOpennessHelper.Models.Block;
using TiaOpennessHelper.Models.ControllerDataType;
using TiaOpennessHelper.Models.ControllerTagTable;
using TiaOpennessHelper.Models.DataBlock;
using TiaOpennessHelper.Models.Members;
using TiaOpennessHelper.SafetyMaker;

namespace TiaOpennessHelper.XMLParser
{
    /// <summary>
    /// 
    /// </summary>
    /// TODO Edit XML Comment Template for XmlParser
    public class XmlParser
    {
        #region Fields

        /// <summary>
        /// The _ns (namespace)
        /// </summary>
        private readonly XmlNamespaceManager _ns;

        #endregion

        #region Properties

        /// <summary>Gets or sets the name of the file.</summary>
        /// <value>The name of the file.</value>
        /// TODO Edit XML Comment Template for FileName
        public string FileName { get; set; }

        /// <summary>Gets or sets the document.</summary>
        /// <value>The document.</value>
        /// TODO Edit XML Comment Template for Document
        public XmlDocument Document { get; set; }
        /// <summary>Gets or sets the root node.</summary>
        /// <value>The root node.</value>
        /// TODO Edit XML Comment Template for RootNode
        public XmlNode RootNode { get; set; }
        /// <summary>Gets or sets the node.</summary>
        /// <value>The node.</value>
        /// TODO Edit XML Comment Template for Node
        public XmlNode Node { get; set; }

        #endregion

        #region C´tor

        /// <summary>Create Xml Parser</summary>
        /// <param name="fileName">Name of the file.</param>
        public XmlParser(string fileName)
        {
            //create new Xml Document
            Document = new XmlDocument();

            _ns = new XmlNamespaceManager(Document.NameTable);
            _ns.AddNamespace("SI", "http://www.siemens.com/automation/Openness/SW/Interface/v2");
            _ns.AddNamespace("siemensNetworks", "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v1");

            //Load Xml File with fileName into memory
            Document.Load(fileName);
            //get root node of xml file
            RootNode = Document.DocumentElement;
        }

        #endregion

        #region public methods
        /// <summary>Parse XML File into Type 'XmlInformation'</summary>
        /// <returns></returns>
        public XmlInformation Parse()
        {
            XmlInformation xmlInfo = null;
            //Get the node that starts with 'SW.'
            var xmlTypeNode = RootNode.SelectSingleNode(@"//*[contains(name(),'SW.')]");

            switch (xmlTypeNode?.Name)
            {
                case "SW.Blocks.FB":
                case "SW.Blocks.FC":
                case "SW.Blocks.OB":
                    xmlInfo = ParseSoftwareBlock();
                    break;

                case "SW.Blocks.DB":
                    xmlInfo = ParseDataBlock();
                    break;

                case "SW.PlcType":
                    xmlInfo = ParsePlcType();
                    break;

                case "SW.Tags.PlcTagTable":
                    xmlInfo = ParsePlcTagTable();
                    break;

            }

            return xmlInfo;
        }

        /// <summary>
        /// Replace XML string to another
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="oldText"></param>
        /// <param name="newText"></param>
        public static void ReplaceXML(XmlDocument doc, string oldText, string newText)
        {
            string newTextFormated = SecurityElement.Escape(newText);
            doc.InnerXml = doc.InnerXml.Replace(oldText, newTextFormated);
        }

        /// <summary>
        /// Check if a block is a Database
        /// </summary>
        /// <param name="path"></param>
        /// <returns>boolean</returns>
        public static bool IsDB(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            var node = doc.SelectSingleNode("//SW.Blocks.GlobalDB");

            if (node != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Get text inside XML Tag "Name"
        /// </summary>
        /// <param name="doc"></param>
        /// <returns>Name</returns>
        public static string GetXmlNameAttribute(XmlDocument doc)
        {
            string name = null;

            XmlNode blockName = doc.SelectSingleNode("//Name");

            if (blockName != null)
                name = blockName.InnerText + ".xml";

            return name;
        }

        /// <summary>
        /// Renumbering of the document ID's (attributes)
        /// </summary>
        /// <param name="nodes">Change id's on this node</param>
        public static void IDRenumbering(XmlNodeList nodes)
        {
            int i = 1;
            foreach (XmlNode nodeAux in nodes)
            {
                if (nodeAux.Attributes["ID"] != null)
                {
                    nodeAux.Attributes["ID"].Value = i.ToString("X");
                    i++;
                }

                if (nodeAux.Attributes["CompositionName"] != null)
                {
                    if (nodeAux.Attributes["CompositionName"].Value == "Comment" || nodeAux.Attributes["CompositionName"].Value == "Title")
                    {
                        foreach (XmlNode nodeFC in nodeAux.FirstChild)
                        {
                            nodeFC.Attributes["ID"].Value = i.ToString("X");
                            i++;
                        }
                    }

                    if (nodeAux.Attributes["CompositionName"].Value == "CompileUnits")
                    {
                        foreach (XmlNode nodeFC in nodeAux.LastChild)
                        {
                            nodeFC.Attributes["ID"].Value = i.ToString("X");
                            i++;

                            foreach (XmlNode nodeSW in nodeFC.FirstChild)
                            {
                                nodeSW.Attributes["ID"].Value = i.ToString("X");
                                i++;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Replace invalid chars from string with '_'
        /// </summary>
        /// <returns></returns>
        public static string RemoveWindowsUnallowedChars(string s)
        {
            string newString = s;
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                if (newString.Contains(c))
                    newString = newString.Replace(c, '_');
            }

            return newString;
        }

        #region PLC Tags
        /// <summary>
        /// Insert a new tag
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="tagDoc"></param>
        /// <param name="symbolic"></param>
        /// <param name="dataType"></param>
        /// <param name="address"></param>
        /// <param name="comment"></param>
        /// <param name="externalAccessible"></param>
        /// <param name="externalVisible"></param>
        /// <param name="externalWritable"></param>
        /// <returns></returns>
        public static void InsertTag(XmlDocument doc, XmlDocument tagDoc, string symbolic, string dataType, string address, string comment, bool externalAccessible, bool externalVisible, bool externalWritable)
        {
            ReplaceXML(tagDoc, "TAGSYMBOLIC", symbolic);
            if (dataType.Contains("ST_"))
                dataType = "\"" + dataType + "\"";
            ReplaceXML(tagDoc, "TAGDATATYPE", dataType);
            ReplaceXML(tagDoc, "TAGADDRESS", address);
            ReplaceXML(tagDoc, "TAGCOMMENT", comment);
            ReplaceXML(tagDoc, "TAGEXTERNALACCESSIBLE", externalAccessible.ToString().ToLower());
            ReplaceXML(tagDoc, "TAGEXTERNALVISIBLE", externalVisible.ToString().ToLower());
            ReplaceXML(tagDoc, "TAGEXTERNALWRITABLE", externalWritable.ToString().ToLower());

            XmlNode tag = tagDoc.SelectSingleNode("/SW.Tags.PlcTag");
            XmlNode ObjectList = doc.SelectSingleNode("/Document/SW.Tags.PlcTagTable/ObjectList");
            XmlNode importNode = ObjectList.OwnerDocument.ImportNode(tag, true);

            ObjectList.AppendChild(importNode);
        }

        /// <summary>
        /// Convert XML Tags to a List of PLC_Tag's
        /// </summary>
        public static void XmlToPlcTags(string path)
        {
            DBMaker.PLC_Tags = new List<PLC_Tag>();

            XmlDocument xmlTagsDoc = new XmlDocument();
            xmlTagsDoc.Load(path);

            XmlNodeList tags = xmlTagsDoc.SelectNodes("//SW.Tags.PlcTagTable//ObjectList//SW.Tags.PlcTag");

            foreach (XmlNode tag in tags)
            {
                string name = tag.SelectSingleNode("AttributeList//Name").InnerText;
                string dataType = tag.SelectSingleNode("AttributeList//DataTypeName").InnerText;
                string address = tag.SelectSingleNode("AttributeList//LogicalAddress").InnerText;
                string comment = tag.SelectSingleNode("ObjectList//MultilingualText//ObjectList//MultilingualTextItem//AttributeList//Text").InnerText;
                string extAccessible = tag.SelectSingleNode("AttributeList//ExternalAccessible").InnerText;
                string extVisible = tag.SelectSingleNode("AttributeList//ExternalVisible").InnerText;
                string extWritable = tag.SelectSingleNode("AttributeList//ExternalWritable").InnerText;

                bool bExtAccessible;
                bool bExtVisible;
                bool bExtWritable;

                try
                {
                    bExtAccessible = bool.Parse(extAccessible);
                    bExtVisible = bool.Parse(extWritable);
                    bExtWritable = bool.Parse(extVisible);
                } 
                catch(Exception)
                {
                    throw new Exception("Invalid \"External\" tag : Values must be boolean.");
                }

                DBMaker.PLC_Tags.Add(new PLC_Tag()
                {
                    Name = name,
                    Symbols = "",
                    DataType = dataType,
                    Address = address,
                    Comment = comment,
                    Accessible = bExtAccessible,
                    Writable = bExtWritable,
                    Visible = bExtVisible
                });
            }
        }
        
        /// <summary>
        /// Check if Xml doc is a PLC Tags doc
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsPlcTags(string path)
        {
            bool isPlcTags = false;
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode SwTagsPlcTagTable = doc.SelectSingleNode("//Document//SW.Tags.PlcTagTable");

            if (SwTagsPlcTagTable != null)
                isPlcTags = true;

            return isPlcTags;
        }
        
        /// <summary>
        /// Create tag for Fail
        /// </summary>
        /// <param name="symbolic"></param>
        /// <param name="datatype"></param>
        /// <param name="comment"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static XElement CreateTag(string symbolic, string datatype, string comment, string address)
        {
            XElement tag = new XElement("Tag",symbolic, new XAttribute("type", datatype), new XAttribute("hmiVisible", "False")
                                                           , new XAttribute("hmiWriteable", "False"), new XAttribute("hmiAccessible", "False")
                                                           , new XAttribute("retain", "False"), new XAttribute("remark", comment)
                                                           , new XAttribute("addr", "%" + address));
            return tag;
        }
        #endregion

        #endregion

        #region private methods

        /// <summary>Parse XML of Controller Tag Table</summary>
        /// <returns>XmlInformation</returns>
        private XmlInformation ParsePlcTagTable()
        {
            var tagTableInfo = new PlcTagTableInformation();

            tagTableInfo.XmlType = TiaXmlType.PlcTagTable;
            tagTableInfo.Name = GetMetaInformation(XPathConstants.Xpathblockname);

            var listOfConstants = RootNode.SelectNodes("//ObjectList/SW.Tags.PlcConstant");
            var listOfTags = RootNode.SelectNodes("//ObjectList/SW.Tags.PlcTag");

            //get all infos about constants within tagtable
            #region constants infos

            if (listOfConstants != null)
                foreach (XmlNode constantInTable in listOfConstants)
                {
                    var constant = new PlcTagTableConstant();

                    foreach (XmlNode child in constantInTable.ChildNodes)
                    {
                        if (child.Name.Equals("AttributeList"))
                        {
                            foreach (XmlNode node in child.ChildNodes)
                            {
                                //get Name of constant
                                if (node.Name.Equals("Name"))
                                {
                                    constant.MemberName = node.InnerText;
                                }
                                //get value of constant
                                if (node.Name.Equals("Value"))
                                {
                                    constant.ConstantValue = node.InnerText;
                                }
                            }
                        }

                        if (child.Name.Equals("LinkList"))
                        {
                            foreach (XmlNode node in child.ChildNodes)
                            {
                                //get datatype of constant
                                constant.MemberDatatype = node.InnerText;
                            }
                        }

                        if (child.Name.Equals("ObjectList"))
                        {
                            var comment = child.SelectSingleNode("MultilingualText");
                            constant.MemberComment.CompositionNameInXml = comment?.Attributes?["CompositionName"].Value;

                            GetTitleOrComment(child, constant.MemberComment);
                        }
                    }

                    tagTableInfo.Constants.Add(constant);
                }

            #endregion

            //get all infos about tags within tagtable
            #region tag infos

            if (listOfTags != null)
                foreach (XmlNode tagsInTable in listOfTags)
                {
                    var tag = new PlcTagTableTag();

                    foreach (XmlNode child in tagsInTable.ChildNodes)
                    {
                        if (child.Name.Equals("AttributeList"))
                        {
                            foreach (XmlNode node in child.ChildNodes)
                            {
                                //get Name of constant
                                if (node.Name.Equals("Name"))
                                {
                                    tag.MemberName = node.InnerText;
                                }
                                //get value of constant
                                if (node.Name.Equals("LogicalAddress"))
                                {
                                    tag.Address = node.InnerText;
                                }
                            }
                        }

                        if (child.Name.Equals("LinkList"))
                        {
                            foreach (XmlNode node in child.ChildNodes)
                            {
                                //get datatype of constant
                                tag.MemberDatatype = node.InnerText;
                            }
                        }

                        if (child.Name.Equals("ObjectList"))
                        {
                            var comment = child.SelectSingleNode("MultilingualText");
                            tag.MemberComment.CompositionNameInXml = comment?.Attributes?["CompositionName"].Value;

                            GetTitleOrComment(child, tag.MemberComment);
                        }
                    }

                    tagTableInfo.Tags.Add(tag);

                }

            #endregion

            return tagTableInfo;
        }

        /// <summary>Parse XML of Software Block</summary>
        /// <returns>XmlInformation</returns>
        private XmlInformation ParseSoftwareBlock()
        {
            var blockInfo = new BlockInformation();

            blockInfo.XmlType = TiaXmlType.Block;
            blockInfo.Name = GetMetaInformation(XPathConstants.Xpathblockname);
            blockInfo.BlockNumber = GetMetaInformation(XPathConstants.Xpathblocknumber);
            blockInfo.BlockLanguage = GetMetaInformation(XPathConstants.Xpathblocklanguage);
            blockInfo.BlockType = GetMetaInformation(XPathConstants.Xpathblocktype);
            blockInfo.BlockMemoryLayout = GetMetaInformation(XPathConstants.Xpathblockmemorylayout);
            blockInfo.BlockAutoNumber = GetMetaInformation(XPathConstants.XpathAutonumber);
            blockInfo.BlockEnableTagReadback = GetMetaInformation(XPathConstants.XpathEnabletagreadback);
            blockInfo.BlockEnableTagReadbackBlockProperties = GetMetaInformation(XPathConstants.XpathEnabletagreadbackblockproperties);
            blockInfo.BlockIsIecCheckEnabled = GetMetaInformation(XPathConstants.XpathIsieccheckenabled);

            blockInfo.BlockAuthor = GetMetaInformation(XPathConstants.Xpathblockauthor);
            blockInfo.BlockTitle = GetBlockTitleOrComment(XPathConstants.Xpathblocktitle);
            blockInfo.BlockComment = GetBlockTitleOrComment(XPathConstants.Xpathblockcomment);
            blockInfo.BlockFamily = GetMetaInformation(XPathConstants.Xpathblockfamily);
            blockInfo.BlockVersion = GetMetaInformation(XPathConstants.Xpathblockversion);
            blockInfo.BlockUserId = GetMetaInformation(XPathConstants.Xpathblockuserid);

            blockInfo.BlockInterface = GetBlockInterfaceInformation();

            blockInfo.BlockNetworks = GetBlockNetworkInformation();

            return blockInfo;
        }

        /// <summary>Parse XML of Data Block</summary>
        /// <returns>XmlInformation</returns>
        private XmlInformation ParseDataBlock()
        {
            var dataBlockInfo = new DataBlockInformation();
            dataBlockInfo.XmlType = TiaXmlType.DataBlock;

            dataBlockInfo.Name = GetMetaInformation(XPathConstants.Xpathblockname);
            dataBlockInfo.DbType = GetDatablockType();

            if (dataBlockInfo.DbType.Equals(DatablockType.ArrayDb))
            {
                dataBlockInfo.ArrayDataType = GetMetaInformation(XPathConstants.Xpatharraydatatype);
                dataBlockInfo.InstanceOfName = String.Empty;
            }
            else if (dataBlockInfo.DbType.Equals(DatablockType.IdBofFb) || dataBlockInfo.DbType.Equals(DatablockType.DBofUdt))
            {
                dataBlockInfo.InstanceOfName = GetMetaInformation(XPathConstants.Xpathinstanceofname);
                dataBlockInfo.ArrayDataType = String.Empty;
            }
            else
            {
                dataBlockInfo.ArrayDataType = String.Empty;
                dataBlockInfo.InstanceOfName = String.Empty;
            }

            dataBlockInfo.BlockNumber = GetMetaInformation(XPathConstants.Xpathblocknumber);
            dataBlockInfo.BlockLanguage = GetMetaInformation(XPathConstants.Xpathblocklanguage);
            dataBlockInfo.BlockType = GetMetaInformation(XPathConstants.Xpathblocktype);
            dataBlockInfo.BlockMemoryLayout = GetMetaInformation(XPathConstants.Xpathblockmemorylayout);
            dataBlockInfo.DownloadWithoutReinit = GetMetaInformation(XPathConstants.XpathDownloadwithoutreinit);
            dataBlockInfo.IsOnlyStoredInLoadMemory = GetMetaInformation(XPathConstants.XpathIsonlystoredinloadmemory);
            dataBlockInfo.IsRetainMemResEnabled = GetMetaInformation(XPathConstants.XpathIsretainmemresenabled);
            dataBlockInfo.IsWriteProtectedInAs = GetMetaInformation(XPathConstants.XpathIswriteprotectedinas);
            dataBlockInfo.MemoryReserve = GetMetaInformation(XPathConstants.XpathMemoryreserve);
            dataBlockInfo.BlockAutoNumber = GetMetaInformation(XPathConstants.XpathAutonumber);

            dataBlockInfo.BlockAuthor = GetMetaInformation(XPathConstants.Xpathblockauthor);
            dataBlockInfo.BlockTitle = GetBlockTitleOrComment(XPathConstants.Xpathblocktitle);
            dataBlockInfo.BlockComment = GetBlockTitleOrComment(XPathConstants.Xpathblockcomment);
            dataBlockInfo.BlockFamily = GetMetaInformation(XPathConstants.Xpathblockfamily);
            dataBlockInfo.BlockVersion = GetMetaInformation(XPathConstants.Xpathblockversion);
            dataBlockInfo.BlockUserId = GetMetaInformation(XPathConstants.Xpathblockuserid);

            dataBlockInfo.BlockInterface = GetBlockInterfaceInformation();

            return dataBlockInfo;
        }

        /// <summary>Parse XML of Controller Data type</summary>
        /// <returns></returns>
        private XmlInformation ParsePlcType()
        {
            var udtInfo = new PlcTypeInformation();

            udtInfo.Name = GetMetaInformation(XPathConstants.Xpathblockname);
            udtInfo.XmlType = TiaXmlType.PlcType;

            udtInfo.UdtTitle = GetBlockTitleOrComment(XPathConstants.Xpathblocktitle);
            udtInfo.UdtComment = GetBlockTitleOrComment(XPathConstants.Xpathblockcomment);

            udtInfo.DatatypeMember = GetUdtMembers();

            return udtInfo;

        }

        /// <summary>Gets the type of the datablock.</summary>
        /// <returns></returns>
        /// TODO Edit XML Comment Template for GetDatablockType
        private DatablockType GetDatablockType()
        {
            Node = RootNode.SelectSingleNode(XPathConstants.Xpathdatablocktype);

            if (Node != null)
            {
                var myType = (DatablockType)Enum.Parse(typeof(DatablockType), Node.InnerText);
                return myType;
            }
            return 0;
        }

        /// <summary>Gets the meta information.</summary>
        /// <param name="xpath">The xpath.</param>
        /// <returns>String</returns>
        /// TODO Edit XML Comment Template for GetMetaInformation
        private string GetMetaInformation(string xpath)
        {
            Node = RootNode.SelectSingleNode(xpath);
            string innerText = "";

            if (Node != null)
            {
                innerText = Node.InnerText;
            }

            return innerText;
        }

        /// <summary>Gets the block interface information.</summary>
        /// <returns>BlockInterface</returns>
        /// TODO Edit XML Comment Template for GetBlockInterfaceInformation
        private BlockInterface GetBlockInterfaceInformation()
        {
            //add xml specific namespace
            //XmlNamespaceManager ns = new XmlNamespaceManager(Document.NameTable);
            //ns.AddNamespace("SI", "http://www.siemens.com/automation/Openness/SW/Interface/v1");

            //local BlockInformation Object
            var blockInterface = new BlockInterface();

            ParseInterface(RootNode.SelectNodes("//Interface/SI:Sections/SI:Section", _ns), blockInterface.InterfaceSections);

            return blockInterface;
        }

        /// <summary>Parses the interface.</summary>
        /// <param name="listOfSections">The list of sections.</param>
        /// <param name="interfaceSections">The interface sections.</param>
        /// TODO Edit XML Comment Template for ParseInterface
        private void ParseInterface(XmlNodeList listOfSections, List<BlockInterfaceSection> interfaceSections)
        {
            foreach (XmlNode section in listOfSections)
            {
                //continue if Block is graph block and have section with name="Base"(invisible for User in TIA Portal)
                if (section.Attributes != null && section.Attributes["Name"].Value.Equals("Base"))
                {
                    continue;
                }

                //new section within the BlockInterface
                var blockInterfaceSection = new BlockInterfaceSection(section.Attributes?["Name"].Value);

                //add BlockInterface Section
                interfaceSections.Add(blockInterfaceSection);

                //list of Member within each section
                var listOfMember = section.ChildNodes;

                foreach (XmlNode member in listOfMember)
                {
                    Member blockIMember = null;

                    //Struct Member
                    if (member.Attributes != null && member.Attributes["Datatype"].Value.Equals("Struct"))
                    {
                        blockIMember = new Struct(member.Attributes["Name"].Value);
                        GetStructChildNodes(member.ChildNodes, (Struct)blockIMember);
                    }

                    //MultiInstance Member
                    else if (member.Attributes != null && (member.LocalName.Equals("Member") && member.HasChildNodes && member.Attributes["Datatype"].Value.Contains("\"") && !member.Attributes["Datatype"].Value.Contains("Array of")))
                    {
                        foreach (XmlNode child in member.ChildNodes)
                        {
                            if (child.Name.Equals("Sections"))
                            {
                                blockIMember = new MultiInstance(member.Attributes["Name"].Value, member.Attributes["Datatype"].Value);
                                GetMultiInstanceMember(member, (MultiInstance)blockIMember);
                            }
                        }
                    }

                    //normal Member
                    else
                    {
                        blockIMember = new Member(member.Attributes?["Name"].Value, member.Attributes?["Datatype"].Value);
                    }

                    if (blockIMember != null)
                    {
                        GetStartValue(member, blockIMember);
                        GetComment(member, blockIMember);
                        blockInterfaceSection.InterfaceMember.Add(blockIMember);
                    }
                }
            }
        }

        /// <summary>Gets the block network information.</summary>
        /// <returns>List&lt;Network&gt;</returns>
        /// TODO Edit XML Comment Template for GetBlockNetworkInformation
        private List<Network> GetBlockNetworkInformation()
        {
            var networks = new List<Network>();

            var listOfNetworks = RootNode.SelectNodes("//SW.Blocks.CompileUnit");

            if (listOfNetworks != null)
                foreach (XmlNode network in listOfNetworks)
                {
                    var blockNetwork = new Network();

                    #region Title/Comment

                    var listMultiLingualText = network.SelectNodes(".//MultilingualText");

                    foreach (XmlNode nodeMultiLingualText in listMultiLingualText)
                    {

                        if (nodeMultiLingualText.Attributes["CompositionName"].Value.Equals("Title"))
                        {
                            blockNetwork.NetworkTitle.CompositionNameInXml = nodeMultiLingualText.Attributes["CompositionName"].Value;
                            GetTitleOrComment(nodeMultiLingualText, blockNetwork.NetworkTitle);

                        }

                        if (nodeMultiLingualText.Attributes["CompositionName"].Value.Equals("Comment"))
                        {
                            blockNetwork.NetworkComment.CompositionNameInXml = nodeMultiLingualText.Attributes["CompositionName"].Value;
                            GetTitleOrComment(nodeMultiLingualText, blockNetwork.NetworkComment);
                        }
                    }

                    #endregion

                    #region Access

                    //all used Tags with Symbol information (excl. Constants)
                    var listOfAccess = network.SelectNodes(".//siemensNetworks:Access[siemensNetworks:Symbol]", _ns);

                    foreach (XmlNode access in listOfAccess)
                    {
                        var memberAccess = new Access();
                        var accessSymbol = new Symbol();

                        memberAccess.AccessScope = access.Attributes["Scope"].Value;
                        memberAccess.MemberDatatype = string.Empty; //access.Attributes["Type"].Value; 
                        memberAccess.UId = access.Attributes["UId"].Value;

                        var listOfSymbolComponentsWithAccessModifier = access.SelectNodes(".//siemensNetworks:Symbol/siemensNetworks:Component", _ns);

                        foreach (XmlNode component in listOfSymbolComponentsWithAccessModifier)
                        {
                            try
                            {
                                accessSymbol.Components.Add(component.Attributes["Name"].Value);
                                accessSymbol.SimpleAccessModifier = component.Attributes["SimpleAccessModifier"].Value;
                            }

                            catch (NullReferenceException)
                            {
                                //catch NullReferenceException if Access Component does not have the Attribute "SimpleAccessModifier"
                            }

                        }
                        memberAccess.AccessSymbol = accessSymbol;
                        memberAccess.MemberName = accessSymbol.ToString();

                        blockNetwork.NetworkAccess.Add(memberAccess);

                    }

                    #endregion

                    #region Part

                    var listOfPart = network.SelectNodes(".//siemensNetworks:Part", _ns);

                    foreach (XmlNode part in listOfPart)
                    {
                        var usedInstruction = new Instruction(part.Attributes["Name"].Value, part.Attributes["UId"].Value);

                        blockNetwork.NetworkInstructions.Add(usedInstruction);
                    }

                    #endregion

                    #region CallRef

                    var listOfCallRef = network.SelectNodes(".//siemensNetworks:Call", _ns);

                    foreach (XmlNode nodeCallref in listOfCallRef)
                    {
                        var callref = new CallRef();

                        callref.CallType = string.Empty; // nodeCallref.Attributes["CallType"].Value;
                        callref.UId = nodeCallref.Attributes["UId"].Value;

                        var nodeCallInfo = nodeCallref.SelectSingleNode(".//siemensNetworks:CallInfo", _ns);

                        callref.Name = nodeCallInfo.Attributes["Name"].Value;
                        callref.BlockType = nodeCallInfo.Attributes["BlockType"].Value;

                        blockNetwork.NetworkCalls.Add(callref);
                    }

                    #endregion

                    networks.Add(blockNetwork);
                }


            return networks;
        }

        /// <summary>Gets the udt members.</summary>
        /// <returns>List&lt;Member&gt;</returns>
        /// TODO Edit XML Comment Template for GetUdtMembers
        private List<Member> GetUdtMembers()
        {
            //add xml specific namespace
            var ns = new XmlNamespaceManager(Document.NameTable);
            ns.AddNamespace("SI", "http://www.siemens.com/automation/Openness/SW/Interface/v1");

            var memberList = new List<Member>();

            //List of Sections within the BlockInterface (Input, Output ....) 
            var listOfSections = RootNode.SelectNodes("//Interface/SI:Sections/SI:Section", ns);

            if (listOfSections != null)
                foreach (XmlNode section in listOfSections)
                {
                    //list of Member within each section
                    var listOfMember = section.ChildNodes;

                    foreach (XmlNode member in listOfMember)
                    {
                        Member blockIMember = null;

                        if (member.Attributes != null && member.Attributes["Datatype"].Value.Equals("Struct"))
                        {
                            blockIMember = new Struct(member.Attributes["Name"].Value);

                            GetStructChildNodes(member.ChildNodes, (Struct)blockIMember);
                        }
                        else if (member.Attributes != null && (member.LocalName.Equals("Member") && member.HasChildNodes && member.Attributes["Datatype"].Value.Contains("\"") && !member.Attributes["Datatype"].Value.Contains("Array of")))
                        {
                            foreach (XmlNode child in member.ChildNodes)
                            {
                                if (child.Name.Equals("Sections"))
                                {
                                    blockIMember = new MultiInstance(member.Attributes["Name"].Value, member.Attributes["Datatype"].Value);
                                    GetMultiInstanceMember(member, (MultiInstance)blockIMember);
                                }
                            }
                        }
                        else
                        {
                            blockIMember = new Member(member.Attributes?["Name"].Value, member.Attributes?["Datatype"].Value);
                        }
                        if (blockIMember != null)
                        {
                            GetStartValue(member, blockIMember);
                            GetComment(member, blockIMember);

                            memberList.Add(blockIMember);
                        }
                    }
                }

            return memberList;
        }

        /// <summary>Gets the structure child nodes.</summary>
        /// <param name="childNodes">The child nodes.</param>
        /// <param name="structMember">The structure member.</param>
        /// TODO Edit XML Comment Template for GetStructChildNodes
        private void GetStructChildNodes(XmlNodeList childNodes, Struct structMember)
        {
            foreach (XmlNode node in childNodes)
            {
                if (node.Name.Equals("Member"))
                {
                    Member blockIMember = null;

                    if (node.Attributes != null && node.Attributes["Datatype"].Value.Equals("Struct"))
                    {
                        blockIMember = new Struct(node.Attributes["Name"].Value);

                        GetStructChildNodes(node.ChildNodes, (Struct)blockIMember);
                    }
                    else if (node.Attributes != null && (node.LocalName.Equals("Member") && node.HasChildNodes && node.Attributes["Datatype"].Value.Contains("\"") && !node.Attributes["Datatype"].Value.Contains("Array of")))
                    {
                        foreach (XmlNode child in node.ChildNodes)
                        {
                            if (child.Name.Equals("Sections"))
                            {
                                blockIMember = new MultiInstance(node.Attributes["Name"].Value, node.Attributes["Datatype"].Value);
                                GetMultiInstanceMember(node, (MultiInstance)blockIMember);
                            }
                        }
                    }
                    else
                    {
                        if (node.Attributes != null)
                            blockIMember = new Member(node.Attributes["Name"].Value, node.Attributes["Datatype"].Value);
                    }
                    if (blockIMember != null)
                    {
                        GetStartValue(node, blockIMember);
                        GetComment(node, blockIMember);
                        structMember.NestedMembers.Add(blockIMember);
                    }
                }

            }

        }

        /// <summary>Gets the multi instance member.</summary>
        /// <param name="member">The member.</param>
        /// <param name="instance">The instance.</param>
        /// TODO Edit XML Comment Template for GetMultiInstanceMember
        private void GetMultiInstanceMember(XmlNode member, MultiInstance instance)
        {
            foreach (XmlNode node in member.ChildNodes)
            {
                ParseInterface(node.SelectNodes("SI:Section", _ns), instance.InterfaceSections);
            }
        }

        /// <summary>Gets the comment.</summary>
        /// <param name="childNodes">The child nodes.</param>
        /// <param name="blockIMember">The block i member.</param>
        /// TODO Edit XML Comment Template for GetComment
        private void GetComment(XmlNode childNodes, Member blockIMember)
        {
            var commentList = childNodes.SelectNodes("SI:Comment", _ns);

            if (commentList != null)
                foreach (XmlNode comment in commentList)
                {
                    if (comment != null)
                    {
                        var listMultiLanguageText = comment.SelectNodes("SI:MultiLanguageText", _ns);
                        blockIMember.MemberComment.CompositionNameInXml = string.Empty;

                        if (listMultiLanguageText != null)
                            foreach (XmlNode multiLanguageText in listMultiLanguageText)
                            {
                                if (multiLanguageText.Attributes != null)
                                    blockIMember.MemberComment.MultiLanguageTextItems.Add(multiLanguageText.Attributes["Lang"].Value, multiLanguageText.InnerText);
                            }
                    }
                }
        }

        /// <summary>Gets the start value.</summary>
        /// <param name="childNodes">The child nodes.</param>
        /// <param name="blockIMember">The block i member.</param>
        /// TODO Edit XML Comment Template for GetStartValue
        private void GetStartValue(XmlNode childNodes, Member blockIMember)
        {
            var startValueList = childNodes.SelectNodes("SI:StartValue", _ns);

            if (startValueList != null)
                foreach (XmlNode startValue in startValueList)
                {
                    if (startValue != null)
                    {
                        blockIMember.MemberDefaultValue = startValue.InnerText;
                    }
                }
        }

        /// <summary>Gets the title or comment.</summary>
        /// <param name="nodeToMultiLanguageText">The node to multi language text.</param>
        /// <param name="textItems">The text items.</param>
        /// TODO Edit XML Comment Template for GetTitleOrComment
        private void GetTitleOrComment(XmlNode nodeToMultiLanguageText, MultiLanguageText textItems)
        {
            var listTextItemValue = nodeToMultiLanguageText.SelectNodes(".//Value");

            if (listTextItemValue != null)
                foreach (XmlNode nodeValue in listTextItemValue)
                {
                    if (nodeValue.Attributes != null)
                        textItems.MultiLanguageTextItems.Add(nodeValue.Attributes["lang"].Value, nodeValue.InnerText);
                }
        }

        /// <summary>Gets the block title or comment.</summary>
        /// <param name="xpath">The xpath.</param>
        /// <returns>MultiLanguageText</returns>
        /// TODO Edit XML Comment Template for GetBlockTitleOrComment
        private MultiLanguageText GetBlockTitleOrComment(string xpath)
        {
            var listTitleOrComment = RootNode.SelectNodes(xpath);
            var textItems = new MultiLanguageText();

            if (listTitleOrComment != null)
                foreach (XmlNode blockTitleNode in listTitleOrComment)
                {
                    if (blockTitleNode.ParentNode?.ParentNode != null && blockTitleNode.ParentNode.ParentNode.Name.Contains("SW.Blocks"))
                    {
                        if (blockTitleNode.Attributes != null)
                            textItems.CompositionNameInXml = blockTitleNode.Attributes["CompositionName"].Value;
                        GetTitleOrComment(blockTitleNode, textItems);
                    }
                }

            return textItems;
        }

        #endregion
    }
}
