using LocalizationTest.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LocalizationTest
{
    public class ResourceProvider : INotifyPropertyChanged
    {
        private static List<WeakReference> _providers = new List<WeakReference>();

        private static StringResources _stringResources = new StringResources();
        public StringResources StringResources
        {
            get
            {
                return _stringResources;
            }
        }

        public ResourceProvider()
        {
            Register(this);
        }

        public static void ChangeCulture(string cultureName)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(cultureName);

            _providers.ForEach(wr => 
            {
                if (wr.IsAlive)
                {
                    (wr.Target as ResourceProvider).RaiseResouresChanged();
                }
             });
                
            RemoveDeadReferences();
        }

        private void RaiseResouresChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StringResources)));
        }

        private static void RemoveDeadReferences()
        {
            _providers = new List<WeakReference>(_providers.Where(wr => wr.IsAlive));
        }

        private static void Register(ResourceProvider resourceProvider)
        {
            RemoveDeadReferences();

            if (!_providers.Any(wr => wr.Target == resourceProvider))
            {
                _providers.Add(new WeakReference(resourceProvider));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
