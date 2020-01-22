namespace TiaPortalOpennessDemo.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    /// TODO Edit XML Comment Template for ComboBoxItem
    public class ComboBoxItem
    {
        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        /// TODO Edit XML Comment Template for Name
        public string Name { get; set; }
        /// <summary>Gets or sets the item.</summary>
        /// <value>The item.</value>
        /// TODO Edit XML Comment Template for Item
        public object Item { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComboBoxItem"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="item">The item.</param>
        /// TODO Edit XML Comment Template for #ctor
        public ComboBoxItem(string name, object item)
        {
            Name = name;
            Item = item;
        }
    }
}
