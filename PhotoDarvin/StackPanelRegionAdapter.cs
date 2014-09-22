using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Regions;

namespace PhotoDarvin
{
    public class StackPanelRegionAdapter : RegionAdapterBase<StackPanel>
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="behaviorFactory">Allows the registration of the default set of RegionBehaviors.</param>
        public StackPanelRegionAdapter(IRegionBehaviorFactory behaviorFactory)
            : base(behaviorFactory)
        {
        }

        /// <summary>
        /// Adapts a ContentControl to an IRegion. 
        /// </summary>
        /// <param name="region">The new region being used.</param>
        /// <param name="regionTarget">The object to adapt.</param>
        protected override void Adapt(IRegion region, StackPanel regionTarget)
        {
            region.Views.CollectionChanged += (sender, e) =>
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (FrameworkElement element in e.NewItems)
                        {
                            regionTarget.Children.Add(element);
                        }
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        foreach (UIElement elementLoopVariable in e.OldItems)
                        {
                            var element = elementLoopVariable;
                            if (regionTarget.Children.Contains(element))
                            {
                                regionTarget.Children.Remove(element);
                            }
                        }
                        break;
                }
            };
        }

        /// <summary>
        /// Template method to create a new instance of IRegion that will be used to adapt the object. 
        /// </summary>
        /// <returns>A new instance of IRegion.</returns>
        protected override Microsoft.Practices.Prism.Regions.IRegion CreateRegion()
        {
            return new AllActiveRegion();
        }
    }
}
