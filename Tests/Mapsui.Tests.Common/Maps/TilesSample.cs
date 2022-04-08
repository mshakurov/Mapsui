﻿using System.Collections.Generic;
using System.Threading.Tasks;
using BruTile;
using Mapsui.Layers;
using Mapsui.Providers;
using Mapsui.Samples.Common;
using Mapsui.Styles;
using Mapsui.Tiling.Extensions;
using Mapsui.UI;

namespace Mapsui.Tests.Common.Maps
{
    public class TilesSample : AsyncSampleBase
    {
        public override string Name => "Tiles";
        public override string Category => "Tests";

        public override async Task SetupAsync(IMapControl mapControl)
        {
            mapControl.Map = await CreateMapAsync();
        }

        public static async Task<Map> CreateMapAsync()
        {
            var layer = await CreateLayerAsync();

            var map = new Map
            {
                BackColor = Color.FromString("WhiteSmoke"),
                Home = n => n.NavigateToFullEnvelope()
            };

            map.Layers.Add(layer);

            return map;
        }

        private static async Task<MemoryLayer> CreateLayerAsync()
        {
            var tileIndexes = new[]
            {
                new TileIndex(0, 0, 1),
                new TileIndex(1, 0, 1),
                new TileIndex(0, 1, 1),
                new TileIndex(1, 1, 1)
            };

            var features = await TileIndexToFeaturesAsync(tileIndexes, new SampleTileSource());
            var layer = new MemoryLayer { DataSource = new MemoryProvider<RasterFeature>(features), Name = "Tiles" };
            layer.Style = new RasterStyle();
            return layer;
        }

        private static async Task<List<RasterFeature>> TileIndexToFeaturesAsync(TileIndex[] tileIndexes, ITileSource tileSource)
        {
            var features = new List<RasterFeature>();
            foreach (var tileIndex in tileIndexes)
            {
                var tileInfo = new TileInfo
                {
                    Index = tileIndex,
                    Extent = TileTransform.TileToWorld(
                        new TileRange(tileIndex.Col, tileIndex.Row), tileIndex.Level, tileSource.Schema)
                };

                var raster = new MRaster(await tileSource.GetTileAsync(tileInfo), tileInfo.Extent.ToMRect());
                features.Add(new RasterFeature(raster));
            }
            return features;
        }
    }
}