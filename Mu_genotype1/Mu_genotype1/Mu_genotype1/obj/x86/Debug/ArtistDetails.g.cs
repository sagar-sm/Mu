﻿

#pragma checksum "C:\Users\Sagar\Mu\Mu_genotype1\Mu_genotype1\Mu_genotype1\ArtistDetails.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "1A03FA9891F7505836952678DB6C54A9"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace Mu_genotype1
{
    public partial class ArtistDetails : Mu_genotype1.Common.LayoutAwarePage, IComponentConnector
    {
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 58 "..\..\..\ArtistDetails.xaml"
                ((Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.ItemListView_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 40 "..\..\..\ArtistDetails.xaml"
                ((Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.GoBack;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


