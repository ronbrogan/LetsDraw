using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Foundation.Serialization;
using Newtonsoft.Json;

namespace SceneComposer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            JsonConvert.DefaultSettings = () =>
            {
                var settings = new JsonSerializerSettings();

                settings.Converters.Add(new Vector2Converter());
                settings.Converters.Add(new Vector3Converter());
                settings.Converters.Add(new Matrix3Converter());
                settings.Converters.Add(new Matrix4Converter());
                settings.Converters.Add(new QuaternionConverter());

                settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

                settings.ContractResolver = new WritablePropsOnlyResolver();

                return settings;
            };


            base.OnStartup(e);
        }
    }
}
