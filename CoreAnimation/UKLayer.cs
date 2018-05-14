using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace CoreAnimation
{
    public sealed class UKLayer : Control
    {
        public static readonly DependencyProperty CaLayerCollectionProperty = DependencyProperty.Register(
            "CALayerCollection", typeof(CALayerCollection), typeof(UKLayer), new PropertyMetadata(default(CALayerCollection)));

        public CALayerCollection CALayerCollection
        {
            get { return (CALayerCollection) GetValue(CaLayerCollectionProperty); }
            set { SetValue(CaLayerCollectionProperty, value); }
        }
        public UKLayer()
        {
            this.DefaultStyleKey = typeof(UKLayer);     
        }
    }
}
