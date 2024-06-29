﻿using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Widgets;
using Mapsui.Widgets.ButtonWidgets;
using System.Collections.Generic;

namespace Mapsui;

public class MapBuilder
{
    readonly List<AddLayer> _layerFactories = [];
    readonly List<AddLayer> _baseLayerFactories = [];
    readonly List<AddWidget> _widgetFactories = [];
    readonly List<ConfigureMap> _mapConfigurators = [];

    public MapBuilder WithMapConfiguration(ConfigureMap configureMap)
    {
        _mapConfigurators.Add(configureMap);
        return this;
    }

    public MapBuilder WithZoomButtons()
    {
        _widgetFactories.Add((m) => new ZoomInOutWidget() { Margin = new MRect(16, 32) });
        return this;
    }

    public MapBuilder WithBaseLayer(AddLayer layerFactory)
    {
        _baseLayerFactories.Add(layerFactory);
        return this;
    }

    public MapBuilder WithLayer(AddLayer layerFactory)
    {
        _layerFactories.Add(layerFactory);
        return this;
    }

    public MapBuilder WithMapCRS(string crs)
    {
        _mapConfigurators.Add((m) => m.CRS = crs);
        return this;
    }

    public MapBuilder WithWidget(AddWidget widgetFactory, ConfigureWidget configureWidget)
    {
        _widgetFactories.Add((m) =>
        {
            var widget = widgetFactory(m);
            configureWidget(widget);
            return widget;
        });
        return this;
    }

    public Map Build()
    {
        var map = new Map();

        foreach (var layerFactory in _layerFactories)
            map.Layers.Add(layerFactory());

        foreach (var widgetFactory in _widgetFactories)
            map.Widgets.Add(widgetFactory(map));

        foreach (var mapConfigurator in _mapConfigurators)
            mapConfigurator(map);

        return map;
    }

    public delegate void ConfigureMap(Map map);
    public delegate void ConfigureWidget(IWidget widget);
    public delegate ILayer AddLayer();
    public delegate IWidget AddWidget(Map map);
}
