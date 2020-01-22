using System;
using System.Collections.Generic;
using TiaOpennessHelper.Utils;

namespace TiaOpennessHelper.Models.Members
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Member" />
    /// TODO Edit XML Comment Template for Struct
    public class Struct : Member
    {
        /// <summary>The nested members</summary>
        /// TODO Edit XML Comment Template for nestedMembers
        public List<Member> NestedMembers;

        /// <summary>Initializes a new instance of the <see cref="Struct"/> class.</summary>
        /// <param name="name">The name.</param>
        /// TODO Edit XML Comment Template for #ctor
        public Struct(string name)
        {
            NestedMembers = new List<Member>();
            MemberName = name;
            MemberDatatype = "Struct";
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        /// TODO Edit XML Comment Template for ToString
        public override string ToString()
        {
            var ret = "";
            var id = 0;

            ret += "<Member Name=\"" + AdjustNames.AdjustXmlStrings(MemberName) + "\" Datatype=\"" + AdjustNames.AdjustXmlStrings(MemberDatatype) + "\">" + Environment.NewLine;
            if (MemberComment.CompositionNameInXml != null)
                ret += MemberComment.ToString(ref id);
            foreach (var member in NestedMembers)
            {
                ret += member.ToString();
            }
            ret += "</Member>" + Environment.NewLine;

            return ret;
        }
    }
}
