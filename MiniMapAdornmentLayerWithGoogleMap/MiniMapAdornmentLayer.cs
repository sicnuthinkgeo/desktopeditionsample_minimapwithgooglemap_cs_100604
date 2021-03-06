﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using ThinkGeo.MapSuite.Core;

namespace MiniMapAdornmentLayerWithGoogleMap
{
    public class MiniMapAdornmentLayer : AdornmentLayer
    {
        private Collection<Layer> layers;
        private int width;
        private int height;

        public MiniMapAdornmentLayer()
            : this(new Layer[] { }, 100, 100)
        { } 

        public MiniMapAdornmentLayer(int width, int height)
            : this(new Layer[] { }, width, height)
        { }

        public MiniMapAdornmentLayer(IEnumerable<Layer> layers, int width, int height)
        {
            this.layers = new Collection<Layer>();
            foreach (Layer layer in layers)
            {
                this.layers.Add(layer);
            }

            this.width = width;
            this.height = height;
        }

        public Collection<Layer> Layers
        {
            get { return layers; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        protected override void DrawCore(GeoCanvas canvas, Collection<SimpleCandidate> labelsInAllLayers)
        {
            GeoImage miniImage = new GeoImage(width, height);
            RectangleShape scaledWorldExtent = MapEngine.GetDrawingExtent(canvas.CurrentWorldExtent, width, height);
            scaledWorldExtent.ScaleUp(300);
            GdiPlusGeoCanvas minCanvas = new GdiPlusGeoCanvas();

            minCanvas.BeginDrawing(miniImage, scaledWorldExtent, canvas.MapUnit);
            foreach (Layer layer in layers)
            {
                layer.Draw(minCanvas, labelsInAllLayers);
            }

            minCanvas.DrawArea(RectangleShape.ScaleDown(minCanvas.CurrentWorldExtent, 1), new GeoPen(GeoColor.StandardColors.Gray, 2), DrawingLevel.LevelOne);
            minCanvas.DrawArea(canvas.CurrentWorldExtent, new GeoPen(GeoColor.StandardColors.Black, 2), DrawingLevel.LevelOne);

            minCanvas.EndDrawing();

            ScreenPointF drawingLocation = GetDrawingLocation(canvas, width, height);

            canvas.DrawScreenImageWithoutScaling(miniImage, (drawingLocation.X + width / 2) + 10, (drawingLocation.Y + height / 2) - 10, DrawingLevel.LevelOne, 0, 0, 0);
        }
    }
}
