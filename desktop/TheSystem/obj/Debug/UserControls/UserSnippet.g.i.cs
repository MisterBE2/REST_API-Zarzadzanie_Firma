﻿#pragma checksum "..\..\..\UserControls\UserSnippet.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8726189567D7580331E7ED44616A24D22A6C3013"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ten kod został wygenerowany przez narzędzie.
//     Wersja wykonawcza:4.0.30319.42000
//
//     Zmiany w tym pliku mogą spowodować nieprawidłowe zachowanie i zostaną utracone, jeśli
//     kod zostanie ponownie wygenerowany.
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
using TheSystem.UserControls;


namespace TheSystem.UserControls {
    
    
    /// <summary>
    /// UserSnippet
    /// </summary>
    public partial class UserSnippet : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 13 "..\..\..\UserControls\UserSnippet.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label labelUser;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\UserControls\UserSnippet.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label labelADM;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\UserControls\UserSnippet.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock textBlockPosition;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\UserControls\UserSnippet.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock textBlockStatus;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\UserControls\UserSnippet.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button buttonPM;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\UserControls\UserSnippet.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button buttonEdit;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\UserControls\UserSnippet.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button buttonDelete;
        
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
            System.Uri resourceLocater = new System.Uri("/TheSystem;component/usercontrols/usersnippet.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\UserControls\UserSnippet.xaml"
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
            this.labelUser = ((System.Windows.Controls.Label)(target));
            return;
            case 2:
            this.labelADM = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.textBlockPosition = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.textBlockStatus = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.buttonPM = ((System.Windows.Controls.Button)(target));
            
            #line 31 "..\..\..\UserControls\UserSnippet.xaml"
            this.buttonPM.Click += new System.Windows.RoutedEventHandler(this.ButtonPM_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.buttonEdit = ((System.Windows.Controls.Button)(target));
            
            #line 32 "..\..\..\UserControls\UserSnippet.xaml"
            this.buttonEdit.Click += new System.Windows.RoutedEventHandler(this.ButtonEdit_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.buttonDelete = ((System.Windows.Controls.Button)(target));
            
            #line 33 "..\..\..\UserControls\UserSnippet.xaml"
            this.buttonDelete.Click += new System.Windows.RoutedEventHandler(this.ButtonDelete_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

