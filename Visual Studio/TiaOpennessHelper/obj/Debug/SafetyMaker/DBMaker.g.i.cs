﻿#pragma checksum "..\..\..\SafetyMaker\DBMaker.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "47BC71936041DBC199B3FF2B7964134DF22458F5F398B6516C80FDB5D8158BA0"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace TiaOpennessHelper.SafetyMaker {
    
    
    /// <summary>
    /// DBMaker
    /// </summary>
    public partial class DBMaker : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 17 "..\..\..\SafetyMaker\DBMaker.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cbImportToTia;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\..\SafetyMaker\DBMaker.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock Saving;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\..\SafetyMaker\DBMaker.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Forms.Integration.WindowsFormsHost WindowsForm_Left;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\..\SafetyMaker\DBMaker.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Forms.Integration.WindowsFormsHost WindowsForm;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/TiaOpennessHelper;component/safetymaker/dbmaker.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\SafetyMaker\DBMaker.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 9 "..\..\..\SafetyMaker\DBMaker.xaml"
            ((TiaOpennessHelper.SafetyMaker.DBMaker)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.cbImportToTia = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 3:
            
            #line 18 "..\..\..\SafetyMaker\DBMaker.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_CreateBD_SPS_SCH_SAF_OBJECT);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 30 "..\..\..\SafetyMaker\DBMaker.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_SaveCurrentValues);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 42 "..\..\..\SafetyMaker\DBMaker.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_ClearGrid);
            
            #line default
            #line hidden
            return;
            case 6:
            this.Saving = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            this.WindowsForm_Left = ((System.Windows.Forms.Integration.WindowsFormsHost)(target));
            return;
            case 8:
            this.WindowsForm = ((System.Windows.Forms.Integration.WindowsFormsHost)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

