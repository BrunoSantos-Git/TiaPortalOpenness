using System;
using System.Collections.ObjectModel;
using Siemens.Engineering.Connection;
using Siemens.Engineering.Online;
using TiaPortalOpennessDemo.Commands;
using TiaPortalOpennessDemo.Utilities;

namespace TiaPortalOpennessDemo.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="TiaPortalOpennessDemo.ViewModels.ViewModelBase" />
    /// TODO Edit XML Comment Template for ConnectionConfigurationViewModel
    public class ConnectionConfigurationViewModel : ViewModelBase
    {
        /// <summary>
        /// </summary>
        /// TODO Edit XML Comment Template for plc
        private OnlineProvider _plc;

        /// <summary>
        /// Gets a value indicating whether this <see cref="ConnectionConfigurationViewModel"/> is result.
        /// </summary>
        /// <value><c>true</c> if result; otherwise, <c>false</c>.</value>
        /// TODO Edit XML Comment Template for Result
        public bool Result { get; private set; }
        /// <summary>Gets or sets the close action.</summary>
        /// <value>The close action.</value>
        /// TODO Edit XML Comment Template for CloseAction
        public Action CloseAction { get; set; }

        /// <summary>The configure enabled</summary>
        /// TODO Edit XML Comment Template for configureEnabled
        private bool _configureEnabled;
        /// <summary>
        /// Gets or sets a value indicating whether [configure enabled].
        /// </summary>
        /// <value><c>true</c> if [configure enabled]; otherwise, <c>false</c>.</value>
        /// TODO Edit XML Comment Template for ConfigureEnabled
        public bool ConfigureEnabled
        {
            get { return _configureEnabled; }
            set
            {
                if (_configureEnabled == value)
                    return;
                _configureEnabled = value;
                RaisePropertyChanged("ConfigureEnabled");
            }
        }

        /// <summary>The mode</summary>
        /// TODO Edit XML Comment Template for mode
        private Collection<ComboBoxItem> _mode = new Collection<ComboBoxItem>();
        /// <summary>Gets the mode.</summary>
        /// <value>The mode.</value>
        /// TODO Edit XML Comment Template for Mode
        public Collection<ComboBoxItem> Mode
        {
            get { return _mode; }
        }

        /// <summary>The selected mode</summary>
        /// TODO Edit XML Comment Template for selectedMode
        private object _selectedMode;
        /// <summary>Gets or sets the selected mode.</summary>
        /// <value>The selected mode.</value>
        /// TODO Edit XML Comment Template for SelectedMode
        public object SelectedMode
        {
            get { return _selectedMode; }
            set
            {
                if (_selectedMode == value)
                    return;
                _selectedMode = value;
                RaisePropertyChanged("SelectedMode");

                PcInterface.Clear();
                SelectedInterface = null;
                RaisePropertyChanged("SelectedInterface");
                Target.Clear();
                RaisePropertyChanged("Target");
                SelectedTarget = null;
                RaisePropertyChanged("SelectedTarget");
                if (value != null)
                {
                    var configurationMode = SelectedMode as ConfigurationMode;
                    if (configurationMode != null)
                        foreach (var pcInterace in configurationMode.PcInterfaces)
                        {
                            PcInterface.Add(new ComboBoxItem(pcInterace.Name, pcInterace));
                        }
                }
                RaisePropertyChanged("PCInterface");
            }
        }

        /// <summary>The pc interface</summary>
        /// TODO Edit XML Comment Template for pcInterface
        private Collection<ComboBoxItem> _pcInterface = new Collection<ComboBoxItem>();
        /// <summary>Gets the pc interface.</summary>
        /// <value>The pc interface.</value>
        /// TODO Edit XML Comment Template for PCInterface
        public Collection<ComboBoxItem> PcInterface
        {
            get { return _pcInterface; }
        }

        /// <summary>The selected interface</summary>
        /// TODO Edit XML Comment Template for selectedInterface
        private object _selectedInterface;
        /// <summary>Gets or sets the selected interface.</summary>
        /// <value>The selected interface.</value>
        /// TODO Edit XML Comment Template for SelectedInterface
        public object SelectedInterface
        {
            get { return _selectedInterface; }
            set
            {
                if (_selectedInterface == value)
                    return;
                _selectedInterface = value;
                RaisePropertyChanged("SelectedInterface");

                Target.Clear();
                SelectedTarget = null;
                RaisePropertyChanged("SelectedTarget");

                if (value != null)
                {
                    var configurationPcInterface = SelectedInterface as ConfigurationPcInterface;
                    if (configurationPcInterface != null)
                        foreach (var pcInterace in configurationPcInterface.TargetInterfaces)
                        {
                            Target.Add(new ComboBoxItem(pcInterace.Name, pcInterace));
                        }
                }
                RaisePropertyChanged("Target");
            }
        }

        /// <summary>The target</summary>
        /// TODO Edit XML Comment Template for target
        private Collection<ComboBoxItem> _target = new Collection<ComboBoxItem>();
        /// <summary>Gets the target.</summary>
        /// <value>The target.</value>
        /// TODO Edit XML Comment Template for Target
        public Collection<ComboBoxItem> Target
        {
            get { return _target; }
        }

        /// <summary>The selected target</summary>
        /// TODO Edit XML Comment Template for selectedTarget
        private object _selectedTarget;
        /// <summary>Gets or sets the selected target.</summary>
        /// <value>The selected target.</value>
        /// TODO Edit XML Comment Template for SelectedTarget
        public object SelectedTarget
        {
            get { return _selectedTarget; }
            set
            {
                if (_selectedTarget == value)
                    return;
                _selectedTarget = value;
                if (value == null)
                {
                    ConfigureEnabled = false;
                }
                else
                {
                    ConfigureEnabled = true;
                }
                RaisePropertyChanged("SelectedTarget");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionConfigurationViewModel"/> class.
        /// </summary>
        /// <param name="plc">The PLC.</param>
        /// TODO Edit XML Comment Template for #ctor
        public ConnectionConfigurationViewModel(OnlineProvider plc)
        {
            _plc = plc;
            Result = false;
            try
            {
                InitializeCommands();
                InitializeProperties();
            }
            catch (Exception)
            {
                CloseAction();
                throw;
            }
        }

        /// <summary>Gets or sets the configure connection command.</summary>
        /// <value>The configure connection command.</value>
        /// TODO Edit XML Comment Template for ConfigureConnectionCommand
        public CommandBase ConfigureConnectionCommand { get; set; }

        /// <summary>Initializes the commands.</summary>
        /// TODO Edit XML Comment Template for InitializeCommands
        private void InitializeCommands()
        {
            ConfigureConnectionCommand = new CommandBase(ConfigureConnectionCommand_Executed);
        }

        /// <summary>
        /// Handles the Executed event of the ConfigureConnectionCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for ConfigureConnectionCommand_Executed
        private void ConfigureConnectionCommand_Executed(object sender, EventArgs e)
        {
            try
            {
                _plc.Configuration.ApplyConfiguration(SelectedTarget as ConfigurationTargetInterface);
                Result = true;
            }
            finally
            {
                CloseAction();
            }
        }

        /// <summary>Initializes the properties.</summary>
        /// TODO Edit XML Comment Template for InitializeProperties
        private void InitializeProperties()
        {
            foreach (var mode in _plc.Configuration.Modes)
            {
                Mode.Add(new ComboBoxItem(mode.Name, mode));
            }
        }

    }
}
