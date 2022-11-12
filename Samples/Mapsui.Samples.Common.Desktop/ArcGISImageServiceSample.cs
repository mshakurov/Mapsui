﻿using System.Threading.Tasks;
using Mapsui.ArcGIS;
using Mapsui.ArcGIS.ImageServiceProvider;
using Mapsui.Cache;
using Mapsui.Layers;
using Mapsui.Logging;
using Mapsui.Styles;

namespace Mapsui.Samples.Common.Desktop
{
    public class ArcGISImageServiceSample : ISample
    {
        private ArcGISImageCapabilities? _capabilities;

        public string Name => "9 ArcGIS image";
        public string Category => "Desktop";
        
        public static IUrlPersistentCache? DefaultCache { get; set; }

        public async Task<ILayer> CreateLayerAsync()
        {
            var layer = new ImageLayer("ArcGISImageServiceLayer")
            {
                DataSource = await CreateProviderAsync(DefaultCache)
            };

            layer.Style = new RasterStyle();
            return layer;
        }

        public async Task<Map> CreateMapAsync()
        {
            var map = new Map();
            map.Layers.Add(await CreateLayerAsync());
            return map;
        }

        private async Task<ArcGISImageServiceProvider> CreateProviderAsync(IUrlPersistentCache? persistentCache = null)
        {
            // https://landsat2.arcgis.com/arcgis/rest/services/LandsatGLS/MS/ImageServer/exportImage?bbox=-2.00375070672E7%2C-8572530.6034%2C2.0037507842788246E7%2C1.68764993966E7&bboxSR=&size=&imageSR=&time=&format=jpgpng&pixelType=S16&noData=&noDataInterpretation=esriNoDataMatchAny&interpolation=+RSP_BilinearInterpolation&compression=&compressionQuality=&bandIds=&sliceId=&mosaicRule=&renderingRule=&adjustAspectRatio=true&validateExtent=false&lercVersion=1&compressionTolerance=&f=image
            var capabilitiesHelper = new CapabilitiesHelper(persistentCache);
            capabilitiesHelper.CapabilitiesReceived += CapabilitiesReceived;
            capabilitiesHelper.CapabilitiesFailed += capabilitiesHelper_CapabilitiesFailed;
            capabilitiesHelper.GetCapabilities(@"https://landsat2.arcgis.com/arcgis/rest/services/LandsatGLS/MS/ImageServer", CapabilitiesType.ImageServiceCapabilities);

            while (_capabilities == null)
            {
                await Task.Delay(100).ConfigureAwait(false);
            }

            return new ArcGISImageServiceProvider(_capabilities);
        }

        private static void capabilitiesHelper_CapabilitiesFailed(object? sender, System.EventArgs e)
        {
            Logger.Log(LogLevel.Warning, "ArcGISImageService capabilities request failed");
        }

        private void CapabilitiesReceived(object? sender, System.EventArgs e)
        {
            _capabilities = sender as ArcGISImageCapabilities;
        }
    }
}
