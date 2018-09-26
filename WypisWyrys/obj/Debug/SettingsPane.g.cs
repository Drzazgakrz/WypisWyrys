﻿#pragma checksum "..\..\SettingsPane.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0BDEA69251D13DA76F815B33ED7BECDE34E54960"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using ArcGIS.Desktop.Extensions;
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
using WypisWyrys;


namespace WypisWyrys {
    
    
    /// <summary>
    /// SettingsPaneView
    /// </summary>
    public partial class SettingsPaneView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 24 "..\..\SettingsPane.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button certificatePath;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\SettingsPane.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button documentPath;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\SettingsPane.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DockPanel dockPanel1;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\SettingsPane.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox scale;
        
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
            System.Uri resourceLocater = new System.Uri("/WypisWyrys;component/settingspane.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\SettingsPane.xaml"
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
            this.certificatePath = ((System.Windows.Controls.Button)(target));
            
            #line 24 "..\..\SettingsPane.xaml"
            this.certificatePath.Click += new System.Windows.RoutedEventHandler(this.setCertificatePath);
            
            #line default
            #line hidden
            return;
            case 2:
            this.documentPath = ((System.Windows.Controls.Button)(target));
            
            #line 27 "..\..\SettingsPane.xaml"
            this.documentPath.Click += new System.Windows.RoutedEventHandler(this.setDocumentPath);
            
            #line default
            #line hidden
            return;
            case 3:
            this.dockPanel1 = ((System.Windows.Controls.DockPanel)(target));
            return;
            case 4:
            
            #line 30 "..\..\SettingsPane.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.setLayersSettings);
            
            #line default
            #line hidden
            return;
            case 5:
            this.scale = ((System.Windows.Controls.ComboBox)(target));
            
            #line 34 "..\..\SettingsPane.xaml"
            this.scale.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.saveScale);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 43 "..\..\SettingsPane.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.changeResolutionsSettings);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

