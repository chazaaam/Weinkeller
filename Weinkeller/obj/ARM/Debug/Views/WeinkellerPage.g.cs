﻿#pragma checksum "C:\Users\User\Documents\GitHub\Weinkeller\Weinkeller\Views\WeinkellerPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "3D516C846621BCCB7C11503FB5F3E90E"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Weinkeller.Views
{
    partial class WeinkellerPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1: // Views\WeinkellerPage.xaml line 1
                {
                    global::Windows.UI.Xaml.Controls.Page element1 = (global::Windows.UI.Xaml.Controls.Page)(target);
                    ((global::Windows.UI.Xaml.Controls.Page)element1).Loaded += this.Page_Loaded;
                }
                break;
            case 2: // Views\WeinkellerPage.xaml line 17
                {
                    this.MainGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 3: // Views\WeinkellerPage.xaml line 18
                {
                    this.WineImage = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 4: // Views\WeinkellerPage.xaml line 19
                {
                    this.InfoGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 5: // Views\WeinkellerPage.xaml line 45
                {
                    this.text_page = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 6: // Views\WeinkellerPage.xaml line 46
                {
                    this.btn_back = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.btn_back).Click += this.Btn_back_Click;
                }
                break;
            case 7: // Views\WeinkellerPage.xaml line 47
                {
                    this.btn_next = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.btn_next).Click += this.Btn_next_Click;
                }
                break;
            case 8: // Views\WeinkellerPage.xaml line 48
                {
                    this.grid_empty = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 9: // Views\WeinkellerPage.xaml line 25
                {
                    this.text_type = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 10: // Views\WeinkellerPage.xaml line 27
                {
                    this.text_Quantity = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 11: // Views\WeinkellerPage.xaml line 29
                {
                    this.text_Vendor = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 12: // Views\WeinkellerPage.xaml line 31
                {
                    this.text_Origin = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 13: // Views\WeinkellerPage.xaml line 33
                {
                    this.text_descr = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 14: // Views\WeinkellerPage.xaml line 42
                {
                    this.text_barcode = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 15: // Views\WeinkellerPage.xaml line 43
                {
                    this.Text_Name = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

