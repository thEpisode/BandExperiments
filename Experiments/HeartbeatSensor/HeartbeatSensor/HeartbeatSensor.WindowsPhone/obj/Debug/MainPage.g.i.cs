﻿

#pragma checksum "C:\Users\Camilo\thEpisode Git\BandExperiments\Experiments\HeartbeatSensor\HeartbeatSensor\HeartbeatSensor.WindowsPhone\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "EE57C97FE6C6130B3DA9FBFDD0B70367"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HeartbeatSensor
{
    partial class MainPage : global::Windows.UI.Xaml.Controls.Page
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Media.Animation.Storyboard HeartbeatAnimation; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Media.Animation.Storyboard HeartAppear; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Media.Animation.Storyboard StopAppear; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Media.Animation.Storyboard StartAppear; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Button ConnectButton; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Button StopButton; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Viewbox backgroundGrid; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Viewbox viewbox; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.TextBlock HeartbeatOutput; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Shapes.Path Stroke; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private bool _contentLoaded;

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent()
        {
            if (_contentLoaded)
                return;

            _contentLoaded = true;
            global::Windows.UI.Xaml.Application.LoadComponent(this, new global::System.Uri("ms-appx:///MainPage.xaml"), global::Windows.UI.Xaml.Controls.Primitives.ComponentResourceLocation.Application);
 
            HeartbeatAnimation = (global::Windows.UI.Xaml.Media.Animation.Storyboard)this.FindName("HeartbeatAnimation");
            HeartAppear = (global::Windows.UI.Xaml.Media.Animation.Storyboard)this.FindName("HeartAppear");
            StopAppear = (global::Windows.UI.Xaml.Media.Animation.Storyboard)this.FindName("StopAppear");
            StartAppear = (global::Windows.UI.Xaml.Media.Animation.Storyboard)this.FindName("StartAppear");
            ConnectButton = (global::Windows.UI.Xaml.Controls.Button)this.FindName("ConnectButton");
            StopButton = (global::Windows.UI.Xaml.Controls.Button)this.FindName("StopButton");
            backgroundGrid = (global::Windows.UI.Xaml.Controls.Viewbox)this.FindName("backgroundGrid");
            viewbox = (global::Windows.UI.Xaml.Controls.Viewbox)this.FindName("viewbox");
            HeartbeatOutput = (global::Windows.UI.Xaml.Controls.TextBlock)this.FindName("HeartbeatOutput");
            Stroke = (global::Windows.UI.Xaml.Shapes.Path)this.FindName("Stroke");
        }
    }
}



