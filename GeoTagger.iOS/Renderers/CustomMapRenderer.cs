using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using GeoTagger.Controls;
using GeoTagger.iOS.Renderers;
using MapKit;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace GeoTagger.iOS.Renderers
{
    public class CustomMapRenderer : MapRenderer
    {
        UIView customPinView;
        CustomMap formsMap;

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                var nativeMap = Control as MKMapView;
                nativeMap.GetViewForAnnotation = null;
                nativeMap.DidSelectAnnotationView -= OnDidSelectAnnotationView;
                nativeMap.DidDeselectAnnotationView -= OnDidDeselectAnnotationView;
            }

            if (e.NewElement != null)
            {
                formsMap = (CustomMap)e.NewElement;
                var nativeMap = Control as MKMapView;

                nativeMap.GetViewForAnnotation = GetViewForAnnotation;
                nativeMap.DidSelectAnnotationView += OnDidSelectAnnotationView;
                nativeMap.DidDeselectAnnotationView += OnDidDeselectAnnotationView;
            }
        }

        private UIImage FromUrl(string uri)
        {
            using (var url = new NSUrl(uri))
            using (var data = NSData.FromUrl(url))
                return UIImage.LoadFromData(data);
        }

        void OnDidSelectAnnotationView(object sender, MKAnnotationViewEventArgs e)
        {
            customPinView = new UIView();

            var customPin = GetCustomPin(e.View.Annotation);

            customPinView.Frame = new CGRect(0, 0, 200, 84);
            var image = new UIImageView(new CGRect(0, 0, 200, 84));
            image.Image = FromUrl(customPin.Url);
            customPinView.AddSubview(image);
            customPinView.Center = new CGPoint(0, -(e.View.Frame.Height + 75));
            e.View.AddSubview(customPinView);
        }

        void OnDidDeselectAnnotationView(object sender, MKAnnotationViewEventArgs e)
        {
            if (!e.View.Selected)
            {
                customPinView.RemoveFromSuperview();
                customPinView.Dispose();
                customPinView = null;
            }
        }

        CustomPin GetCustomPin(IMKAnnotation annotation)
        {
            var position = new Position(annotation.Coordinate.Latitude, annotation.Coordinate.Longitude);
            foreach (var pin in formsMap.CustomPins)
            {
                if (pin.Position == position)
                {
                    return pin;
                }
            }
            return null;
        }
    }
}
