﻿#pragma checksum "..\..\..\..\UserControls\Message\MessageControl.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "B92EC5C99F33E4D58F66999457BE58A784AFDDBB"
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
using TheSystem.UserControls.Message;


namespace TheSystem.UserControls.Message {
    
    
    /// <summary>
    /// MessageControl
    /// </summary>
    public partial class MessageControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 8 "..\..\..\..\UserControls\Message\MessageControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal TheSystem.UserControls.Message.MessageControl container;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\..\UserControls\Message\MessageControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel content;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\..\UserControls\Message\MessageControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button backButton;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\..\UserControls\Message\MessageControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textBoxMessage;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\..\UserControls\Message\MessageControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button sendButton;
        
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
            System.Uri resourceLocater = new System.Uri("/TheSystem;component/usercontrols/message/messagecontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\UserControls\Message\MessageControl.xaml"
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
            this.container = ((TheSystem.UserControls.Message.MessageControl)(target));
            
            #line 8 "..\..\..\..\UserControls\Message\MessageControl.xaml"
            this.container.KeyUp += new System.Windows.Input.KeyEventHandler(this.Container_KeyUp);
            
            #line default
            #line hidden
            return;
            case 2:
            this.content = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 3:
            this.backButton = ((System.Windows.Controls.Button)(target));
            
            #line 29 "..\..\..\..\UserControls\Message\MessageControl.xaml"
            this.backButton.Click += new System.Windows.RoutedEventHandler(this.ButtonBack_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.textBoxMessage = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.sendButton = ((System.Windows.Controls.Button)(target));
            
            #line 33 "..\..\..\..\UserControls\Message\MessageControl.xaml"
            this.sendButton.Click += new System.Windows.RoutedEventHandler(this.ButtonSend_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
