using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TiaOpennessHelper.Models.Block;
using TiaOpennessHelper.Models.Members;
using TiaOpennessHelper.Models.SCL;

namespace TiaOpennessHelper.SCLParser
{
    /// <summary>
    /// Class to parse scl source files.
    /// </summary>
    public class SclParser
    {
        #region properties
        /// <summary>Regex string for header</summary>
        private const string StrHeader = "(?:(?<Type>FUNCTION_BLOCK|FUNCTION|ORGANIZATION_BLOCK) \"(?<Name>[^\"].*?)\"(?: : (?<Return>[^\"].*?))?\r\n){1}(?:TITLE = (?<Title>.*?)\r\n)?(?:{ S7_Optimized_Access := '(?<Layout>\\w*)' }\r\n)?(?:AUTHOR : (?<Author>.*?)\r\n)?(?:FAMILY : (?<Family>.*?)\r\n)?(?:NAME : '(?<UID>.*?)'\r\n)?(?:VERSION : (?<Version>\\d+.\\d+)\r\n)?(?://(?<Comment>.*?)\r\n)?";
        /// <summary>Regex string for input variables</summary>
        private const string StrVarIn = "(?:VAR_INPUT \r\n)(?<Content>(.*?\r\n)*?)(?:\\s*END_VAR)";
        /// <summary>Regex string for output variables</summary>
        private const string StrVarOut = "(?:VAR_OUTPUT \r\n)(?<Content>(.*?\r\n)*?)(?:\\s*END_VAR)";
        /// <summary>Regex string for inout variables</summary>
        private const string StrVarInOut = "(?:VAR_IN_OUT \r\n)(?<Content>(.*?\r\n)*?)(?:\\s*END_VAR)";
        /// <summary>Regex string for temporary variables</summary>
        private const string StrVarTemp = "(?:VAR_TEMP \r\n)(?<Content>(.*?\r\n)*?)(?:\\s*END_VAR)";
        /// <summary>Regex string for static variables</summary>
        private const string StrVarStat = "(?:VAR \r\n)(?<Content>(.*?\r\n)*?)(?:\\s*END_VAR)";
        /// <summary>Regex string for constant variables</summary>
        private const string StrVarConst = "(?:VAR CONSTANT \r\n)(?<Content>(.*?\r\n)*?)(?:\\s*END_VAR)";
        /// <summary>First part of regex string for all members</summary>
        private const string StrMember1 = "(?:(?:^(?:\\s{";
        /// <summary>Second part of regex string for all members</summary>
        private const string StrMember2 = "}(?<MName>[^ ].*?)(?: {(?<MAttributes>.*?)})* : (?<MType>.*?)(?:(?: := )(?<MValue>.*?))?;(?:(?:\\s*//\\s)(?<MComment>.*)\r\n)?))|(?:(?:\\s*(?<SName>[^ ].*?) : Struct(?:(?:\\s*//\\s)(?<SComment>.*))?\r\n)(?<SContent>(?:.*?\r\n)*)(?:\\s*?END_STRUCT;)))";
        /// <summary>Regex string for the code part</summary>
        private const string StrCode = "(?:BEGIN\r\n)(?<Code>(?:.*\r\n)*)(?:(?:END_FUNCTION_BLOCK|END_FUNCTION|END_ORGANIZATION_BLOCK)\r\n)";
        #endregion

        #region public methods
        /// <summary>Parses the given file and returns the result as a SCLBlock.</summary>
        /// <param name="fileName">Path to file to parse</param>
        /// <returns>filled SCLBlock</returns>
        /// <exception cref="System.ArgumentException">Arguent is null or empty;fileName</exception>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static SclBlock Parse(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("Arguent is null or empty", nameof(fileName));

            var block = new SclBlock();

            // use ReadAllText to read File in a single string
            var lines = File.ReadAllText(fileName);

            // read header information
            var reHeader = new Regex(StrHeader, RegexOptions.Compiled | RegexOptions.Multiline);
            // read VAR_INPUT
            var reInput = new Regex(StrVarIn, RegexOptions.Compiled | RegexOptions.Multiline);
            // read VAR_OUTPUT
            var reOutput = new Regex(StrVarOut, RegexOptions.Compiled | RegexOptions.Multiline);
            // read VAR_INOUT
            var reInOut = new Regex(StrVarInOut, RegexOptions.Compiled | RegexOptions.Multiline);
            // read VAR
            var reStat = new Regex(StrVarStat, RegexOptions.Compiled | RegexOptions.Multiline);
            // read VAR_TEMP
            var reTemp = new Regex(StrVarTemp, RegexOptions.Compiled | RegexOptions.Multiline);
            // read VAR_CONSTANT
            var reConstant = new Regex(StrVarConst, RegexOptions.Compiled | RegexOptions.Multiline);
            // read Member
            var reMember = new Regex(StrMember1 + "6" + StrMember2, RegexOptions.Compiled | RegexOptions.Multiline);
            // read Code
            var reCode = new Regex(StrCode, RegexOptions.Compiled | RegexOptions.Multiline);

            ParseHeader(block, lines, reHeader);

            ParseInterface(block, "Input", lines, reInput, reMember);
            ParseInterface(block, "Output", lines, reOutput, reMember);
            ParseInterface(block, "InOut", lines, reInOut, reMember);
            ParseInterface(block, "Static", lines, reStat, reMember);
            ParseInterface(block, "Temp", lines, reTemp, reMember);
            ParseInterface(block, "Constant", lines, reConstant, reMember);

            var m = reCode.Match(lines);
            if (m.Success)
            {
                block.BlockCode = m.Groups["Code"].Value;
            }

            return block;
        }
        #endregion

        #region private methods
        /// <summary>
        /// Parses the definition section (header) of a source file and adds it to the block
        /// </summary>
        /// <param name="block">The block.</param>
        /// <param name="lines">The lines.</param>
        /// <param name="header">The header.</param>
        private static void ParseHeader(SclBlock block, string lines, Regex header)
        {
            var match = header.Match(lines);
            if (match.Success)
            {
                if (match.Groups["Type"].Value.Equals("FUNCTION_BLOCK"))               // BlockType
                    block.BlockType = "FB";
                else if (match.Groups["Type"].Value.Equals("FUNCTION"))
                    block.BlockType = "FC";
                else
                    block.BlockType = "OB";

                block.Name = match.Groups["Name"].Value;               // Name
                block.BlockTitle.MultiLanguageTextItems["en-US"] = match.Groups["Title"].Value;              // BlockTitel
                block.BlockMemoryLayout = match.Groups["Layout"].Value;       // MemoryLayout (opt./nopt.)
                block.BlockAuthor = match.Groups["Author"].Value;             // Author
                block.BlockFamily = match.Groups["Family"].Value;             // Family
                block.BlockUserId = match.Groups["UID"].Value;             // User ID
                block.BlockVersion = match.Groups["Version"].Value;            // Version
                block.BlockComment.MultiLanguageTextItems["en-US"] = match.Groups["Comment"].Value;           // Comment
            }
        }

        /// <summary>
        /// Parses the interface section (header) of a source file and adds it to the block
        /// </summary>
        /// <param name="block">The block.</param>
        /// <param name="interfaceName">Name of the interface.</param>
        /// <param name="lines">The lines.</param>
        /// <param name="reInterface">The re interface.</param>
        /// <param name="reMember">The re member.</param>
        private static void ParseInterface(SclBlock block, string interfaceName, string lines, Regex reInterface, Regex reMember)
        {
            var match = reInterface.Match(lines);
            if (match.Success)
            {
                var section = new BlockInterfaceSection(interfaceName);
                block.BlockInterface.InterfaceSections.Add(section);
                foreach (Match m in reMember.Matches(match.Groups["Content"].Value))
                {
                    if (m.Success)
                    {
                        if (m.Groups["MName"].Value != "")
                        {
                            var member = new Member();
                            member.MemberName = m.Groups["MName"].Value;
                            member.MemberDatatype = m.Groups["MType"].Value;
                            member.MemberComment.MultiLanguageTextItems["en-US"] = m.Groups["MComment"].Value;
                            member.MemberDefaultValue = m.Groups["MValue"].Value;
                            section.InterfaceMember.Add(member);
                        }
                        else
                        {
                            var member = new Struct(m.Groups["SName"].Value);
                            member.MemberComment.MultiLanguageTextItems["en-US"] = m.Groups["SComment"].Value;
                            member.NestedMembers.AddRange(ParseStruct(m.Groups["SContent"].Value, 1));
                            section.InterfaceMember.Add(member);
                        }
                    }
                }
            }
        }

        /// <summary>Parses a member of type struct and returns all child elements</summary>
        /// <param name="lines">The lines.</param>
        /// <param name="indent">The indent.</param>
        /// <returns>List&lt;Member&gt;</returns>
        private static List<Member> ParseStruct(string lines, int indent)
        {
            var ret = new List<Member>();

            var reMember = new Regex(StrMember1 + (6 + indent * 3) + StrMember2, RegexOptions.Multiline);

            foreach (Match m in reMember.Matches(lines))
            {
                if (m.Success)
                {
                    if (m.Groups["MName"].Value != "")
                    {
                        var member = new Member();
                        member.MemberName = m.Groups["MName"].Value;
                        member.MemberDatatype = m.Groups["MType"].Value;
                        member.MemberComment.MultiLanguageTextItems["en-US"] = m.Groups["MComment"].Value;
                        member.MemberDefaultValue = m.Groups["MValue"].Value;
                        ret.Add(member);
                    }
                    else
                    {
                        var member = new Struct(m.Groups["SName"].Value);
                        member.MemberComment.MultiLanguageTextItems["en-US"] = m.Groups["SComment"].Value;
                        member.NestedMembers.AddRange(ParseStruct(m.Groups["SContent"].Value, indent++));
                        ret.Add(member);
                    }
                }
            }

            return ret;
        }
        #endregion
    }
}
